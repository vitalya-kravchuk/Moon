using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using MoonCalc;

public partial class Controls_Main_MainControl : BaseControl
{
    #region Properties
    int? _RhythmID = null;
    protected int RhythmID
    {
        get
        {
            if (!_RhythmID.HasValue)
            {
                _RhythmID = Rhythms.GetID(Calc);
                if (_RhythmID == 0)
                    _RhythmID = Rhythms.Save(Calc);
            }
            return _RhythmID.Value;
        }
    }
    protected int NoteID
    {
        get
        {
            int _NoteID;
            int.TryParse(hfNoteID.Value, out _NoteID);
            return _NoteID;
        }
        set
        {
            hfNoteID.Value = value.ToString();
        }
    }
    protected eChapter ChapterName
    {
        get
        {
            eChapter _ChapterName = eChapter.Common;
            try
            {
                _ChapterName = (eChapter)
                    Enum.Parse(typeof(eChapter), hfChapterName.Value);
            }
            catch { }
            return _ChapterName;
        }
        set
        {
            hfChapterName.Value = value.ToString();
        }
    }
    protected int ChapterID
    {
        get
        {
            return Chapters.GetID(ChapterName);
        }
    }
    #endregion

    #region Helper
    void SetTooltip(ref Label label, string tooltip)
    {
        label.Attributes["onmouseover"] = string.Format("tooltip(this, '{0}')", tooltip);
        label.Attributes["onmouseout"] = "tooltipHide(this)";
    }

    void SetRhythmTime(ref Label label, DateTime dtStart, DateTime dtEnd)
    {
        string text, tooltip;
        DateTime dt = DateTime.Now;

        string timeStart = dtStart.TimeOfDay.ToString();
        string start = timeStart;
        if (dt.Day != dtStart.Day)
            start = Helper.GetAbbreviatedDayName(dtStart.DayOfWeek) + " " + timeStart;

        string timeEnd = dtEnd.TimeOfDay.ToString();
        string end = timeEnd;
        if (dt.Day != dtEnd.Day && dtStart.Day != dtEnd.Day)
            end = Helper.GetAbbreviatedDayName(dtEnd.DayOfWeek) + " " + timeEnd;

        if (dtStart != dtEnd)
        {
            text = start + " - " + end;
        }
        else
        {
            text = start;
        }

        if (dtStart.Day != dtEnd.Day)
        {
            tooltip = dtStart.ToShortDateString() + " - " + dtEnd.ToShortDateString();
        }
        else
        {
            tooltip = dtStart.ToShortDateString();
        }

        label.Text = text;
        SetTooltip(ref label, tooltip);
    }
    #endregion

    #region Update
    void UpdateDateTime()
    {
        try
        {
            TimeSpan time = TimeSpan.Parse(tbTime.Text);
            System.DateTime dt = System.DateTime.Parse(tbDate.Text);
            dt = dt.Add(-dt.TimeOfDay);
            dt = dt.Add(time);
            DT = dt;
        }
        catch { }
        Calc = ReCalculate();
    }

    void UpdatePanel()
    {
        bpcLocation.Caption = PersonLocation.PlaceName;

        string sTime = DT.ToString("HH:mm");
        bpcTime.Caption = sTime;
        tbTime.Text = sTime;

        string dayName = Helper.GetDayName(DT.DayOfWeek);
        dayName = dayName.Substring(0, 1).ToUpper() +
            dayName.Substring(1, dayName.Length - 1);

        bpcDate.Caption = dayName + ", " + DT.ToLongDateString();
        tbDate.Text = DT.ToString("dd.MM.yyyy");
        bpcNow.ImageUrl = ImagesPath + "Panel/now.gif";

        bpcLogout.Visible = AreLogin;
        bpcLogout.ImageUrl = ImagesPath + "Panel/logout.gif";

        bpcMyInfo.Caption = Person.Name;
        if (AreLogin)
            bpcMyInfo.OnClientClick = "ShowDialog.MyInfo(false)";
        else
            bpcMyInfo.OnClientClick = "ShowDialog.Login()";
    }

    void UpdateStarSky()
    {
        imgZodiac.ImageUrl = ImagesPath + "Zodiac/" + Calc.zodiac.ToString() + ".jpg";
        string symbol = ((SymbolDay)(int)Calc.lunarDay - 1).ToString();
        imgSymbol.ImageUrl = ImagesPath + "Symbol/" + symbol + ".jpg";
    }

    void UpdateMenu()
    {
        phMenu.Controls.Clear();
        phMenu.Controls.Add(Menu.Get(this, ImagesPath));
    }

    void UpdateMoon()
    {
        int i = ((int)Calc.phase) + 1;
        imgMoon.ImageUrl = ImagesPath + "Moon/moon" + i.ToString() + ".jpg";
    }

