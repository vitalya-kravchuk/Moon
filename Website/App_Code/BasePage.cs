using System.Web.UI;
using System;

public class BasePage : Page
{
    #region Site
    public string SitePath
    {
        get
        {
            return "http://" + Request.Url.Authority + ((Request.ApplicationPath != @"/") ? Request.ApplicationPath : "") + "/";
        }
    }
    public string ImagesPath
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
    public string AppDataPathPhysical
    {
        get
        {
            return Server.MapPath("~/App_Data/");
        }
    }
    #endregion
}
