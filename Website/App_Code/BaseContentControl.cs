using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

public class BaseContentControl : BaseControl
{
    const string LoginText = "Мой персональный гороскоп";

    #region Properties
    protected virtual string ContentName
    {
        get
        {
            return string.Empty;
        }
    }
    protected virtual string DataPath
    {
        get
        {
            return "~/App_Data/Content/" + ContentName + "/";
        }
    }
    protected virtual string FileExt
    {
        get
        {
            return ".txt";
        }
    }
    #endregion

    protected string GetFilePath(string fileName)
    {
        return Server.MapPath(DataPath + fileName + FileExt);
    }

    #region Controls
    protected void SetBook(HyperLink hl, string key)
    {
        Books book = new Books(AppDataPathPhysical).Get(key);
        hl.Text = book.Name + ". " + book.Author;
        hl.NavigateUrl = book.OrderURL;
        hl.Target = "_blank";
    }

    protected void SetParagraph(Label label, string text, string tooltip)
    {
        label.Text = text;
        if (!string.IsNullOrEmpty(tooltip))
        {
            label.Attributes["onmouseover"] = string.Format("tooltip(this, '{0}')", tooltip);
            label.Attributes["onmouseout"] = "tooltipHide(this)";
        }
    }

    protected void SetText(Label label, string fileName, int markerIndex)
    {
        string[] lines = File.ReadAllLines(GetFilePath(fileName), Encoding.Default);
        string marker = ";" + markerIndex;
        label.Text = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Equals(marker))
            {
                label.Text = lines[i + 1];
                break;
            }
        }
    }

    protected void SetLogin(LinkButton lbLogin, Label lblText)
    {
        lbLogin.Text = LoginText;
        lbLogin.OnClientClick = "ShowDialog.Login(); return false;";
        lbLogin.Visible = !AreLogin;
        lblText.Visible = AreLogin;
    }
    #endregion
}
