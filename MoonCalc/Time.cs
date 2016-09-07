using System;

namespace MoonCalc
{
    /// <summary>
    /// Формат даты и времени
    /// </summary>
    enum TimeFormat
    {
        None,   // только дата
        DDd,    // дробная часть дня
        HHh,    // время в часах с точностью до десятых
        HHMM,   // время в часах и минутах (округляется до следующей минуты)
        HHMMSS  // часы, минуты и секунды (с округлением до следующей секунды)
    }

    class DateTime
    {
        double m_Mjd;
        TimeFormat m_Format;

        public DateTime()
        {
            m_Mjd = 0.0;
            m_Format = TimeFormat.None;
        }

        public DateTime(double Mjd)
        {
            m_Mjd = Mjd;
            m_Format = TimeFormat.None;
        }

        public DateTime(double Mjd, TimeFormat Format)
        {
            m_Mjd = Mjd;
            m_Format = Format;
        }

        public void Set(TimeFormat Format)
        {
            m_Format = Format;
        }

        public string Get()
        {
            double MjdRound = 0, Hours = 0, S = 0;
            int Year = 0, Month = 0, Day = 0;
            int H = 0, M = 0;
            string os = "";

            switch (m_Format)
            {
                case TimeFormat.None:
                    TimeCalc.CalDat(m_Mjd, ref Year, ref Month, ref Day, ref Hours);
                    os = Year + "." + Month + "." + Day;
                    break;

                case TimeFormat.DDd:
                    TimeCalc.CalDat(m_Mjd, ref Year, ref Month, ref Day, ref Hours);
                    os = Year + "." + Month + "." + (Day + Hours / 24.0).ToString();
                    break;

                case TimeFormat.HHh:
                    // 0.1 ч.
                    MjdRound = Math.Floor(240.0 * m_Mjd + 0.5) / 240.0 + 0.0001;
                    TimeCalc.CalDat(MjdRound, ref Year, ref Month, ref Day, ref Hours);
                    os = Year + "." + Month + "." + Day + " " + Hours;
                    break;

                case TimeFormat.HHMM:
                    // 0.1 мин.
                    MjdRound = Math.Floor(1440.0 * m_Mjd + 0.5) / 1440.0 + 0.00001;
                    TimeCalc.CalDat(MjdRound, ref Year, ref Month, ref Day, ref H, ref M, ref S);
                    os = Year + "." + Month + "." + Day + " " + H + ":" + M;
                    break;

                case TimeFormat.HHMMSS:
                    // 1 сек.
                    MjdRound = Math.Floor(86400.0 * m_Mjd + 0.5) / 86400.0 + 0.000001;
                    TimeCalc.CalDat(MjdRound, ref Year, ref Month, ref Day, ref H, ref M, ref S);
                    os = Year + "." + Month + "." + Day + " " + H + ":" + M + ":" + Helper.DoubleToInt32(S);
                    break;
            }

            return os;
        }
    }

    class Time
    {
        double m_Hour;
        TimeFormat m_Format;

        public Time()
        {
            m_Hour = 0.0;
            m_Format = TimeFormat.HHMMSS;
        }

        public Time(double Hour)
        {
            m_Hour = Hour;
            m_Format = TimeFormat.HHMMSS;
        }

        public Time(double Hour, TimeFormat Format)
        {
            m_Hour = Hour;
            m_Format = Format;
        }

        public void Set(TimeFormat Format)
        {
            m_Format = Format;
        }

        public string Get()
        {
            string os = "";

            double Hour, S = 0;
            int H = 0, M = 0;

            switch (m_Format)
            {
                case TimeFormat.HHh:
                    os = m_Hour.ToString();
                    break;

                case TimeFormat.HHMM:
                    // 1 мин.
                    Hour = Math.Floor(60.0 * m_Hour + 0.5) / 60.0 + 0.00001;
                    MathEx.DMS(Hour, ref H, ref M, ref S);
                    os = H.ToString() + ":" + M.ToString();
                    break;

                case TimeFormat.HHMMSS:
                    // 1 сек.
                    Hour = Math.Floor(3600.0 * m_Hour + 0.5) / 3600.0 + 0.0000001;
                    MathEx.DMS(Hour, ref H, ref M, ref S);
                    os = H.ToString() + ":" + M.ToString() + ":" + Helper.DoubleToInt32(S);
                    break;
            }

            return os;
        }
    }

