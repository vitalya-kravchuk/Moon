using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MoonCalc;

public enum RhythmType
{
    LunarDay,
    IndividualLunarDay,
    Zodiac,
    Phase,
    Eclipse,
    NewYear
}

public enum YearAnimal
{
    Rat,
    Bull,
    Tiger,
    Rabbit,
    Dragon,
    Snake,
    Horse,
    Ram,
    Monkey,
    Cock,
    Dog,
    Pig
}

public struct RhythmsViewSettings
{
    public Settings Birth;
    public Settings Location;
    public DateTime SelectedDateTime;
    public string OnClientClick;
    public string CssClass;
    public bool Printable;
}

public class RhythmsView
{
    public RhythmsViewSettings settings { get; set; }

    public RhythmsView(RhythmsViewSettings settings)
    {
        this.settings = settings;
    }

    public Table Get(RhythmType rhythmType, DateTime dateTime)
    {
        Table table;
        switch (rhythmType)
        {
            case RhythmType.Eclipse:
                table = GetEclipse(dateTime);
                break;
            case RhythmType.NewYear:
                table = GetNewYear(dateTime.Year);
                break;
            default:
                table = GetDefault(rhythmType, dateTime);
                break;
        }
        table.CellPadding = 3;
        table.CellSpacing = 0;
        table.CssClass = settings.CssClass;
        return table;
    }

    public static YearAnimal GetYearAnimal(int year)
    {
        int y = Math.Abs(year - 1984);
        return (YearAnimal)(y % 12);
    }

    #region Default
    protected Table GetDefault(RhythmType rhythmType, DateTime dateTime)
    {
        DateTime dtStart = DateTime.Now;
        int birthdayLunarDay = 0;
        if (rhythmType == RhythmType.IndividualLunarDay)
        {
            Calculate cInd = Calculate.GetBirthday(settings.Birth);
            if (cInd != null) birthdayLunarDay = cInd.lunarDay;
        }
        List<Calculate> calcList = Calculate.GetList(dateTime, settings.Location, birthdayLunarDay);
        TimeSpan tsElapsed = DateTime.Now - dtStart;
        Logger.Log.Info(tsElapsed.ToString());

        Table table = new Table();
        int beforeMonth = 0;
        DateTime dt = DateTime.MinValue;
        string beforeValue = string.Empty;
        string value = string.Empty;
        bool headerRowAdded = false;
        for (int i = 0; i < calcList.Count; i++)
        {
            switch (rhythmType)
            {
                case RhythmType.LunarDay:
                    dt = calcList[i].dtLunarDay;
                    value = calcList[i].lunarDay.ToString() + ". " + 
                        Res.GetSymbolLunarDay(calcList[i].lunarDay);
                    break;
                case RhythmType.IndividualLunarDay:
                    dt = calcList[i].dtLunarDay;
                    value = calcList[i].individualLunarDay.ToString() + ". " +
                        Res.GetSymbolLunarDay(calcList[i].individualLunarDay);
                    break;
                case RhythmType.Zodiac:
                    dt = calcList[i].dtZodiac;
                    value = Res.GetZodiac((int)calcList[i].zodiac);
                    break;
                case RhythmType.Phase:
                    dt = calcList[i].dtPhase;
                    value = Res.GetPhase((int)calcList[i].phase);
                    break;
            }
            if (beforeValue.Equals(value)) continue;
            beforeValue = value;
            if (beforeMonth != dt.Month) table.Rows.Add(GetHeaderMonthRow(dt));
            beforeMonth = dt.Month;
            if (!headerRowAdded)
            {
                table.Rows.Add(GetHeaderRow(rhythmType));
                headerRowAdded = true;
            }
            table.Rows.Add(GetRow(dt, value));
        }
        return table;
    }

    protected TableHeaderRow GetHeaderMonthRow(DateTime dt)
    {
        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thc = new TableHeaderCell();
        thc.ColumnSpan = 4;
        thc.Text = string.Format("{0}, {1} г.", Helper.GetMonthName(dt.Month), dt.Year);
        thr.Cells.Add(thc);
        thr.CssClass = settings.CssClass + "_month";
        return thr;
    }

    protected TableHeaderRow GetHeaderRow(RhythmType rhythmType)
    {
        TableHeaderRow thr = new TableHeaderRow();
        thr.Cells.Add(new TableHeaderCell() { Text = "Число" });
        thr.Cells.Add(new TableHeaderCell() { Text = "День" });
        switch (rhythmType)
        {
            case RhythmType.LunarDay:
            case RhythmType.IndividualLunarDay:
                thr.Cells.Add(new TableHeaderCell() { Text = "Лунные сутки" });
                break;
            case RhythmType.Zodiac:
                thr.Cells.Add(new TableHeaderCell() { Text = "Зодиак" });
                break;
            case RhythmType.Phase:
                thr.Cells.Add(new TableHeaderCell() { Text = "Фаза" });
                break;
        }
        thr.Cells.Add(new TableHeaderCell() { Text = "Начало" });
        thr.CssClass = settings.CssClass + "_header";
        return thr;
    }