    void UpdateRhythmInfo()
    {
        SetRhythmTime(ref lblLunarDayTime, Calc.dtLunarDay, Calc.dtLunarDayEnd);
        lblLunarDay.Text = Res.GetLunarDay(Calc.lunarDay);
        lblIndividualLunarDay.Text = Res.GetLunarDay(Calc.individualLunarDay);

        SetRhythmTime(ref lblZodiacTime, Calc.dtZodiac, Calc.dtZodiacEnd);
        lblZodiac.Text = Res.GetZodiacIn((int)Calc.zodiac);

        SetRhythmTime(ref lblPhaseTime, Calc.dtPhase, Calc.dtPhaseEnd);
        lblPhase.Text = Res.GetPhase((int)Calc.phase);

        SetRhythmTime(ref lblEclipseTime, Calc.dtPhase, Calc.dtPhase);
        lblEclipse.Text = Res.GetString("Eclipse" + Calc.eclipse.ToString());
        bool showEclipse = true;
        if (Calc.eclipse == Eclipse.No ||
            Calc.eclipse == Eclipse.MoonNo ||
            Calc.eclipse == Eclipse.SunNo)
            showEclipse = false;
        if (showEclipse)
        {
            TimeSpan tsExpired = DT - Calc.dtPhase;
            showEclipse = (tsExpired.Hours <= 0);
        }
        trRhythmInfoEclipse.Visible = showEclipse;
        trRhythmInfoEclipseSep.Visible = showEclipse;

        SetRhythmTime(ref lblNewYearTime, Calc.dtLunarDay, Calc.dtLunarDay);
        lblNewYear.Text = Res.GetNewYear() + " " + 
            Res.GetString(RhythmsView.GetYearAnimal(Calc.dtLunarDay.Year).ToString());
        trRhythmInfoNewYear.Visible = Calc.newYear;
        trRhythmInfoNewYearSep.Visible = Calc.newYear;

        if (!AreLogin)
        {
            lblIndividualLunarDay.Text = "Мой индивидуальный день";
            lblIndividualLunarDay.Attributes["onclick"] = "ShowDialog.Login();";
        }
        else
        {
            string symbolInd = ((SymbolDay)(int)Calc.individualLunarDay - 1).ToString();
            string symbol = ((SymbolDay)(int)Calc.lunarDay - 1).ToString();
            lblIndividualLunarDay.Attributes["onmouseover"] =
                string.Format(@"
                    tooltip(this, 'Выбрать индивидуальные лунные сутки');
                    document.getElementById('{0}').src = '{1}';",
                        imgSymbol.ClientID, ImagesPath + "Symbol/" + symbolInd + ".jpg");
            lblIndividualLunarDay.Attributes["onmouseout"] =
                string.Format(@"
                    tooltipHide(this);
                    document.getElementById('{0}').src = '{1}';",
                        imgSymbol.ClientID, ImagesPath + "Symbol/" + symbol + ".jpg");
        }
    }

    void UpdateQuickDaysGo()
    {
        DateTime dtNow = DateTime.Now;
        DateTime dtLeft = DT.AddDays(-1);
        DateTime dtRight = DT.AddDays(1);
        string tooltip = string.Empty;

        if (dtNow.Day == DT.Day)
            tooltip = "вчера";
        else
            tooltip = "предыдущий день";
        tooltip = string.Format("{0}: {1}, {2}",
            tooltip, Helper.GetDayName(dtLeft.DayOfWeek), dtLeft.ToLongDateString());
        lbQuickDaysGoLeft.Attributes["onmouseover"] = string.Format("tooltip(this, '{0}')", tooltip);
        lbQuickDaysGoLeft.Attributes["onmouseout"] = "tooltipHide(this)";

        if (dtNow.Day == DT.Day)
            tooltip = "завтра";
        else
            tooltip = "следующий день";
        tooltip = string.Format("{0}: {1}, {2}",
            tooltip, Helper.GetDayName(dtRight.DayOfWeek), dtRight.ToLongDateString());
        lbQuickDaysGoRight.Attributes["onmouseover"] = string.Format("tooltip(this, '{0}')", tooltip);
        lbQuickDaysGoRight.Attributes["onmouseout"] = "tooltipHide(this)";
    }

    void UpdateMind()
    {
        string[] lines = File.ReadAllLines(AppDataPathPhysical + "Content/Mind.txt", Encoding.Default);
        Random random = new Random();
        int index = random.Next(lines.Length);
        lContentTextMind.Text = lines[index].Trim();
    }

