using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

public enum eMailTemplates
{
    EmailConfirm,
    EmailConfirmRequest,
    EmailChangedConfirm,
    RemindPassword,
    NewRegistrationEvent,
}

enum eMarkers
{
    WebSiteName,
    ContactEmail,
}

public class MailTemplate
{
    const char marker = '%';
    string[] fileContent = null;

    Dictionary<string, string> _Markers = null;
    Dictionary<string, string> Markers
    {
        get
        {
            if (_Markers == null)
            {
                _Markers = new Dictionary<string, string>();
                foreach (string name in Enum.GetNames(typeof(eMarkers)))
                {
                    _Markers.Add(marker + name, ConfigurationManager.AppSettings[name]);
                }
            }
            return _Markers;
        }
    }

    #region Properties
    public string Subject { get; set; }
    public string Body { get; set; }
    #endregion

    public MailTemplate()
    {
    }

    public MailTemplate(string templatePath, eMailTemplates mailTemplate)
    {
        fileContent = File.ReadAllLines(templatePath + mailTemplate.ToString() + ".txt");
    }

    public MailTemplate Get(params object[] args)
    {
        for (int i = 0; i < fileContent.Length; i++)
        {
            ReplaceMarkers(ref fileContent[i]);
            ReplaceMarkersArgs(ref fileContent[i], args);
        }
        return new MailTemplate()
        {
            Subject = fileContent[0],
            Body = GetText(fileContent, 2)
        };
    }

    void ReplaceMarkers(ref string s)
    {
        foreach (KeyValuePair<string, string> kvp in Markers)
            s = s.Replace(kvp.Key, kvp.Value);
    }

    void ReplaceMarkersArgs(ref string s, params object[] args)
    {
        for (int i = 0; i < args.Length; i++)
            s = s.Replace(marker + i.ToString(), Convert.ToString(args[i]));
    }

    string GetText(string[] lines, int startIndex)
    {
        string text = "";
        for (int i = startIndex; i < lines.Length; i++)
            text += lines[i] + "\r\n";
        return text;
    }
}
