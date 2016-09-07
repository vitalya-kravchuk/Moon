using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

public partial class Controls_Menu_BooksControl : BaseControl
{
    protected int CategoryID
    {
        get
        {
            int categoryID;
            int.TryParse(Request["CategoryID"], out categoryID);
            return categoryID;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCategory();
            BindBooks();
        }
    }

    #region Bind
    void BindCategory()
    {
        phCategory.Controls.Clear();
        List<string> catList = new Books(AppDataPathPhysical).GetCategories();
        for (int i = 0; i < catList.Count; i++)
        {
            HyperLink hl = new HyperLink();
            hl.Text = catList[i];
            hl.NavigateUrl = "javascript:parent.ShowDialog.Books(true, '" + i.ToString() + "');";
            hl.Font.Bold = (CategoryID == i);
            phCategory.Controls.Add(hl);
            if (i < catList.Count - 1)
            {
                Label lblSep = new Label();
                lblSep.Text = "&nbsp;|&nbsp;";
                phCategory.Controls.Add(lblSep);
            }
        }
    }

    void BindBooks()
    {
        lvBooks.DataSource = new Books(AppDataPathPhysical).GetByCategoryID(CategoryID);
        lvBooks.DataBind();
    }
    #endregion

    #region Buttons
    protected string GetDownloadButton(object value)
    {
        string downloadURI = value.ToString();
        if (string.IsNullOrEmpty(downloadURI))
            return "";
        FileInfo fileInfo = new FileInfo(Server.MapPath(downloadURI));
        if (!fileInfo.Exists)
            return "";
        string ext = fileInfo.Extension.Remove(0, 1).ToUpper();
        double mb = Math.Round(fileInfo.Length / 1024f / 1024f, 2);
        string tooltip = string.Format("Скачать ({0}, {1} MБ)", ext, Helper.DoubleToString(mb));
        string imgSrc = ImagesPath + "download.jpg";
        return string.Format(
            "<a href='{0}' target='_blank'><img src='{1}' border='0' onmouseover='tooltip(this, \"{2}\");' onmouseout='tooltipHide(this);' /></a>",
            SitePath + "Download.aspx?file=" + downloadURI, imgSrc, tooltip);
    }

    protected string GetOrderButton(object value)
    {
        string orderURL = value.ToString();
        if (string.IsNullOrEmpty(orderURL))
            return "";
        string imgSrc = "http://www.ozon.ru/graphics/img_ass/buttons/button88x31.gif";
        string tooltip = "Купить";
        return string.Format(
            "<a href='{0}' target='_blank'><img src='{1}' border='0' onmouseover='tooltip(this, \"{2}\");' onmouseout='tooltipHide(this);' /></a>",
            orderURL, imgSrc, tooltip);
    }
    #endregion

    #region Helper
    protected string GetShortText(object value)
    {
        string text = value.ToString();
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        int count = text.IndexOf(" ", 250);
        if (count < text.Length && count > 0)
            return text.Substring(0, count) + "...";
        else
            return text;
    }
    #endregion
}
