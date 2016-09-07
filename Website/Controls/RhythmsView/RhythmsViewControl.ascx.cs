using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_RhythmsView_RhythmsViewControl : BaseContentControl
{
    #region Properites
    protected RhythmType rhythmType
    {
        get
        {
            RhythmType rt = RhythmType.LunarDay;
            try
            {
                rt = (RhythmType)Enum.Parse(
                    typeof(RhythmType), Request["rhythmType"]);
            }
            catch
            {
            }
            return rt;
        }
    }
    protected int month
    {
        get
        {
            int m = DT.Month;
            try
            {
                m = int.Parse(Request["month"]);
            }
            catch
            {
            }
            return m;
        }
    }
    protected int year
    {
        get
        {
            int y = DT.Year;
            try
            {
                y = int.Parse(Request["year"]);
                if (y < 100) y = 100;
            }
            catch
            {
            }
            return y;
        }
    }
    protected bool print
    {
        get
        {
            return !string.IsNullOrEmpty(Request["print"]);
        }
    }
    #endregion

    #region Bind
    void BindRhythmsView()
    {
        ddlRhythmType.Items.Clear();
        Array arr = Enum.GetValues(typeof(RhythmType));
        for (int i = 0; i < arr.Length; i++)
        {
            bool add = true;
            if (!AreLogin)
            {
                if (arr.GetValue(i).ToString() == RhythmType.IndividualLunarDay.ToString())
                {
                    add = false;
                }
            }
            if (add)
            {
                ListItem li = new ListItem(
                    Res.GetString("rt" + arr.GetValue(i).ToString()), arr.GetValue(i).ToString());
                ddlRhythmType.Items.Add(li);
            }
        }
        ddlRhythmType.SelectedValue = rhythmType.ToString();
    }

    void BindMonth()
    {
        ddlMonth.Items.Clear();
        for (int i = 1; i < 13; i++)
        {
            ListItem li = new ListItem(Helper.GetMonthName(i), i.ToString());
            ddlMonth.Items.Add(li);
        }
        ddlMonth.SelectedValue = month.ToString();
    }
    #endregion

    #region Get
    RhythmType GetRhythmType()
    {
        return (RhythmType)Enum.Parse(
            typeof(RhythmType), ddlRhythmType.SelectedValue);
    }

    DateTime GetDateTime()
    {
        int m = int.Parse(ddlMonth.SelectedValue);

        int y = year;
        try
        {
            y = int.Parse(tbYear.Text);
            if (y < 100) y = 100;
        }
        catch
        {
        }
        tbYear.Text = y.ToString();
        
        return new DateTime(y, m, 1);
    }

    DateTime GetSelectedDateTime()
    {
        DateTime dt;
        switch (GetRhythmType())
        {
            case RhythmType.LunarDay:
            case RhythmType.IndividualLunarDay:
                dt = Calc.dtLunarDay;
                break;
            case RhythmType.Zodiac:
                dt = Calc.dtZodiac;
                break;
            case RhythmType.Phase:
            case RhythmType.Eclipse:
                dt = Calc.dtPhase;
                break;
            default:
                dt = DT;
                break;
        }
        return dt;
    }
    #endregion

    void SetTitle()
    {
        lblTitle.Text = ddlRhythmType.SelectedItem.Text;
        lblPlace.Text = PersonLocation.PlaceName;
        if (GetRhythmType() == RhythmType.IndividualLunarDay)
            lblTitle.Text += " для " + Person.Name;
        if (GetRhythmType() == RhythmType.Eclipse)
            lblTitle.Text += " на " + year.ToString() + " г.";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRhythmsView();
            BindMonth();
            tbYear.Text = year.ToString();
        }

        SetTitle();
        tblSet.Visible = !print;
        tblTitle.Visible = print;
        ddlMonth.Visible = 
            (GetRhythmType() != RhythmType.NewYear && GetRhythmType() != RhythmType.Eclipse);

        RhythmsView rv = new RhythmsView(new RhythmsViewSettings()
        {
            Birth = PersonBirth,
            Location = PersonLocation,
            OnClientClick = "parent.Recalc('{0}'); parent.Dialog.Hide();",
            CssClass = "RhythmsView",
            SelectedDateTime = GetSelectedDateTime(),
            Printable = print
        });
        phRhythms.Controls.Clear();
        phRhythms.Controls.Add(rv.Get(GetRhythmType(), GetDateTime()));

        if (print)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
                "window.print();", true);
        }
    }
}
