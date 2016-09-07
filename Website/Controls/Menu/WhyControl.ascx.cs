using System;

public partial class Controls_Menu_WhyControl : BaseControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Books book = new Books(AppDataPathPhysical).Get("30_lunar_days");
            hlBook.Text = book.Author + ". " + book.Name;
            hlBook.NavigateUrl = book.OrderURL;
        }
    }
}
