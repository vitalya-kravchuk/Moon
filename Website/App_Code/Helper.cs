using System;
using System.Globalization;

public class Helper
{
    #region Convert
    public static string ToString(object value)
    {
        if (value == null)
            return string.Empty;
        if (value is double)
            return DoubleToString((double)value);
        else
            return value.ToString();
    }

    public static string DoubleToString(double d)
    {
        return Convert.ToString(d).Replace(',', '.');
    }

    public static double StringToDouble(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";
        return Convert.ToDouble(s.Replace(',', '.'), provider);
    }

    public static string DateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy HH:mm:ss");
    }
    #endregion

    #region Format
    public static string GetMonthName(int month)
    {
        DateTimeFormatInfo dti = CultureInfo.CurrentUICulture.DateTimeFormat;
        return dti.GetMonthName(month);
    }

    public static string GetAbbreviatedDayName(DayOfWeek dayOfWeek)
    {
        DateTimeFormatInfo dti = CultureInfo.CurrentUICulture.DateTimeFormat;
        return dti.GetAbbreviatedDayName(dayOfWeek);
    }

    public static string GetDayName(DayOfWeek dayOfWeek)
    {
        DateTimeFormatInfo dti = CultureInfo.CurrentUICulture.DateTimeFormat;
        return dti.GetDayName(dayOfWeek);
    }
    #endregion
}
