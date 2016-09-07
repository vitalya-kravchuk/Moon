using System;

public partial class Controls_Content_HealthControl : BaseContentControl
{
    protected override string ContentName
    {
        get
        {
            return "Health";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBook(hlBook, "daily_life");

        SetParagraph(lblParDay, "Лунные сутки", Res.GetLunarDay(Calc.lunarDay));
        SetParagraph(lblParDayInd, "Индивидуальные лунные сутки", Res.GetLunarDay(Calc.individualLunarDay));

        SetText(lblTxtDay, "Day", Calc.lunarDay);
        SetText(lblTxtDayInd, "Day", Calc.individualLunarDay);

        SetLogin(lbLogin, lblTxtDayInd);
    }
}
