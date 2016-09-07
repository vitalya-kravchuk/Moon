using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public enum MenuItem
{
    Notify,
    Sep1,

    Blog,
    Practice,
    Tips,
    Rhythm,
    Sep2,

    Why,
    Test,
    Books,
    Comment,
    Help,
}

public class Menu
{
    public Control Owner { get; set; }
    public string ImagesPath { get; set; }
    public Table Table { get; set; }

    public Menu(Control owner, string imagesPath, Table table)
    {
        Owner = owner;
        ImagesPath = imagesPath;
        Table = table;
    }

    public static Table Get(Control owner, string imagesPath)
    {
        Table table = new Table();
        table.CellPadding = 0;
        table.CellSpacing = 0;

        Menu m = new Menu(owner, imagesPath, table);
        for (int i = 0; i < Enum.GetValues(typeof(MenuItem)).Length; i++)
        {
            string itemName = ((MenuItem)i).ToString();
            if (itemName.StartsWith("Sep"))
                m.AddSep();
            else
                m.AddItem((MenuItem)i);
        }

        return table;
    }

    void AddItem(MenuItem menuItem)
    {
        string itemName = menuItem.ToString();

        Image img = new Image();
        img.ID = "imgMenu" + itemName;
        img.ImageUrl = ImagesPath + "Menu/" + itemName + ".png";
        string imgClientID = Owner.ClientID + "_" + img.ClientID;

        TableCell tc = new TableCell();
        tc.HorizontalAlign = HorizontalAlign.Center;
        tc.CssClass = "MenuPanel";
        tc.Attributes.Add("onclick", "Event.MenuClick('" + itemName + "')");
        tc.Attributes.Add("onmouseover",
            string.Format("Event.MenuMouseMove(this, '{0}', true); tooltip(this, '{1}');", 
            imgClientID, Res.GetString("MenuTip" + itemName)));
        tc.Attributes.Add("onmouseout",
            string.Format("Event.MenuMouseMove(this, '{0}', false); tooltipHide(this);",
            imgClientID));
        tc.Controls.Add(img);

        TableRow tr = new TableRow();
        tr.Cells.Add(tc);
        Table.Rows.Add(tr);
    }

    void AddSep()
    {
        Image imgLeft = new Image();
        imgLeft.ImageUrl = ImagesPath + "Menu/sep.png";
        TableCell tcLeft = new TableCell();
        tcLeft.HorizontalAlign = HorizontalAlign.Left;
        tcLeft.Controls.Add(imgLeft);

        Image imgRight = new Image();
        imgRight.ImageUrl = ImagesPath + "Menu/sep.png";
        TableCell tcRight = new TableCell();
        tcRight.HorizontalAlign = HorizontalAlign.Right;
        tcRight.Controls.Add(imgRight);

        TableRow tr = new TableRow();
        tr.Cells.Add(tcLeft);
        tr.Cells.Add(tcRight);

        Table table = new Table();
        table.Width = Unit.Percentage(100);
        table.CellPadding = 0;
        table.CellSpacing = 0;
        table.Rows.Add(tr);

        TableCell tcSep = new TableCell();
        tcSep.Height = Unit.Pixel(21);
        tcSep.CssClass = "MenuSep";
        tcSep.Controls.Add(table);

        TableRow trSep = new TableRow();
        trSep.Cells.Add(tcSep);
        Table.Rows.Add(trSep);
    }
}
