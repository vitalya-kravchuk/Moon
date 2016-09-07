using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Newtonsoft.Json;

public partial class Controls_MyInfo_MyInfoControl : BaseControl
{
    #region Properties
    protected double Latitude
    {
        get
        {
            return Helper.StringToDouble(hfLat.Value);
        }
        set
        {
            hfLat.Value = Helper.DoubleToString(value);
        }
    }
    protected double Longitude
    {
        get
        {
            return Helper.StringToDouble(hfLng.Value);
        }
        set
        {
            hfLng.Value = Helper.DoubleToString(value);
        }
    }
    protected int MapType
    {
        get
        {
            return Convert.ToInt32(hfMapType.Value);
        }
        set
        {
            hfMapType.Value = value.ToString();
        }
    }
    protected int MapZoom
    {
        get
        {
            return Convert.ToInt32(hfMapZoom.Value);
        }
        set
        {
            hfMapZoom.Value = value.ToString();
        }
    }
    #endregion

    #region Bind
    void BindYears()
    {
        ddlYear.Items.Clear();
        for (int i = DateTime.Now.Year; i >= 1920; i--)
        {
            ddlYear.Items.Add(new ListItem(i.ToString()));
        }
        ddlYear.Items.Insert(0, new ListItem(""));
    }

    void BindMonthes()
    {
        ddlMonth.Items.Clear();
        for (int i = 1; i <= 12; i++)
        {
            ddlMonth.Items.Add(
                new ListItem(Helper.GetMonthName(i), i.ToString()));
        }
        ddlMonth.Items.Insert(0, new ListItem(""));
    }

    void BindDays()
    {
        int selDay = 0;
        if (ddlDay.SelectedIndex > 0)
            selDay = Convert.ToInt32(ddlDay.SelectedValue);

        ddlDay.Items.Clear();
        int daysCount = 31;
        if (ddlYear.SelectedIndex > 0 && ddlMonth.SelectedIndex > 0)
        {
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            daysCount = DateTime.DaysInMonth(year, month);
        }
        for (int i = 1; i <= daysCount; i++)
        {
            ddlDay.Items.Add(new ListItem(i.ToString()));
        }
        ddlDay.Items.Insert(0, new ListItem(""));

        if (selDay > daysCount)
            selDay = daysCount;
        if (selDay > 0)
            ddlDay.SelectedValue = selDay.ToString();
    }

    void BindTime(DropDownList ddl, int count)
    {
        ddl.Items.Clear();
        for (int i = 0; i <= count; i++)
        {
            string s = i.ToString();
            if (s.Length == 1)
                s = "0" + s;
            ddl.Items.Add(new ListItem(s, i.ToString()));
        }
        ddl.Items.Insert(0, new ListItem(""));
    }

    void BindTime()
    {
        BindTime(ddlHour, 23);
        BindTime(ddlMinute, 59);
    }
    
