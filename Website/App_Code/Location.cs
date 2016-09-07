using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using MoonCalc;

public class Location : Settings
{
    #region Properties
    /// <summary>
    /// Имя местоположения
    /// </summary>
    public string PlaceName { get; set; }

    /// <summary>
    /// Тип карты
    /// </summary>
    public int MapType { get; set; }

    /// <summary>
    /// Масштаб карты
    /// </summary>
    public int MapZoom { get; set; }
    #endregion

    public Location()
    {
        MapType = 2;
        MapZoom = 3;
    }

    static string GetCountryCodeByIP(string clientIP)
    {
        try
        {
            Logger.Log.Info(clientIP);
            TcpClient TCP = new TcpClient("whois.ripe.net", 43);
            Stream stream = TCP.GetStream();
            byte[] arrIP = Encoding.ASCII.GetBytes(clientIP + "\r\n");
            stream.Write(arrIP, 0, arrIP.Length);
            using (StreamReader sr = new StreamReader(TCP.GetStream(), Encoding.ASCII))
            {
                string s = sr.ReadToEnd();
                int startIndex = s.IndexOf("country", 0);
                string country = "";
                if (startIndex > -1)
                {
                    int length = s.IndexOf('\n', startIndex + 1) - startIndex;
                    country = s.Substring(startIndex, length);
                }
                TCP.Close();
                if (country.Length == 0)
                    return "";

                string code = country.Remove(0, "country:".Length);
                if (code.Length > 0)
                {
                    code = code.Trim(' ', '\t');
                    if (code.Length > 2)
                        code = code.Substring(0, 2).ToUpper();
                    return code;
                }
                else
                {
                    return "";
                }
            }
        }
        catch
        {
        }
        return "";
    }

    public static Location GetByIP(string clientIP)
    {
        Countries country = Countries.GetByCode(GetCountryCodeByIP(clientIP));
        if (country == null)
            country = Countries.GetByCode("UA");
        return new Location()
        {
            PlaceName = country.Name,
            Latitude = country.Latitude,
            Longitude = country.Longitude,
            TimeZone = country.TimeZone,
            DST = country.DST
        };
    }

    public static DateTime GetLocalDateTime(Location location)
    {
        double timeZone = location.TimeZone;
        double h = Math.Floor(timeZone);
        double m = (timeZone - Math.Floor(timeZone)) * 60;
        TimeSpan tsTimeZone = new TimeSpan((int)h, (int)m, 0);

        DateTime dateTime = DateTime.Now.ToUniversalTime().Add(tsTimeZone);
        if (location.DST)
        {
            DaylightSavingTime dst = DaylightSavingTime.GetDaylightSavingTime(dateTime);
            dateTime = dateTime.AddHours(DaylightSavingTime.Try(dst, dateTime));
        }
        return dateTime;
    }
}
