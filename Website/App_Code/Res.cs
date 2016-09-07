using System;
using Resources;
using System.Collections.Generic;

public class Res
{
    #region Elements
    static string[] GetElements(string s)
    {
        return s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    }

    static string GetElement(string s, int index)
    {
        string[] elements = GetElements(s);
        if (index >= 0 && index < elements.Length)
            return elements[index];
        else
            return string.Empty;
    }

    static int GetElementsCount(string s)
    {
        return GetElements(s).Length;
    }
    #endregion

    public static string GetString(string name)
    {
        return Strings.ResourceManager.GetString(name);
    }

    public static string GetSymbolLunarDay(int lunarDay)
    {
        return GetElement(Strings.SymbolOfLunarDay, lunarDay - 1);
    }

    public static string GetLunarDay(int lunarDay)
    {
        if (lunarDay > 0)
        {
            return lunarDay.ToString() + " лунные сутки. " +
                GetSymbolLunarDay(lunarDay);
        }
        return string.Empty;
    }

    public static string GetZodiacIn(int zodiac)
    {
        return "Луна " + GetElement(Strings.ZodiacIn, zodiac);
    }

    public static string GetZodiac(int zodiac)
    {
        return GetElement(Strings.Zodiac, zodiac);
    }

    public static string GetPhase(int phase)
    {
        return GetElement(Strings.Phase, phase);
    }

    public static string GetNewYear()
    {
        return Strings.NewYear;
    }

    public static string GetSlogan()
    {
        return Strings.Slogan;
    }
}
