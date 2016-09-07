using System;
using System.IO;
using System.Text;

public partial class Controls_Content_GardenControl : BaseContentControl
{
    protected override string ContentName
    {
        get
        {
            return "Garden";
        }
    }

    void SetGardenContent()
    {
        string[] content = File.ReadAllLines(GetFilePath("Zodiac"), Encoding.Default);
        string marker = ";" + ((int)Calc.zodiac + 1).ToString();
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i].Equals(marker))
            {
                SetParagraph(lblParDays, content[i + 1], Res.GetZodiacIn((int)Calc.zodiac));
                lblTxtDays.Text = RemovePoint(content[i + 2]);
                int j = 0;
                if (Calc.phase == MoonCalc.Phase.FullMoon ||
                    Calc.phase == MoonCalc.Phase.LastQuarter)
                    j++;
                lblTxtFavorably.Text = FormatList(content[i + 3 + j]);
                lblTxtAdversely.Text = FormatList(content[i + 5]);
                break;
            }
        }
    }

    string FormatList(string content)
    {
        if (string.IsNullOrEmpty(content))
            return string.Empty;
        string[] list = RemovePoint(content).Split(';');
        string res = "<ul>";
        foreach (string s in list)
            res += "<li>" + s + "</li>";
        res += "</ul>";
        return res;
    }

    string RemovePoint(string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            if (s.EndsWith("."))
                return s.Remove(s.Length - 1, 1);
        }
        return s;
    }

    void SetVisible()
    {
        bool Adversely = !string.IsNullOrEmpty(lblTxtAdversely.Text);
        lblParAdversely.Visible = Adversely;
        lblTxtAdversely.Visible = Adversely;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBook(hlBook, "success_calendar");
        SetParagraph(lblParFavorably, "Благоприятно", Res.GetZodiacIn((int)Calc.zodiac));
        SetParagraph(lblParAdversely, "Неблагоприятно", Res.GetZodiacIn((int)Calc.zodiac));
        SetGardenContent();
        SetVisible();
    }
}
