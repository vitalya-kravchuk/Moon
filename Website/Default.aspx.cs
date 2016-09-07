using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web;

public partial class _Default : BasePage
{
    #region Meta
    string ContentPath
    {
        get
        {
            return AppDataPathPhysical + "meta/";
        }
    }

    HtmlMeta GetAuthor()
    {
        return new HtmlMeta()
        {
            Name = "author",
            Content = "Кравчук Виталий Борисович"
        };
    }

    HtmlMeta GetKeywords()
    {
        string content = File.ReadAllText(ContentPath + "keywords.txt");
        content = content.Replace("<%year%>", 
            DateTime.Now.Year.ToString());
        content = content.Replace("<%month%>", 
            Helper.GetMonthName(DateTime.Now.Month).ToLower());
        content = content.Replace("\r\n", ", ");
        return new HtmlMeta()
        {
            Name = "keywords",
            Content = content
        };
    }

    HtmlMeta GetDescription()
    {
        return new HtmlMeta()
        {
            Name = "description",
            Content = File.ReadAllText(ContentPath + "description.txt")
        };
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer != null)
        {
            string urlReferrer = Request.UrlReferrer.ToString();
            if (urlReferrer.IndexOf(SitePath) < 0)
                Logger.Log.InfoFormat("URL Referrer: {0}", urlReferrer);
        }

        phMeta.Controls.Clear();
        phMeta.Controls.Add(GetAuthor());
        phMeta.Controls.Add(GetKeywords());
        phMeta.Controls.Add(GetDescription());
    }
}
