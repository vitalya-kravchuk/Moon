using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using DAL;
using MoonCalc;
using Newtonsoft.Json;

public class BaseControl : System.Web.UI.UserControl
{
    protected DateTime DT
    {
        get
        {
            if (Session["DT"] == null)
                Session["DT"] = Location.GetLocalDateTime(PersonLocation);
            return (DateTime)Session["DT"];
        }
        set
        {
            Session["DT"] = value;
        }
    }
    protected Calculate Calc
    {
        get
        {
            if (Session["Calc"] == null)
                Session["Calc"] = ReCalculate();
            return (Calculate)Session["Calc"];
        }
        set
        {
            Session["Calc"] = value;
        }
    }
    protected string ClientIP
    {
        get
        {
            return Request.UserHostAddress;
        }
    }
    protected bool FirstVisit
    {
        get
        {
            bool _FirstVisit = (Request.Cookies["FirstVisit"] == null);
            if (_FirstVisit)
            {
                HttpCookie cookie = new HttpCookie("FirstVisit");
                DateTime dateTime = DateTime.Now;
                cookie.Expires = dateTime.AddYears(5);
                cookie["DateTime"] = dateTime.ToString();
                Response.Cookies.Add(cookie);
            }
            return _FirstVisit;
        }
    }

    #region Site
    protected string SitePath
    {
        get
        {
            return "http://" + Request.Url.Authority + ((Request.ApplicationPath != @"/") ? Request.ApplicationPath : "") + "/";
        }
    }
    protected string ImagesPath
    {
        get
        {
            return SitePath + "App_Themes/" + Page.Theme + "/Images/";
        }
    }
    protected string AppImagesPath
    {
        get
        {
            return SitePath + "App_Images/";
        }
    }
    protected string AppDataPathPhysical
    {
        get
        {
            return Server.MapPath("~/App_Data/");
        }
    }
    protected string WebSiteName
    {
        get
        {
            return ConfigurationManager.AppSettings["WebSiteName"];
        }
    }
    protected string ContactEmail
    {
        get
        {
            return ConfigurationManager.AppSettings["ContactEmail"];
        }
    }
    protected string GoogleAPIKey
    {
        get
        {
            return ConfigurationManager.AppSettings["GoogleAPIKey"];
        }
    }
    protected string CryptPassword
    {
        get
        {
            return ConfigurationManager.AppSettings["CryptPassword"];
        }
    }
    #endregion

    #region Session
    void SetPersonIDSession(int personID)
    {
        Session["PersonID"] = Crypt.EncryptString(personID.ToString(), CryptPassword);
        RemovePersonIDCookie();
    }
    int GetPersonIDSession()
    {
        int personID = 0;
        string cryptPersonID = (Session["PersonID"] == null) ? string.Empty : (string)Session["PersonID"];
        if (cryptPersonID.Length > 3)
        {
            int.TryParse(Crypt.DecryptString(cryptPersonID, CryptPassword), out personID);
        }
        return personID;
    }
    #endregion

    #region Cookies
    void SetPersonIDCookie(int personID)
    {
        HttpCookie cookie = new HttpCookie("Person");
        cookie.Expires = DateTime.Now.AddYears(5);
        string cryptPersonID = Crypt.EncryptString(personID.ToString(), CryptPassword);
        cookie["PersonID"] = cryptPersonID;
        Response.Cookies.Add(cookie);
    }
    int GetPersonIDCookie()
    {
        HttpCookie cookie = Request.Cookies["Person"];
        if (cookie == null)
            return 0;
        else
        {
            int personID = 0;
            if (cookie["PersonID"].Length > 3)
            {
                string cryptPersonID = Crypt.DecryptString(cookie["PersonID"], CryptPassword);
                int.TryParse(cryptPersonID, out personID);
            }
            return personID;
        }
    }
    void RemovePersonIDCookie()
    {
        HttpCookie cookie = new HttpCookie("Person");
        cookie.Expires = DateTime.Now.AddYears(-5);
        Response.Cookies.Add(cookie);
    }

    void SetLocationCookie(Location location)
    {
        HttpCookie cookie = new HttpCookie("Location");
        cookie.Expires = DateTime.Now.AddYears(5);
        cookie["Latitude"] = Helper.DoubleToString(location.Latitude);
        cookie["Longitude"] = Helper.DoubleToString(location.Longitude);
        cookie["PlaceName"] = Server.UrlEncode(location.PlaceName);
        cookie["TimeZone"] = Helper.DoubleToString(location.TimeZone);
        cookie["DST"] = location.DST.ToString();
        cookie["MapType"] = location.MapType.ToString();
        cookie["MapZoom"] = location.MapZoom.ToString();
        Response.Cookies.Add(cookie);
    }
    Location GetLocationCookie()
    {
        HttpCookie cookie = Request.Cookies["Location"];
        if (cookie == null)
            return null;
        else
            return new Location()
            {
                Latitude = Helper.StringToDouble(cookie["Latitude"]),
                Longitude = Helper.StringToDouble(cookie["Longitude"]),
                PlaceName = Server.UrlDecode(cookie["PlaceName"]),
                TimeZone = Helper.StringToDouble(cookie["TimeZone"]),
                DST = Convert.ToBoolean(cookie["DST"]),
                MapType = Convert.ToInt32(cookie["MapType"]),
                MapZoom = Convert.ToInt32(cookie["MapZoom"])
            };
    }
    #endregion