    protected TableRow GetRow(DateTime dt, string value)
    {
        TableRow tr = new TableRow();
        tr.Cells.Add(new TableCell() { Text = dt.Day.ToString() });
        tr.Cells.Add(new TableCell() { Text = Helper.GetAbbreviatedDayName(dt.DayOfWeek) });
        tr.Cells.Add(new TableCell() { Text = value });
        tr.Cells.Add(new TableCell() { Text = dt.TimeOfDay.ToString() });
        SetRowCss(tr, dt);
        SetOnClientClick(tr, dt);
        return tr;
    }
    #endregion

    protected Table GetEclipse(DateTime dateTime)
    {
        DateTime dtStart = DateTime.Now;
        DaylightSavingTime dst = null;
        if (settings.Location.DST)
            dst = DaylightSavingTime.GetDaylightSavingTime(dateTime);
        List<PhaseEclipse> peList = PhaseEclipse.Get(dateTime.Year, 1, 12, settings.Location.TimeZone, dst);
        TimeSpan tsElapsed = DateTime.Now - dtStart;
        Logger.Log.Info(tsElapsed.ToString());

        Table table = new Table();
        TableHeaderRow thr = new TableHeaderRow();
        thr.Cells.Add(new TableHeaderCell() { Text = "Дата" });
        thr.Cells.Add(new TableHeaderCell() { Text = "Время" });
        thr.Cells.Add(new TableHeaderCell() { Text = "Затмение" });
        thr.CssClass = settings.CssClass + "_header";
        table.Rows.Add(thr);
        for (int i = 0; i < peList.Count; i++)
        {
            Eclipse eclipse = peList[i].eclipse;
            if (eclipse == Eclipse.No ||
                eclipse == Eclipse.MoonNo ||
                eclipse == Eclipse.SunNo)
                continue;
            DateTime dt = peList[i].dateTime;
            TableRow tr = new TableRow();
            tr.Cells.Add(new TableCell() { Text = dt.ToString("dd MMMM") });
            tr.Cells.Add(new TableCell() { Text = dt.TimeOfDay.ToString() });
            tr.Cells.Add(new TableCell() { Text = Res.GetString("Eclipse" + eclipse.ToString()) });
            SetRowCss(tr, dt);
            SetOnClientClick(tr, dt);
            table.Rows.Add(tr);
        }
        return table;
    }

    protected Table GetNewYear(int year)
    {
        DateTime dtStart = DateTime.Now;
        List<Calculate> calcList = Calculate.GetList(
            new DateTime(year, 1, 1), settings.Location, 0);
        Calculate calc = calcList.Find(c => (c.lunarDay == 1 && c.dtLunarDay.Day >= 21));
        if (calc == null)
        {
            calcList = Calculate.GetList(
                new DateTime(year, 2, 1), settings.Location, 0);
            calc = calcList.Find(c => (c.lunarDay == 1));
        }
        DateTime dt = calc.dtLunarDay;
        TimeSpan tsElapsed = DateTime.Now - dtStart;
        Logger.Log.Info(tsElapsed.ToString());

        Table table = new Table();
        TableHeaderRow thr = new TableHeaderRow();
        thr.Cells.Add(new TableHeaderCell() { Text = "Дата" });
        thr.Cells.Add(new TableHeaderCell() { Text = "Время" });
        thr.Cells.Add(new TableHeaderCell() { Text = "Год" });
        thr.CssClass = settings.CssClass + "_header";
        table.Rows.Add(thr);
        TableRow tr = new TableRow();
        tr.Cells.Add(new TableCell() { Text = dt.ToString("dd MMMM yyyy") });
        tr.Cells.Add(new TableCell() { Text = dt.TimeOfDay.ToString() });
        tr.Cells.Add(new TableCell() { Text = Res.GetString(GetYearAnimal(dt.Year).ToString()) });
        SetRowCss(tr, dt);
        SetOnClientClick(tr, dt);
        table.Rows.Add(tr);
        return table;
    }

    #region Helper
    void SetOnClientClick(TableRow tr, DateTime dt)
    {
        if (!settings.Printable)
        {
            //dt = dt.AddMinutes(1);
            tr.Attributes["onclick"] =
                string.Format(settings.OnClientClick, dt.ToString("dd.MM.yyyy HH:mm:ss"));
        }
    }
    void SetRowCss(TableRow tr, DateTime dt)
    {
        tr.CssClass = settings.CssClass;
        for (int i = 0; i < tr.Cells.Count; i++)
            tr.Cells[i].CssClass = settings.CssClass;
        if (!settings.Printable && dt == settings.SelectedDateTime)
        {
            tr.CssClass = settings.CssClass + "_selected";
        }
    }
    #endregion
}
