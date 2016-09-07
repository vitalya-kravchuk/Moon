using System;
using System.Web.UI;

public partial class Controls_Menu_SendMailControl : BaseControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (AreLogin)
            {
                tbName.Text = Person.Name;
                tbEmail.Text = Person.Email;
            }
        }
    }

    protected void ibOK_Click(object sender, ImageClickEventArgs e)
    {
        string subject = WebSiteName + ": Отзыв";
        new Mail().Send(tbEmail.Text, tbName.Text, ContactEmail, subject, tbMessage.Text);
        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
            "parent.Dialog.Hide(); alert('Спасибо');", true);
    }
}
