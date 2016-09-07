using System;
using System.Collections.Generic;

namespace MoonCalc
{
    /// <summary>
    /// Знак Зодиака
    /// </summary>
    public enum ZodiacSign
    {
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius,
        Capricorn,
        Aquarius,
        Pisces
    }

    /// <summary>
    /// Луна в знаке Зодиака
    /// </summary>
    class Zodiac
    {
        #region Properties
        public System.DateTime dateTime { get; set; }
        public double RA { get; set; }
        public ZodiacSign sign { get; set; }
        #endregion

        /// <summary>
        /// Возвращает Зодиак, в котором находится Луна
        /// </summary>
        /// <param name="RA">Прямое восхождение Луны в градусах</param>
        /// <returns>Знак Зодиака</returns>
        static ZodiacSign Get(double RA)
        {
            int i = 1;

            if (RA < 26.02) i = 1;
            else if (RA < 56.76) i = 2;
            else if (RA < 89.99) i = 3;
            else if (RA < 122.18) i = 4;
            else if (RA < 151.17) i = 5;
            else if (RA < 178.22) i = 6;
            else if (RA < 206.02) i = 7;
            else if (RA < 236.76) i = 8;
            else if (RA < 269.99) i = 9;
            else if (RA < 302.18) i = 10;
            else if (RA < 331.16) i = 11;
            else if (RA < 358.21) i = 12;
            else i = 1;

            return (ZodiacSign)i - 1;
        }

        /// <summary>
        /// Список Зодиаков на определенный интервал и шаг
        /// </summary>
        /// <param name="dtStart">Дата начала</param>
        /// <param name="dtEnd">Дата конца</param>
        /// <param name="StepDay">Шаг в днях</param>
        /// <param name="StepHour">Шаг в часах</param>
        /// <param name="ToNextZodiac">Только до следующего Зодиака</param>
        static List<Zodiac> Get(System.DateTime dtStart, System.DateTime dtEnd, double StepDay, double StepHour, bool ToNextZodiac)
        {
            const int Degree = 10;
            const double Interval = 10.0 / 36525.0;  // 10d in [cy]

            double MjdStart, Step, MjdEnd;
            double Date, T;
            Vec3D r_Moon;
            Cheb3D ChebMoonEqu = new Cheb3D(MoonCoord.Equ, Degree, Interval);

            MjdStart = TimeCalc.Mjd(dtStart);
            MjdEnd = TimeCalc.Mjd(dtEnd);
            Step = StepDay + StepHour / 24.0;
            Date = MjdStart;

            int z = 0;

            List<Zodiac> zList = new List<Zodiac>();
            while (Date < MjdEnd + Step / 2)
            {
                T = (Date - Const.MJD_J2000) / 36525.0;
                r_Moon = ChebMoonEqu.Value(T);

                if (ToNextZodiac)
                {
                    if (z == 0 && zList.Count > 1) z = (int)zList[0].sign;
                    if (zList.Count > 2)
                        if (zList[zList.Count - 1].sign != (ZodiacSign)z)
                            break;
                }

                double RA = Const.Deg * r_Moon[Vec3DPolIndex.phi];
                zList.Add(new Zodiac()
                {
                    dateTime = System.DateTime.Parse((new DateTime(Date, TimeFormat.HHMMSS)).Get()),
                    RA = RA,
                    sign = Get(RA)
                });

                Date += Step;
            }
            return zList;
        }

        /// <summary>
        /// Список Зодиаков на месяц
        /// </summary>
        /// <param name="dateTime">Дата</param>
        /// <param name="timeZone">Часовой пояс</param>
        /// <param name="daylightSavingTime">Переход на летнее время</param>
        /// <param name="helperCalc">Вспомогательные вычисления</param>
        public static List<Zodiac> Get(System.DateTime dateTime, double timeZone, DaylightSavingTime daylightSavingTime, bool helperCalc)
        {
            System.DateTime dtStart = new System.DateTime(dateTime.Year, dateTime.Month, 1);
            dtStart = dtStart.AddMonths(-1);
            dtStart = dtStart.AddDays(System.DateTime.DaysInMonth(dtStart.Year, dtStart.Month) - 3);

            System.DateTime dtEnd = new System.DateTime(dateTime.Year, dateTime.Month, 1);
            dtEnd = dtEnd.AddMonths(1);
            dtEnd = dtEnd.AddDays(3);
            if (helperCalc) dtEnd = dtEnd.AddDays(30);

            List<Zodiac> zList = new List<Zodiac>();
            List<Zodiac> zDaysList = Get(dtStart, dtEnd, 1, 0, false);
            for (int i = 0; i < zDaysList.Count - 1; i++)
            {
                if (zDaysList[i].sign != zDaysList[i + 1].sign)
                {
                    dtStart = zDaysList[i].dateTime;
                    dtEnd = zDaysList[i + 1].dateTime;
                    List<Zodiac> zHoursList = Get(dtStart, dtEnd, 0, 1, true);

                    dtStart = zHoursList[zHoursList.Count - 2].dateTime;
                    dtEnd = zHoursList[zHoursList.Count - 1].dateTime;
                    List<Zodiac> zMinutesList = Get(dtStart, dtEnd, 0, 0.01, true);

                    Zodiac zm = (Zodiac)zMinutesList[zMinutesList.Count - 1];

                    // Часовой пояс
                    zm.dateTime = zm.dateTime.AddHours(timeZone);
                    zm.dateTime = zm.dateTime.AddHours(DaylightSavingTime.Try(daylightSavingTime, zm.dateTime));

                    zList.Add(zm);
                }
            }

            if (helperCalc == false)
            {
                int j = 0;
                while (j < zList.Count)
                {
                    if (zList[j].dateTime.Month == dateTime.Month) break;
                    if (zList[j].dateTime.Month == zList[j + 1].dateTime.Month)
                        zList.RemoveAt(j);
                    else
                        j++;
                }

                int k = zList.Count - 1;
                while (k > 0)
                {
                    if (zList[k].dateTime.Month == dateTime.Month) break;
                    if (zList[k].dateTime.Month == zList[k - 1].dateTime.Month)
                        zList.RemoveAt(k);
                    k--;
                }
            }

            return zList;
        }
    }
}
