using System;

public partial class Controls_Content_BeautyControl : BaseContentControl
{
    protected override string ContentName
    {
        get
        {
            return "Beauty";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int zodiac = (int)Calc.zodiac;
        SetBook(hlBook, "luna_prognoz");
        SetText(lblTxtZodiac, "Zodiac", zodiac + 1);
    }
}