    #region Person
    Persons _Person;
    protected Persons Person
    {
        get
        {
            if (_Person == null)
            {
                int personID;
                if (PersonRemember)
                {
                    personID = GetPersonIDCookie();
                }
                else
                {
                    personID = GetPersonIDSession();
                }

                if (personID > 0)
                {
                    _Person = Persons.GetByID(personID);
                }
                else
                {
                    _Person = new Persons();
                }
            }
            return _Person;
        }
        set
        {
            _Person = value;
            if (PersonRemember)
                SetPersonIDCookie(_Person.PersonID);
            else
                SetPersonIDSession(_Person.PersonID);
        }
    }

    Location _PersonBirth;
    protected Location PersonBirth
    {
        get
        {
            if (_PersonBirth == null && !string.IsNullOrEmpty(Person.Birth))
                _PersonBirth = JsonConvert.DeserializeObject<Location>(Person.Birth);
            return _PersonBirth;
        }
    }

    Location _PersonLocation;
    protected Location PersonLocation
    {
        get
        {
            if (_PersonLocation == null)
            {
                _PersonLocation = GetLocationCookie();
                if (_PersonLocation == null)
                {
                    _PersonLocation = Location.GetByIP(ClientIP);
                    SetLocationCookie(_PersonLocation);
                }
            }
            return _PersonLocation;
        }
        set
        {
            _PersonLocation = value;
            SetLocationCookie(_PersonLocation);
        }
    }

    UserSettings _UserSettings;
    protected UserSettings UserSettings
    {
        get
        {
            if (_UserSettings == null)
            {
                _UserSettings = (Person.PersonID > 0) ? 
                    UserSettings.GetByPersonID(Person.PersonID) : new UserSettings();
            }
            return _UserSettings;
        }
    }

    protected bool PersonRemember
    {
        get
        {
            bool personRemember = false;
            HttpCookie cookie = Request.Cookies["PersonRemember"];
            if (cookie != null)
                personRemember = !string.IsNullOrEmpty(cookie["Value"]);
            return personRemember;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("PersonRemember");
            cookie.Expires = DateTime.Now.AddYears(5);
            cookie["Value"] = value ? DateTime.Now.ToString() : string.Empty;
            Response.Cookies.Add(cookie);
        }
    }
    #endregion

    #region Login
    protected bool Login(string email, string password, bool remember)
    {
        Persons person = Persons.Get(email, password);
        if (person != null)
        {
            PersonRemember = remember;
            Person = person;
            Calc = ReCalculate();
            if (remember)
                SetPersonIDCookie(person.PersonID);
            else
                SetPersonIDSession(person.PersonID);
            return true;
        }
        return false;
    }
    protected void Logout()
    {
        Person = new Persons();
        Calc = ReCalculate();
        Response.Redirect(SitePath);
    }
    protected bool AreLogin
    {
        get
        {
            return Person.PersonID > 0;
        }
    }
    #endregion

    #region Mail
    protected string MailTemplatesPath
    {
        get
        {
            return AppDataPathPhysical + "MailTemplates/";
        }
    }
    protected string EmailConfirmURL
    {
        get
        {
            return SitePath + "?EmailConfirm=" +
                Server.UrlEncode(Crypt.EncryptString(Person.Email, CryptPassword));
        }
    }
    protected string EmailConfirm
    {
        get
        {
            return Request.QueryString["EmailConfirm"];
        }
    }
    #endregion

    protected Calculate ReCalculate()
    {
        DateTime dtStart = DateTime.Now;
        int birthdayLunarDay = 0;
        if (PersonBirth != null)
        {
            Calculate cInd = Calculate.GetBirthday(PersonBirth);
            if (cInd != null) 
                birthdayLunarDay = cInd.lunarDay;
        }
        List<Calculate> cListEx = Calculate.GetList(DT, PersonLocation, birthdayLunarDay, true);
        Calculate calculate = Calculate.Get(cListEx, DT);
        TimeSpan tsElapsed = DateTime.Now - dtStart;
        Logger.Log.InfoFormat("DT: {0}, Elapsed: {1}", DT, tsElapsed);
        return calculate;
    }

    protected void SaveUserSettings(eChapter chapter)
    {
        if (AreLogin)
        {
            UserSettings userSettings = new UserSettings();
            userSettings.PersonID = Person.PersonID;
            userSettings.Chapter = chapter;
            userSettings.Save();
        }
    }

    protected void ShowBallon(string text, string targetID)
    {
        string script = string.Format("Ballon.Show('{0}', '{1}');", text, targetID);
        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script, true);
    }
}
