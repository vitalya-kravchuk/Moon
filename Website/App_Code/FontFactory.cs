using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;

public class FontFactory
{
    public static string PrivateFontsPath { get; set; }

    static readonly string[] standartFonts =
    {
        "Arial",
        "Verdana",
        "Tahoma",
        "Times New Roman",
        "Comic Sans MS",
    };
    static Dictionary<string, FontFamily> privateFontFamilies = new Dictionary<string, FontFamily>();

    static FontFamily LoadFontFamily(string fileName)
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        pfc.AddFontFile(PrivateFontsPath + fileName);
        return pfc.Families[0];
    }

    public static List<string> GetFontsList(bool includePrivate)
    {
        List<string> fontsList = standartFonts.ToList<string>();
        if (!string.IsNullOrEmpty(PrivateFontsPath) && includePrivate)
        {
            string[] files = Directory.GetFiles(PrivateFontsPath, "*.ttf");
            for (int i = 0; i < files.Length; i++)
                fontsList.Add(Path.GetFileNameWithoutExtension(files[i]));
        }
        fontsList.Sort();
        return fontsList;
    }

    public static Font LoadFont(string fontName, int size, FontStyle style, GraphicsUnit unit)
    {
        fontName = fontName.ToUpper();
        
        bool isStandartFont = false;
        foreach (string standartFontName in standartFonts)
        {
            if (standartFontName.ToUpper().Equals(fontName))
            {
                isStandartFont = true;
                break;
            }
        }

        Font font;
        if (isStandartFont)
        {
            font = new Font(fontName, size, style, unit);
        }
        else
        {
            if (!privateFontFamilies.ContainsKey(fontName))
            {
                FontFamily fontFamily = LoadFontFamily(fontName + ".ttf");
                privateFontFamilies.Add(fontName, fontFamily);
            }
            font = new Font(privateFontFamilies[fontName], size, style, unit);
        }
        return font;
    }

    public static Font LoadFont(string fontName, int size, FontStyle style)
    {
        return LoadFont(fontName, size, style, GraphicsUnit.Pixel);
    }
}
