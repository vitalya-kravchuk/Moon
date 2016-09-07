using System;
using System.IO;

public partial class Download : System.Web.UI.Page
{
    protected string File
    {
        get
        {
            return Request["file"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.Log.InfoFormat("{0}", File);
        FileInfo fileInfo = new FileInfo(Server.MapPath(File));
        if (fileInfo.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(fileInfo.FullName);
            Response.End();
        }
    }
}
