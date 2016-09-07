using System;

public partial class Controls_Content_CommonControl : BaseContentControl
{
    protected override string ContentName
    {
        get
        {
            return "Common";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBook(hlBook, "daily_life");

        SetParagraph(lblParDay, "Поведение", Res.GetLunarDay(Calc.lunarDay));
        SetParagraph(lblParDayInd, "Индивидуальное поведение", Res.GetLunarDay(Calc.individualLunarDay));
        SetParagraph(lblParDreams, "Сны", Res.GetLunarDay(Calc.lunarDay));
        SetParagraph(lblParPhase, "Активность", Res.GetPhase((int)Calc.phase));
        SetParagraph(lblParZodiac, "Эмоции", Res.GetZodiacIn((int)Calc.zodiac));

        SetText(lblTxtDay, "Day", Calc.lunarDay);
        SetText(lblTxtDayInd, "Day", Calc.individualLunarDay);
        SetText(lblTxtDreams, "Dreams", Calc.lunarDay);
        SetText(lblTxtZodiac, "Zodiac", (int)Calc.zodiac + 1);
        SetText(lblTxtPhase, "Phase", (int)Calc.phase + 1);

        SetLogin(lbLogin, lblTxtDayInd);
    }
}
