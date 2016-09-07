using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;

public class FontImage : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        #region Parameters

        string name = context.Request["name"];
        int size = Convert.ToInt32(context.Request["size"]);
        FontStyle style = (FontStyle)Convert.ToInt32(context.Request["style"]);
        Color color = ColorTranslator.FromHtml(context.Request["color"]);
        string text = context.Request["text"];

        #endregion

        Font font = FontFactory.LoadFont(name, size, style);
        SolidBrush fontBrush = new SolidBrush(color);

        Bitmap bitmap = new Bitmap(1, 1);
        Graphics graphics = Graphics.FromImage(bitmap);

        int imageWidth = (int)graphics.MeasureString(text, font).Width;
        int imageHeight = font.Height;
        int textOffset = 10;
        imageWidth += textOffset * 2;
        imageHeight += textOffset * 2;

        bitmap = new Bitmap(imageWidth, imageHeight);
        graphics = Graphics.FromImage(bitmap);
        try
        {
            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, font, fontBrush, new Point(textOffset, textOffset));
            graphics.Flush();

            //context.Response.ContentType = "image/png";
            //bitmap.Save(context.Response.OutputStream, ImageFormat.Png);

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);

            context.Response.ContentType = "image/png";
            ms.WriteTo(context.Response.OutputStream);
        }
        finally
        {
            bitmap.Dispose();
            graphics.Dispose();
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
