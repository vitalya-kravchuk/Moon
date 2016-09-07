using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Calendar_Default : BasePage
{
    const string cDescriptions = "Без описания|Общее|Здоровье|Деньги|Красота|Сад";

    #region Bind

    void BindDescriptions()
    {
        string[] list = cDescriptions.Split('|');
        ddlDescription.Items.Clear();
        for (int i = 0; i < list.Length; i++)
            ddlDescription.Items.Add(new ListItem(list[i], i.ToString()));
        ddlDescription.SelectedIndex = 0;
    }

    void BindMonthes()
    {
        ddlMonth.Items.Clear();
        for (int i = 1; i < 13; i++)
        {
            ListItem li = new ListItem(Helper.GetMonthName(i), i.ToString());
            ddlMonth.Items.Add(li);
        }
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
    }

    #endregion

    void UpdateCalendarImage()
    {
        int year;
        int.TryParse(tbYear.Text, out year);
        if (year == 0 || year < 1000)
            year = DateTime.Now.Year;
        tbYear.Text = year.ToString();

        imgCalendar.ImageUrl = string.Format("CalendarImage.ashx?year={0}&month={1}",
            year, ddlMonth.SelectedValue);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDescriptions();
            BindMonthes();
            tbYear.Text = DateTime.Now.Year.ToString();
        }
        UpdateCalendarImage();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateCalendarImage();
    }
}
