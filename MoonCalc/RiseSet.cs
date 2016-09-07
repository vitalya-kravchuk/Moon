using System;
using System.Collections.Generic;

namespace MoonCalc
{
    /// <summary>
    /// Восход и заход Луны
    /// </summary>
    class RiseSet
    {
        #region Properties
        public System.DateTime riseDateTime { get; set; }
        public System.DateTime setDateTime { get; set; }
        #endregion

        static readonly double sinh0 = Math.Sin(Const.Rad * (8.0 / 60.0));

        /// <summary>
        /// Синус высоты Луны
        /// </summary>
        /// <param name="MJD0">0h юлианская дата</param>
        /// <param name="Hour">Час</param>
        /// <param name="lambda">Географическая восточная долгота [рад]</param>
        /// <param name="Cphi">Косинус географической широты</param>
        /// <param name="Sphi">Синус географической широты</param>
        /// <returns>Синус высоты Луны на момент события</returns>
        static double SinAlt(double MJD0, double Hour, double lambda, double Cphi, double Sphi)
        {
            double MJD, T, RA = 0, Dec = 0, tau = 0;

            MJD = MJD0 + Hour / 24.0;
            T = (MJD - 51544.5) / 36525.0;

            MoonCoord.Mini(T, ref RA, ref Dec);

            tau = TimeCalc.GMST(MJD) + lambda - RA;

            return (Sphi * System.Math.Sin(Dec) + Cphi * System.Math.Cos(Dec) * System.Math.Cos(tau));
        }

        /// <summary>
        /// Поиск событий восхода и захода Луны 
        /// </summary>
        /// <param name="MJD0h">0h желаемая дата</param>
        /// <param name="lambda">Географическая восточная долгота наблюдателя в [рад]</param>
        /// <param name="phi">Географическая широта наблюдателя в [рад]</param>
        /// <param name="LT_Rise">Местное время восхода</param>
        /// <param name="LT_Set">Местное время заката</param>
        /// <param name="rises">Происходит восход</param>
        /// <param name="sets">Происходит закат</param>
        /// <param name="above">Циркумполярная Луна</param>
        static void FindEvents(double MJD0h, double lambda, double phi,
            ref double LT_Rise, ref double LT_Set, ref bool rises, ref bool sets, ref bool above)
        {
            double Cphi = Math.Cos(phi);
            double Sphi = Math.Sin(phi);

            double hour = 1.0;
            double y_minus, y_0, y_plus;
            double xe = 0, ye = 0, root1 = 0, root2 = 0;
            int nRoot = 0;

            // 	Инициализация для поиска
            y_minus = SinAlt(MJD0h, hour - 1.0, lambda, Cphi, Sphi) - sinh0;

            above = (y_minus > 0.0);
            rises = false;
            sets = false;

            // Интервал от [0ч-2ч] до [22ч-24ч]
            do
            {
                y_0 = SinAlt(MJD0h, hour, lambda, Cphi, Sphi) - sinh0;
                y_plus = SinAlt(MJD0h, hour + 1.0, lambda, Cphi, Sphi) - sinh0;

                // поиск параболы в трех значениях y_minus, y_0, y_plus
                MathEx.Quad(y_minus, y_0, y_plus, ref xe, ref ye, ref root1, ref root2, ref nRoot);

                if (nRoot == 1)
                {
                    if (y_minus < 0.0)
                    {
                        LT_Rise = hour + root1;
                        rises = true;
                    }
                    else
                    {
                        LT_Set = hour + root1;
                        sets = true;
                    }
                }

                if (nRoot == 2)
                {
                    if (ye < 0.0)
                    {
                        LT_Rise = hour + root2;
                        LT_Set = hour + root1;
                    }
                    else
                    {
                        LT_Rise = hour + root1;
                        LT_Set = hour + root2;
                    }
                    rises = true;
                    sets = true;
                }

                // подготовка к следующему интервалу
                y_minus = y_plus;
                hour += 2.0;
            }
            while (!((hour == 25.0) || (rises && sets)));
        }

        /// <summary>
        /// Список восходов и заходов Луны на определенное количество дней
        /// </summary>
        /// <param name="Year">Год</param>
        /// <param name="Month">Месяц</param>
        /// <param name="Day">День</param>
        /// <param name="Latitude">Широта</param>
        /// <param name="Longitude">Долгота</param>
        /// <param name="timeZone">Часовой пояс</param>
        /// <param name="daylightSavingTime">Переход на летнее время</param>
        /// <param name="DaysCount">Количество дней</param>
        public static List<RiseSet> Get(int Year, int Month, int Day, double Latitude, double Longitude, double timeZone, DaylightSavingTime daylightSavingTime, int DaysCount)
        {
            double lambda = Const.Rad * Longitude;
            double phi = Const.Rad * Latitude;
            double zone = timeZone / 24.0;
            double start_date = TimeCalc.Mjd(Year, Month, Day, 0, 0, 0) - zone;

            bool above = false, rise = false, sett = false;
            double date, LT_Rise = 0, LT_Set = 0;

            List<RiseSet> rsList = new List<RiseSet>();
            for (int day = 0; day < DaysCount; day++)
            {
                date = start_date + day;
                DateTime dateTime = new DateTime(date + zone);
                System.DateTime currDateTime = System.DateTime.Parse(dateTime.Get());

                FindEvents(date, lambda, phi, ref LT_Rise, ref LT_Set, ref rise, ref sett, ref above);
                RiseSet rs = new RiseSet();
                if (rise)
                {
                    Time time = new Time(LT_Rise, TimeFormat.HHMMSS);
                    TimeSpan ts = TimeSpan.Parse(time.Get());
                    rs.riseDateTime = currDateTime + ts;
                }
                if (sett)
                {
                    Time time = new Time(LT_Set, TimeFormat.HHMMSS);
                    TimeSpan ts = TimeSpan.Parse(time.Get());
                    rs.setDateTime = currDateTime + ts;
                }

                if (rs.setDateTime != System.DateTime.MinValue)
                    if (rs.setDateTime < rs.riseDateTime)
                        rs.setDateTime = rs.setDateTime.AddDays(1);

                rs.riseDateTime = rs.riseDateTime.AddHours(DaylightSavingTime.Try(daylightSavingTime, rs.riseDateTime));
                rs.setDateTime = rs.setDateTime.AddHours(DaylightSavingTime.Try(daylightSavingTime, rs.setDateTime));

                rsList.Add(rs);
            }
            return rsList;
        }
    }
}
