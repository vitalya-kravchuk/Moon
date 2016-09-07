using System;
using System.Collections.Generic;

namespace MoonCalc
{
    /// <summary>
    /// Фаза
    /// </summary>
    public enum Phase
    {
        NewMoon,        // новолуние
        FirstQuarter,   // растущая Луна
        FullMoon,       // полнолуние
        LastQuarter     // убывающая Луна
    }

    /// <summary>
    /// Затмение
    /// </summary>
    public enum Eclipse
    {
        No,                         // Нет затмения
        SunNo,                      // Нет солнечного затмения
        SunCentral,                 // Солнечное центральное затмение
        SunPossiblyCentral,         // Возможно солнечное центральное затмение 
        SunPartial,                 // Солнечное частичное затмение
        SunPossilbyPartial,         // Возможно частичное солнечное затмение
        MoonNo,                     // Нет лунного затмения
        MoonTotal,                  // Полное лунное затмение
        MoonPossiblyTotal,          // Возможно лунное полное затмение
        MoonPartial,                // Лунное частичное затмение
        MoonPossiblyPartial,        // Возможно частичное лунное затмение
        MoonPartialShade,           // Лунное полутеневое затмение
        MoonPossiblyPartialShade    // Возможно лунное полутеневое затмение
    }

    /// <summary>
    /// Фаза Луны и затмение
    /// </summary>
    public class PhaseEclipse
    {
        #region Properties
        public Phase phase { get; set; }
        public System.DateTime dateTime { get; set; }
        public Eclipse eclipse { get; set; }
        #endregion

        const double dT = 7.0 / 36525.0;                    // Шаг (1 неделя)
        const double Acc = (0.5 / 1440.0) / 36525.0;        // Точность (0.5 минут)
        const double tau_Sun = 8.32 / (1440.0 * 36525.0);   // 8.32 мин  [cy]

        static Phase lunarPhase = Phase.NewMoon;

        /// <summary>
        /// Возможность солнечного затмения 
        /// </summary>
        /// <param name="beta">эклиптическая широта Луны (рад)</param>
        static Eclipse SolarEclipseFlag(double beta)
        {
            double b = Math.Abs(beta);
            if (b > 0.027586) return Eclipse.SunNo;               // нет затмения
            if (b < 0.015223) return Eclipse.SunCentral;          // центральное затмение
            if (b < 0.018209) return Eclipse.SunPossiblyCentral;  // возможно центральное затмение 
            if (b < 0.024594) return Eclipse.SunPartial;          // частичное затмение
            return Eclipse.SunPossilbyPartial;                    // возможно частичное затмение
        }

        /// <summary>
        /// Возможность лунного затмения 
        /// </summary>
        /// <param name="beta">эклиптическая широта Луны (рад)</param>
        static Eclipse LunarEclipseFlag(double beta)
        {
            double b = Math.Abs(beta);
            if (b > 0.028134) return Eclipse.MoonNo;              // нет затмения
            if (b < 0.006351) return Eclipse.MoonTotal;           // полное затмение
            if (b < 0.009376) return Eclipse.MoonPossiblyTotal;   // возможно полное затмение
            if (b < 0.015533) return Eclipse.MoonPartial;         // частичное затмение
            if (b < 0.018568) return Eclipse.MoonPossiblyPartial; // возможно частичное затмение
            if (b < 0.025089) return Eclipse.MoonPartialShade;    // полутеневое затмение
            return Eclipse.MoonPossiblyPartialShade;              // возможно полутеневое затмение
        }

        /// <summary>
        /// Целевая функция поиска этапа событий
        /// </summary>
        /// <param name="T">Юлианский Календарь J2000</param>
        /// <remarks>Использует статическую lunarPhase</remarks>
        static double PhasesFunc(double T)
        {
            double Int64Diff = MoonCoord.Pos(T)[Vec3DPolIndex.phi] - SunCoord.Pos(T - tau_Sun)[Vec3DPolIndex.phi];
            return MathEx.Modulo(Int64Diff - (int)lunarPhase * Const.pi / 2.0 + Const.pi, Const.pi2) - Const.pi;
        }

        /// <summary>
        /// Список фаз и затмений Луны
        /// </summary>
        /// <param name="year">Год, с которого начинается список</param>
        /// <param name="fromMonth">Месяц, с которого начинается список</param>
        /// <param name="toMonth">Месяц, на котором список заканчивается</param>
        /// <param name="timeZone">Часовой пояс</param>
        /// <param name="daylightSavingTime">Переход на летнее время и обратно</param>
        /// <remarks>fromMonth и toMonth только для цикла (т.е. может быть -1 или 13), главное, чтобы fromMonth меньше toMonth</remarks>
        static public List<PhaseEclipse> Get(int year, int fromMonth, int toMonth, double timeZone, DaylightSavingTime daylightSavingTime)
        {
            double MjdUT, ET_UT = 0, T0, T1, TPhase, D0, D1, beta;

            T0 = (TimeCalc.Mjd(year, fromMonth, 1, 0, 0, 0.0) - Const.MJD_J2000) / 36525.0;
            T1 = T0 + dT;
            TPhase = T0;
            bool Success = false;
            bool valid = false;
            Eclipse eclipse = Eclipse.No;

            List<PhaseEclipse> peList = new List<PhaseEclipse>();
            for (int Lunation = fromMonth; Lunation <= toMonth; Lunation++)
            {
                for (int iPhase = (int)Phase.NewMoon; iPhase <= (int)Phase.LastQuarter; iPhase++)
                {
                    lunarPhase = (Phase)iPhase;

                    // Желаемый этап события
                    D0 = PhasesFunc(T0);
                    D1 = PhasesFunc(T1);

                    while ((D0 * D1 > 0.0) || (D1 < D0))
                    {
                        T0 = T1; D0 = D1; T1 += dT; D1 = PhasesFunc(T1);
                    }

                    // Итерация времени фаз
                    MathEx.Pegasus(PhasesFunc, T0, T1, Acc, ref TPhase, ref Success);

                    // Поправить разницу эфемеридного и универсального времени
                    TimeCalc.ETminUT(TPhase, ref ET_UT, ref valid);
                    MjdUT = (TPhase * 36525.0 + Const.MJD_J2000) - ET_UT / 86400.0;

                    // Дата и время
                    DateTime dt = new DateTime(MjdUT, TimeFormat.HHMMSS);
                    System.DateTime dtPhase = System.DateTime.Parse(dt.Get()).AddHours(timeZone);
                    dtPhase = dtPhase.AddHours(DaylightSavingTime.Try(daylightSavingTime, dtPhase));

                    // Затмение
                    beta = MoonCoord.Pos(TPhase)[Vec3DPolIndex.theta];
                    if ((Phase)iPhase == Phase.NewMoon)
                        eclipse = SolarEclipseFlag(beta);
                    else if ((Phase)iPhase == Phase.FullMoon)
                        eclipse = LunarEclipseFlag(beta);
                    else
                        eclipse = Eclipse.No;

                    // Добавить с список
                    peList.Add(new PhaseEclipse()
                    {
                        dateTime = dtPhase,
                        phase = lunarPhase,
                        eclipse = eclipse
                    });

                    // Интервал
                    T0 = TPhase;
                    T1 = T0 + dT;
                }
            }
            return peList;
        }
    }
}
