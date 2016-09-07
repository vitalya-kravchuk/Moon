using System;

namespace MoonCalc
{
    /// <summary>
    /// Прецессии и нутации
    /// </summary>
    class PrecNut
    {
        /// <summary>
        /// Переход от среднего истинного экватора и равноденствия
        /// </summary>
        /// <param name="T">Юлианский календарь J2000</param>
        /// <returns>Матрица нутаций</returns>
        public static Mat3D NutMatrix(double T)
        {
            double ls, D, F, N;
            double eps, dpsi, deps;

            // Средние аргументы лунных и солнечных движений
            ls = Const.pi2 * MathEx.Frac(0.993133 + 99.997306 * T);     // средняя аномалия Солнца         
            D = Const.pi2 * MathEx.Frac(0.827362 + 1236.853087 * T);    // дифф. долгота Луна-Солнце  
            F = Const.pi2 * MathEx.Frac(0.259089 + 1342.227826 * T);    // средний аргумент широты
            N = Const.pi2 * MathEx.Frac(0.347346 - 5.372447 * T);       // долгота восходящего узла

            // Углы нутаций
            dpsi = (-17.200 * Math.Sin(N) - 1.319 * Math.Sin(2 * (F - D + N)) - 0.227 * Math.Sin(2 * (F + N))
                     + 0.206 * Math.Sin(2 * N) + 0.143 * Math.Sin(ls)) / Const.Arcs;
            deps = (+9.203 * Math.Cos(N) + 0.574 * Math.Cos(2 * (F - D + N)) + 0.098 * Math.Cos(2 * (F + N))
                     - 0.090 * Math.Cos(2 * N)) / Const.Arcs;

            // Средний наклон эклиптики
            eps = 0.4090928 - 2.2696E-4 * T;

            return Mat3D.R_x(-eps - deps) * Mat3D.R_z(-dpsi) * Mat3D.R_x(+eps);
        }
    }
}