    /// <summary>
    /// Операции со временем и календарем
    /// </summary>
    class TimeCalc
    {
        /// <summary>
        /// Юлианский календарь с датой и временем 
        /// </summary>
        public static double Mjd(int Year, int Month, int Day, int Hour, int Min, double Sec)
        {
            Int64 MjdMidnight;
            double FracOfDay;
            int b;

            if (Month <= 2) { Month += 12; --Year; }

            if ((10000L * Year + 100L * Month + Day) <= 15821004L)
                b = -2 + ((Year + 4716) / 4) - 1179;            // Юлианский календарь
            else
                b = (Year / 400) - (Year / 100) + (Year / 4);   // Григорианский календарь

            MjdMidnight = 365L * Year - 679004L + b + Helper.DoubleToInt32(30.6001 * (Month + 1)) + Day;
            FracOfDay = MathEx.Ddd(Hour, Min, Sec) / 24.0;

            return MjdMidnight + FracOfDay;
        }

        /// <summary>
        /// Юлианский календарь с датой и временем 
        /// </summary>
        public static double Mjd(System.DateTime dateTime)
        {
            return Mjd(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        /// <summary>
        /// Разница ET-UT от эфемеридного времени и универсального времени
        /// </summary>
        /// <param name="T">Юлианский календарь J2000</param>
        /// <param name="DTsec">ET-UT в секундах</param>
        /// <param name="valid">Флаг области приближения</param>
        public static void ETminUT(double T, ref double DTsec, ref bool valid)
        {
            int i = (int)Math.Floor(T / 0.25);
            double t = T - i * 0.25;

            if ((T < -1.75) || (0.05 < T))
            {
                valid = false;
                DTsec = 0.0;
            }
            else
            {
                valid = true;
                switch (i)
                {
                    case -7: DTsec = 10.4 + t * (-80.8 + t * (413.9 + t * (-572.3))); break; // 1825-
                    case -6: DTsec = 6.6 + t * (46.3 + t * (-358.4 + t * (18.8))); break; // 1850-
                    case -5: DTsec = -3.9 + t * (-10.8 + t * (-166.2 + t * (867.4))); break; // 1875-
                    case -4: DTsec = -2.6 + t * (114.1 + t * (327.5 + t * (-1467.4))); break; // 1900-
                    case -3: DTsec = 24.2 + t * (-6.3 + t * (-8.2 + t * (483.4))); break; // 1925-
                    case -2: DTsec = 29.3 + t * (32.5 + t * (-3.8 + t * (550.7))); break; // 1950-
                    case -1: DTsec = 45.3 + t * (130.5 + t * (-570.5 + t * (1516.7))); break; // 1975-
                    case 0:
                        t += 0.25;
                        DTsec = 45.3 + t * (130.5 + t * (-570.5 + t * (1516.7)));        // 2000-2005
                        break;
                }
            }
            return;
        }

        /// <summary>
        /// Дата и время по календарю из Юлианской Даты
        /// </summary>
        /// <param name="Mjd">Модифицированая юлианская дата</param>
        public static void CalDat(double Mjd, ref int Year, ref int Month, ref int Day, ref double Hour)
        {
            Int64 a, b, c, d, e, f;
            double FracOfDay;

            a = Helper.DoubleToInt64(Mjd + 2400001.0);

            // Юлианский календарь
            if (a < 2299161)
            {
                b = 0;
                c = a + 1524;
            }
            // Григорианский календарь
            else
            {
                b = Helper.DoubleToInt64((a - 1867216.25) / 36524.25);
                c = a + b - (b / 4) + 1525;
            }

            d = Helper.DoubleToInt64((c - 122.1) / 365.25);
            e = (365 * d) + (d / 4);
            f = Helper.DoubleToInt64((c - e) / 30.6001);

            Day = Helper.DoubleToInt32(c - e - Helper.DoubleToInt32(30.6001 * f));
            Month = Helper.DoubleToInt32(f - 1 - 12 * (f / 14));
            Year = Helper.DoubleToInt32(d - 4715 - ((7 + Month) / 10));
            FracOfDay = Mjd - Math.Floor(Mjd);
            Hour = FracOfDay * 24.0;
        }

        /// <summary>
        /// Дата и время по календарю из Юлианской Даты
        /// </summary>
        /// <param name="Mjd">Модифицированая юлианская дата</param>
        public static void CalDat(double Mjd, ref int Year, ref int Month, ref int Day, ref int Hour, ref int Min, ref double Sec)
        {
            double Hours = 0;
            CalDat(Mjd, ref Year, ref Month, ref Day, ref Hours);
            MathEx.DMS(Hours, ref Hour, ref Min, ref Sec);
        }

        /// <summary>
        /// Звездное время по Гринвичу
        /// </summary>
        /// <param name="MJD">Дата и время по юлианскому календарю</param>
        /// <returns>GMST в рад</returns>
        public static double GMST(double MJD)
        {
            double Secs = 86400.0; // Секунд в день

            double MJD_0, UT, T_0, T, gmst;

            MJD_0 = Math.Floor(MJD);
            UT = Secs * (MJD - MJD_0); // [сек]
            T_0 = (MJD_0 - 51544.5) / 36525.0;
            T = (MJD - 51544.5) / 36525.0;

            gmst =
                24110.54841 + 8640184.812866 * T_0 + 1.0027379093 * UT
                + (0.093104 - 6.2e-6 * T) * T * T; // [сек]

            return (Const.pi2 / Secs) * MathEx.Modulo(gmst, Secs); // [рад]
        }
    }

    /// <summary>
    /// Переход на летнее время и обратно
    /// </summary>
    /// <remarks>
    /// В России и в Европе переход на летнее время осуществляется 
    /// в последнее воскресенье марта в 2:00 переводом часовых стрелок на 1 час вперед, 
    /// а обратный переход осуществляется в последнее воскресенье октября в 3:00 переводом стрелок на 1 час назад
    /// </remarks>
    public class DaylightSavingTime
    {
        #region Properties
        public System.DateTime spring { get; set; }
        public System.DateTime autumn { get; set; }
        #endregion

        /// <summary>
        /// Дата и время перехода на летнее время
        /// </summary>
        /// <param name="dateTime">Относительно какой даты определить</param>
        static System.DateTime Summer(System.DateTime dateTime)
        {
            int Year = dateTime.Year;
            System.DateTime dtSpring = new System.DateTime(Year, 3, System.DateTime.DaysInMonth(Year, 3));
            int daysExpired = 0 - (int)dtSpring.DayOfWeek;
            dtSpring = dtSpring.AddDays(daysExpired);
            dtSpring = dtSpring.AddHours(2);
            return dtSpring;
        }

        /// <summary>
        /// Дата и время перехода на зимнее время
        /// </summary>
        /// <param name="dateTime">Относительно какой даты определить</param>
        static System.DateTime Winter(System.DateTime dateTime)
        {
            int Year = dateTime.Year;
            System.DateTime dtAutumn = new System.DateTime(Year, 10, System.DateTime.DaysInMonth(Year, 10));
            int daysExpired = 0 - (int)dtAutumn.DayOfWeek;
            dtAutumn = dtAutumn.AddDays(daysExpired);
            dtAutumn = dtAutumn.AddHours(3);
            return dtAutumn;
        }

        /// <summary>
        /// Вычислить дату перехода на летнее и зимнее время
        /// </summary>
        public static DaylightSavingTime GetDaylightSavingTime(System.DateTime dateTime)
        {
            return new DaylightSavingTime()
            {
                spring = Summer(dateTime),
                autumn = Winter(dateTime)
            };
        }

        /// <summary>
        /// +1 (час), если летнее время
        /// </summary>
        public static double Try(DaylightSavingTime daylightSavingTime, System.DateTime dateTime)
        {
            if (daylightSavingTime == null) return 0;
            if (dateTime == System.DateTime.MinValue) return 0;

            double t = 0;

            int m1 = daylightSavingTime.spring.Month;
            int d1 = daylightSavingTime.spring.Day;
            int h1 = daylightSavingTime.spring.Hour;

            int m2 = daylightSavingTime.autumn.Month;
            int d2 = daylightSavingTime.autumn.Day;
            int h2 = daylightSavingTime.autumn.Hour;

            int m = dateTime.Month;
            int d = dateTime.Day;
            int h = dateTime.Hour;

            if (m > m1 && m < m2)
            {
                t = 1;
            }
            else if (m == m1 && d > d1)
            {
                t = 1;
            }
            else if (m == m1 && d == d1 && h >= h1)
            {
                t = 1;
            }
            else if (m == m2 && d < d2)
            {
                t = 1;
            }
            else if (m == m2 && d == d2 && h < h2)
            {
                t = 1;
            }
            else
            {
                return 0;
            }

            return t;
        }
    }
}
