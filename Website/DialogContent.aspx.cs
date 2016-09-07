using System;
using System.IO;
using System.Web.UI;

public partial class DialogContent : System.Web.UI.Page
{
    protected string Path
    {
        get
        {
            return Request.QueryString["path"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        phContent.Controls.Clear();
        if (File.Exists(Server.MapPath(Path)))
        {
            Control control = LoadControl(Path);
            phContent.Controls.Add(control);
        }
    }
}