    void BindTimeZones()
    {
        DataSet ds = new DataSet();
        ds.ReadXml(AppDataPathPhysical + "TimeZones.xml");
        DataView dv = ds.Tables["TimeZone"].DefaultView;
        ddlTimeZone.DataTextField = "Text";
        ddlTimeZone.DataValueField = "Value";
        ddlTimeZone.DataSource = dv;
        ddlTimeZone.DataBind();
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region bind
            BindYears();
            BindMonthes();
            BindDays();
            BindTime();
            BindTimeZones();
            #endregion

            if (AreLogin)
            {
                tbName.Text = Person.Name;
                tbEmail.Text = Person.Email;
                tbPassword.Text = Person.Password;
                tbPasswordConfirm.Text = Person.Password;

                if (PersonBirth != null)
                {
                    ddlMonth.SelectedValue = PersonBirth.Birthday.Month.ToString();
                    ddlYear.SelectedValue = PersonBirth.Birthday.Year.ToString();
                    BindDays();
                    ddlDay.SelectedValue = PersonBirth.Birthday.Day.ToString();

                    ddlHour.SelectedValue = PersonBirth.Birthday.TimeOfDay.Hours.ToString();
                    ddlMinute.SelectedValue = PersonBirth.Birthday.TimeOfDay.Minutes.ToString();
                }

                ibOK.Attributes["onmouseover"] = "tooltip(this, 'Сохранить');";
            }
            else
            {
                ibOK.Attributes["onmouseover"] = "tooltip(this, 'Регистрация');";
            }

            Location birthLocation = PersonBirth;
            if (birthLocation == null)
                birthLocation = Location.GetByIP(ClientIP);
            Latitude = birthLocation.Latitude;
            Longitude = birthLocation.Longitude;
            MapType = birthLocation.MapType;
            MapZoom = birthLocation.MapZoom;
            tbBirthPlace.Text = birthLocation.PlaceName;
            ddlTimeZone.SelectedValue = Helper.DoubleToString(birthLocation.TimeZone);
            cbDST.Checked = birthLocation.DST;
        }
    }

    protected void ibOK_Click(object sender, ImageClickEventArgs e)
    {
        string script = string.Empty;
        string email = tbEmail.Text.Trim().ToLower();
        if (Persons.EmailExists(Person.PersonID, tbEmail.Text))
        {
            script = @"
                parent.Dialog.ShowContent();
                alert('Такой email уже зарегистрирован');
            ";
        }
        else
        {
            int hour = 0;
            if (ddlHour.SelectedIndex > 0)
                hour = Convert.ToInt32(ddlHour.SelectedValue);
            int minute = 0;
            if (ddlMinute.SelectedIndex > 0)
                minute = Convert.ToInt32(ddlMinute.SelectedValue);
            DateTime dtBithday = new DateTime(
                Convert.ToInt32(ddlYear.SelectedValue),
                Convert.ToInt32(ddlMonth.SelectedValue),
                Convert.ToInt32(ddlDay.SelectedValue),
                hour, minute, 0);

            Location location = new Location()
            {
                Birthday = dtBithday,
                Latitude = Latitude,
                Longitude = Longitude,
                PlaceName = tbBirthPlace.Text.Trim(),
                DST = cbDST.Checked,
                TimeZone = Helper.StringToDouble(ddlTimeZone.SelectedValue),
                MapType = MapType,
                MapZoom = MapZoom
            };

            DateTime dtReg = Person.RegistrationDate;
            if (dtReg == DateTime.MinValue)
                dtReg = DateTime.Now;

            bool newPerson = !(Person.PersonID > 0);
            bool emailChanged = (Person.Email != email) && !newPerson;

            Persons person = new Persons()
            {
                PersonID = Person.PersonID,
                Name = tbName.Text.Trim(),
                Email = email,
                Password = tbPassword.Text,
                Birth = JsonConvert.SerializeObject(location),
                RegistrationDate = dtReg,
                IsEmailConfirm = Person.IsEmailConfirm
            };
            if (emailChanged)
                person.IsEmailConfirm = false;

            person.PersonID = person.Save();
            Person = person;

            if (newPerson)
            {
                MailTemplate mtEmailConfirm =
                    new MailTemplate(MailTemplatesPath, eMailTemplates.EmailConfirm).Get(Person.Name, EmailConfirmURL);
                MailTemplate mtNewRegistrationEvent =
                    new MailTemplate(MailTemplatesPath, eMailTemplates.NewRegistrationEvent).Get(
                        Person.Name, PersonLocation.PlaceName, Person.Email, PersonBirth.Birthday, Person.RegistrationDate, Person.PersonID);

                Mail mail = new Mail();
                mail.Send(Person.Email, mtEmailConfirm.Subject, mtEmailConfirm.Body);
                mail.Send(ContactEmail, mtNewRegistrationEvent.Subject, mtNewRegistrationEvent.Body);
            }
            if (emailChanged)
            {
                Logger.Log.InfoFormat("Email changed. PersonID = {0}", Person.PersonID);
                MailTemplate mtEmailChangedConfirm =
                    new MailTemplate(MailTemplatesPath, eMailTemplates.EmailChangedConfirm).Get(EmailConfirmURL);
                new Mail().Send(Person.Email, mtEmailChangedConfirm.Subject, mtEmailChangedConfirm.Body);
            }

            script = "parent.Event.Reload();";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script, true);
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDays();
    }
}
