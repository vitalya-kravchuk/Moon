using System;

public partial class Controls_Content_MoneyControl : BaseContentControl
{
    protected override string ContentName
    {
        get
        {
            return "Money";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBook(hlBook, "rich_money");

        SetParagraph(lblParDay, "Лунные сутки", Res.GetLunarDay(Calc.lunarDay));
        SetParagraph(lblParDayInd, "Индивидуальные лунные сутки", Res.GetLunarDay(Calc.individualLunarDay));

        SetText(lblTxtDay, "Day", Calc.lunarDay);
        SetText(lblTxtDayInd, "Day", Calc.individualLunarDay);

        SetLogin(lbLogin, lblTxtDayInd);
    }
}
