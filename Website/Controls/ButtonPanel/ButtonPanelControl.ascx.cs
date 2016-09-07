using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_ButtonPanel_ButtonPanelControl : System.Web.UI.UserControl
{
    public string ImageUrl { get; set; }
    public string Caption { get; set; }
    public string OnClientClick { get; set; }

    public override string ClientID
    {
        get
        {
            return "tbl" + ID;
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);

        Table table = new Table();
        table.ID = ClientID;
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Style.Add("cursor", "pointer");
        table.Attributes.Add("onmouseover", "Event.SetButtonPanelActive(true, '" + ID + "');");
        table.Attributes.Add("onmouseout", "Event.SetButtonPanelActive(false, '" + ID + "');");
        if (!string.IsNullOrEmpty(OnClientClick))
            table.Attributes.Add("onclick", OnClientClick);

        TableCell tcLeft = new TableCell();
        tcLeft.ID = "td" + ID + "Left";
        tcLeft.Style.Add("white-space", "nowrap");
        tcLeft.Width = Unit.Pixel(13);
        tcLeft.Height = Unit.Pixel(33);
        tcLeft.Text = "&nbsp;";

        TableCell tcRight = new TableCell();
        tcRight.ID = "td" + ID + "Right";
        tcRight.Style.Add("white-space", "nowrap");
        tcRight.Width = Unit.Pixel(13);
        tcRight.Height = Unit.Pixel(33);
        tcRight.Text = "&nbsp;";

        TableCell tc = new TableCell();
        tc.ID = "td" + ID;
        tc.Style.Add("white-space", "nowrap");
        tc.Height = Unit.Pixel(33);

        if (!string.IsNullOrEmpty(ImageUrl))
        {
            Image image = new Image();
            image.ImageUrl = ImageUrl;
            image.ImageAlign = ImageAlign.AbsMiddle;
            tc.Controls.Add(image);
        }

        if (!string.IsNullOrEmpty(Caption))
        {
            Label label = new Label();
            label.ID = "l" + ID;
            label.CssClass = "PanelButtonLabel";
            label.Text = Caption;
            tc.Controls.Add(label);
        }

        TableRow tr = new TableRow();
        tr.Cells.Add(tcLeft);
        tr.Cells.Add(tc);
        tr.Cells.Add(tcRight);

        table.Rows.Add(tr);

        StringWriter sw = new StringWriter();
        table.RenderControl(new HtmlTextWriter(sw));
        writer.Write(sw.ToString());
    }
}