    void UpdateContent()
    {
        hfChapterName.Value = ChapterName.ToString();
        lContentTitleDesc.Text = Res.GetString("ChapterTip" + ChapterName);
        string path = "~/Controls/Content/" + ChapterName + "Control.ascx";
        phContent.Controls.Clear();
        if (File.Exists(Server.MapPath(path)))
        {
            Control control = LoadControl(path);
            phContent.Controls.Add(control);
        }

        if (!AreLogin)
        {
            lbContentNoteShow.OnClientClick = "ShowDialog.Login(); return false;";
        }

        Notes note = null;
        if (AreLogin)
            note = Notes.Get(Person.PersonID, RhythmID, ChapterID);
        NoteID = 0;
        string noteText = string.Empty;
        if (note != null)
        {
            NoteID = note.NoteID;
            noteText = note.Text;
        }
        bool noteEmpty = string.IsNullOrEmpty(noteText);
        lblContentNoteEdit.Text = noteText.Replace("\n", "<br/>");
        tbContentNote.Text = noteText;
        lblContentNoteEdit.Visible = !noteEmpty;
        lbContentNoteShow.Visible = noteEmpty;
    }

    void UpdateAnimationCacheMLC()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<div id='divAnimationCacheMLC' style='display: none'>");
        for (int i = 0; i <= 11; i++)
        {
            sb.AppendFormat("<img src='{0}' />\r\n",
                ImagesPath + "Panel/mlc/" + i.ToString() + ".jpg");
        }
        sb.AppendLine("</div>");
        litAnimationCacheMLC.Text = sb.ToString();
    }

    void Update()
    {
        UpdatePanel();
        UpdateStarSky();
        UpdateMenu();
        UpdateRhythmInfo();
        UpdateMoon();
        UpdateContent();
        UpdateQuickDaysGo();
    }
    #endregion

    void EmailConfirmation()
    {
        string script = string.Empty;

        if (!string.IsNullOrEmpty(EmailConfirm))
        {
            try
            {
                string email = Crypt.DecryptString(EmailConfirm, CryptPassword);
                Persons person = Persons.EmailConfirm(email);
                if (person != null)
                {
                    Person = person;
                    script = @"
                        alert('Email успешно подтвержден');
                        window.location = '" + SitePath + @"';
                    ";
                }
            }
            catch
            {
            }
        }
        if (AreLogin && !Person.IsEmailConfirm)
        {
            script = "ShowDialog.EmailConfirm();";
        }

        if (!string.IsNullOrEmpty(script))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateMind();
        if (FirstVisit)
        {
            UpdateAnimationCacheMLC();
        }
        if (!IsPostBack)
        {
            ChapterName = UserSettings.Chapter;
            EmailConfirmation();
            UpdateDateTime();
            Update();
            if (!AreLogin)
            {
                ShowBallon("Укажите дату и место рождения", bpcMyInfo.ClientID);
            }
        }
    }
    protected void btnUpdateLocation_Click(object sender, EventArgs e)
    {
        DT = Location.GetLocalDateTime(PersonLocation);
        Calc = ReCalculate();
        Update();
    }
    protected void btnUpdateDateTime_Click(object sender, EventArgs e)
    {
        UpdateDateTime();
        Update();
    }
    protected void btnUpdateContent_Click(object sender, EventArgs e)
    {
        try
        {
            ChapterName = (eChapter)
                Enum.Parse(typeof(eChapter), hfChapterName.Value, true);
            SaveUserSettings(ChapterName);
        }
        catch
        {
        }
        UpdateContent();
    }
    protected void btnRecalc_Click(object sender, EventArgs e)
    {
        try
        {
            DT = DateTime.Parse(hfRecalcDateTime.Value);
            Calc = ReCalculate();
        }
        catch
        {
        }
        Update();
    }
    protected void btnNow_Click(object sender, EventArgs e)
    {
        DT = Location.GetLocalDateTime(PersonLocation);
        Calc = ReCalculate();
        Update();
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Logout();
    }
    protected void lbContentNoteSave_Click(Object sender, EventArgs e)
    {
        if (AreLogin)
        {
            Notes note = new Notes()
            {
                NoteID = NoteID,
                PersonID = Person.PersonID,
                RhythmID = RhythmID,
                ChapterID = ChapterID,
                Text = tbContentNote.Text.Trim()
            };
            NoteID = note.Save();
        }
        UpdateContent();
    }
    protected void lbContentNoteCancel_Click(Object sender, EventArgs e)
    {
        UpdateContent();
    }
    protected void lbQuickDaysGoLeft_Click(Object sender, EventArgs e)
    {
        DT = DT.AddDays(-1);
        Calc = ReCalculate();
        Update();
    }
    protected void lbQuickDaysGoRight_Click(Object sender, EventArgs e)
    {
        DT = DT.AddDays(1);
        Calc = ReCalculate();
        Update();
    }
    #endregion
}
