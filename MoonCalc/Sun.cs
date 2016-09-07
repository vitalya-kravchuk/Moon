using System;

namespace MoonCalc
{
    /// <summary>
    /// Возмущения Солнца
    /// </summary>
    class SunPert
    {
        const int o = 10;           // Индекс смещения
        const int dim = 2 * o + 1;  // Размер массива

        double m_T;
        double[] m_C = new double[dim];
        double[] m_S = new double[dim];
        double[] m_c = new double[dim];
        double[] m_s = new double[dim];
        double m_dl, m_db, m_dr;
        double m_u, m_v;

        /// <summary>
        /// Установить время, среднюю аномалию и диапазон индекса
        /// </summary>
        public void Init(double T, double M, int I_min, int I_max, double m, int i_min, int i_max)
        {
            int i;

            // обнулить возмущение
            m_dl = 0.0; m_dr = 0.0; m_db = 0.0;
            // время
            m_T = T;

            // M cosine и sine
            m_C[o] = 1.0; m_C[o + 1] = Math.Cos(M); m_C[o - 1] = +m_C[o + 1];
            m_S[o] = 0.0; m_S[o + 1] = Math.Sin(M); m_S[o - 1] = -m_S[o + 1];

            for (i = 1; i < I_max; i++)
                MathEx.AddThe(m_C[o + i], m_S[o + i], m_C[o + 1], m_S[o + 1], ref m_C[o + i + 1], ref m_S[o + i + 1]);
            for (i = -1; i > I_min; i--)
                MathEx.AddThe(m_C[o + i], m_S[o + i], m_C[o - 1], m_S[o - 1], ref m_C[o + i - 1], ref m_S[o + i - 1]);

            // m cosine и sine
            m_c[o] = 1.0; m_c[o + 1] = Math.Cos(m); m_c[o - 1] = +m_c[o + 1];
            m_s[o] = 0.0; m_s[o + 1] = Math.Sin(m); m_s[o - 1] = -m_s[o + 1];

            for (i = 1; i < i_max; i++)
                MathEx.AddThe(m_c[o + i], m_s[o + i], m_c[o + 1], m_s[o + 1], ref m_c[o + i + 1], ref m_s[o + i + 1]);
            for (i = -1; i > i_min; i--)
                MathEx.AddThe(m_c[o + i], m_s[o + i], m_c[o - 1], m_s[o - 1], ref m_c[o + i - 1], ref m_s[o + i - 1]);
        }

        /// <summary>
        /// Cуммирование возмущений долготы, широты и радиуса
        /// </summary>
        public void Term(int I, int i, int iT, double dlc, double dls, double drc, double drs, double dbc, double dbs)
        {
            if (iT == 0)
            {
                MathEx.AddThe(m_C[o + I], m_S[o + I], m_c[o + i], m_s[o + i], ref m_u, ref m_v);
            }
            else
            {
                m_u *= m_T; m_v *= m_T;
            }

            m_dl += (dlc * m_u + dls * m_v);
            m_dr += (drc * m_u + drs * m_v);
            m_db += (dbc * m_u + dbs * m_v);
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double dl()
        {
            return m_dl;
        }

        /// <summary>
        /// Широта
        /// </summary>
        public double dr()
        {
            return m_dr;
        }

        /// <summary>
        /// Радиус
        /// </summary>
        public double db()
        {
            return m_db;
        }
    }

    /// <summary>
    /// Координаты Солнца
    /// </summary>
    class SunCoord
    {
        /// <summary>
        /// Вычислить эклиптическую позицию Солнца
        /// </summary>
        /// <param name="T">Юлианский календарь J2000</param>
        /// <returns>Геоцентрическое положение Солнца (в [AU]) используя эклиптику и дату равноденствия</returns>
        public static Vec3D Pos(double T)
        {
            double M2, M3, M4, M5, M6;  // Среднии аномалии
            double D, A, U;             // Средний аргумент лунной орбиты
            double dl, dr, db;          // Исправление долготы ["], радиуса [AU] и широты ["]
            double l, b, r;             // Эклиптические координаты

            // Возмущения
            SunPert Ven = new SunPert();
            SunPert Mar = new SunPert();
            SunPert Jup = new SunPert();
            SunPert Sat = new SunPert();

            // Средние аномалии планет и средние аргументы лунной орбиты [рад]
            M2 = Const.pi2 * MathEx.Frac(0.1387306 + 162.5485917 * T);
            M3 = Const.pi2 * MathEx.Frac(0.9931266 + 99.9973604 * T);
            M4 = Const.pi2 * MathEx.Frac(0.0543250 + 53.1666028 * T);
            M5 = Const.pi2 * MathEx.Frac(0.0551750 + 8.4293972 * T);
            M6 = Const.pi2 * MathEx.Frac(0.8816500 + 3.3938722 * T);

            D = Const.pi2 * MathEx.Frac(0.8274 + 1236.8531 * T);
            A = Const.pi2 * MathEx.Frac(0.3749 + 1325.5524 * T);
            U = Const.pi2 * MathEx.Frac(0.2591 + 1342.2278 * T);

            // Кеплеровы условия и возмущения Венеры
            Ven.Init(T, M3, 0, 7, M2, -6, 0);

            Ven.Term(1, 0, 0, -0.22, 6892.76, -16707.37, -0.54, 0.00, 0.00);
            Ven.Term(1, 0, 1, -0.06, -17.35, 42.04, -0.15, 0.00, 0.00);
            Ven.Term(1, 0, 2, -0.01, -0.05, 0.13, -0.02, 0.00, 0.00);
            Ven.Term(2, 0, 0, 0.00, 71.98, -139.57, 0.00, 0.00, 0.00);
            Ven.Term(2, 0, 1, 0.00, -0.36, 0.70, 0.00, 0.00, 0.00);
            Ven.Term(3, 0, 0, 0.00, 1.04, -1.75, 0.00, 0.00, 0.00);
            Ven.Term(0, -1, 0, 0.03, -0.07, -0.16, -0.07, 0.02, -0.02);
            Ven.Term(1, -1, 0, 2.35, -4.23, -4.75, -2.64, 0.00, 0.00);
            Ven.Term(1, -2, 0, -0.10, 0.06, 0.12, 0.20, 0.02, 0.00);
            Ven.Term(2, -1, 0, -0.06, -0.03, 0.20, -0.01, 0.01, -0.09);
            Ven.Term(2, -2, 0, -4.70, 2.90, 8.28, 13.42, 0.01, -0.01);
            Ven.Term(3, -2, 0, 1.80, -1.74, -1.44, -1.57, 0.04, -0.06);
            Ven.Term(3, -3, 0, -0.67, 0.03, 0.11, 2.43, 0.01, 0.00);
            Ven.Term(4, -2, 0, 0.03, -0.03, 0.10, 0.09, 0.01, -0.01);
            Ven.Term(4, -3, 0, 1.51, -0.40, -0.88, -3.36, 0.18, -0.10);
            Ven.Term(4, -4, 0, -0.19, -0.09, -0.38, 0.77, 0.00, 0.00);
            Ven.Term(5, -3, 0, 0.76, -0.68, 0.30, 0.37, 0.01, 0.00);
            Ven.Term(5, -4, 0, -0.14, -0.04, -0.11, 0.43, -0.03, 0.00);
            Ven.Term(5, -5, 0, -0.05, -0.07, -0.31, 0.21, 0.00, 0.00);
            Ven.Term(6, -4, 0, 0.15, -0.04, -0.06, -0.21, 0.01, 0.00);
            Ven.Term(6, -5, 0, -0.03, -0.03, -0.09, 0.09, -0.01, 0.00);
            Ven.Term(6, -6, 0, 0.00, -0.04, -0.18, 0.02, 0.00, 0.00);
            Ven.Term(7, -5, 0, -0.12, -0.03, -0.08, 0.31, -0.02, -0.01);

            dl = Ven.dl(); dr = Ven.dr(); db = Ven.db();

            // Возмущения Марса
            Mar.Init(T, M3, 1, 5, M4, -8, -1);

            Mar.Term(1, -1, 0, -0.22, 0.17, -0.21, -0.27, 0.00, 0.00);
            Mar.Term(1, -2, 0, -1.66, 0.62, 0.16, 0.28, 0.00, 0.00);
            Mar.Term(2, -2, 0, 1.96, 0.57, -1.32, 4.55, 0.00, 0.01);
            Mar.Term(2, -3, 0, 0.40, 0.15, -0.17, 0.46, 0.00, 0.00);
            Mar.Term(2, -4, 0, 0.53, 0.26, 0.09, -0.22, 0.00, 0.00);
            Mar.Term(3, -3, 0, 0.05, 0.12, -0.35, 0.15, 0.00, 0.00);
            Mar.Term(3, -4, 0, -0.13, -0.48, 1.06, -0.29, 0.01, 0.00);
            Mar.Term(3, -5, 0, -0.04, -0.20, 0.20, -0.04, 0.00, 0.00);
            Mar.Term(4, -4, 0, 0.00, -0.03, 0.10, 0.04, 0.00, 0.00);
            Mar.Term(4, -5, 0, 0.05, -0.07, 0.20, 0.14, 0.00, 0.00);
            Mar.Term(4, -6, 0, -0.10, 0.11, -0.23, -0.22, 0.00, 0.00);
            Mar.Term(5, -7, 0, -0.05, 0.00, 0.01, -0.14, 0.00, 0.00);
            Mar.Term(5, -8, 0, 0.05, 0.01, -0.02, 0.10, 0.00, 0.00);

            dl += Mar.dl(); dr += Mar.dr(); db += Mar.db();

            // Возмущения Юпитера
            Jup.Init(T, M3, -1, 3, M5, -4, -1);

            Jup.Term(-1, -1, 0, 0.01, 0.07, 0.18, -0.02, 0.00, -0.02);
            Jup.Term(0, -1, 0, -0.31, 2.58, 0.52, 0.34, 0.02, 0.00);
            Jup.Term(1, -1, 0, -7.21, -0.06, 0.13, -16.27, 0.00, -0.02);
            Jup.Term(1, -2, 0, -0.54, -1.52, 3.09, -1.12, 0.01, -0.17);
            Jup.Term(1, -3, 0, -0.03, -0.21, 0.38, -0.06, 0.00, -0.02);
            Jup.Term(2, -1, 0, -0.16, 0.05, -0.18, -0.31, 0.01, 0.00);
            Jup.Term(2, -2, 0, 0.14, -2.73, 9.23, 0.48, 0.00, 0.00);
            Jup.Term(2, -3, 0, 0.07, -0.55, 1.83, 0.25, 0.01, 0.00);
            Jup.Term(2, -4, 0, 0.02, -0.08, 0.25, 0.06, 0.00, 0.00);
            Jup.Term(3, -2, 0, 0.01, -0.07, 0.16, 0.04, 0.00, 0.00);
            Jup.Term(3, -3, 0, -0.16, -0.03, 0.08, -0.64, 0.00, 0.00);
            Jup.Term(3, -4, 0, -0.04, -0.01, 0.03, -0.17, 0.00, 0.00);

            dl += Jup.dl(); dr += Jup.dr(); db += Jup.db();

            // Возмущения Сатурна
            Sat.Init(T, M3, 0, 2, M6, -2, -1);

            Sat.Term(0, -1, 0, 0.00, 0.32, 0.01, 0.00, 0.00, 0.00);
            Sat.Term(1, -1, 0, -0.08, -0.41, 0.97, -0.18, 0.00, -0.01);
            Sat.Term(1, -2, 0, 0.04, 0.10, -0.23, 0.10, 0.00, 0.00);
            Sat.Term(2, -2, 0, 0.04, 0.10, -0.35, 0.13, 0.00, 0.00);

            dl += Sat.dl(); dr += Sat.dr(); db += Sat.db();

            // Разница между Земля-Луна-барицентром и центром Земли
            dl += +6.45 * Math.Sin(D) - 0.42 * Math.Sin(D - A) + 0.18 * Math.Sin(D + A)
                  + 0.17 * Math.Sin(D - M3) - 0.06 * Math.Sin(D + M3);

            dr += +30.76 * Math.Cos(D) - 3.06 * Math.Cos(D - A) + 0.85 * Math.Cos(D + A)
                  - 0.58 * Math.Cos(D + M3) + 0.57 * Math.Cos(D - M3);

            db += +0.576 * Math.Sin(U);

            // Долгосрочные периодические возмущения
            dl += +6.40 * Math.Sin(Const.pi2 * (0.6983 + 0.0561 * T))
                  + 1.87 * Math.Sin(Const.pi2 * (0.5764 + 0.4174 * T))
                  + 0.27 * Math.Sin(Const.pi2 * (0.4189 + 0.3306 * T))
                  + 0.20 * Math.Sin(Const.pi2 * (0.3581 + 2.4814 * T));

            // Эклиптические координаты ([рад], [AU])
            l = Const.pi2 * MathEx.Frac(0.7859453 + M3 / Const.pi2 +
                           ((6191.2 + 1.1 * T) * T + dl) / 1296.0e3);
            r = 1.0001398 - 0.0000007 * T + dr * 1.0e-6;
            b = db / Const.Arcs;

            // Позиция вектора
            return new Vec3D(new Polar(l, b, r));
        }

        /// <summary>
        /// Вычислить прямое восхождение Солнца и склонение с помощью низкой точности аналитической серии 
        /// </summary>
        /// <param name="T">Юлианский Календарь J2000</param>
        /// <param name="RA">прямое восхождение Солнца в рад</param>
        /// <param name="Dec">склонение Солнца в рад</param>
        public static void Mini(double T, ref double RA, ref double Dec)
        {
            double eps = 23.43929111 * Const.Rad;

            double L, M;
            Vec3D e_Sun;

            // Аномалия эклиптики и долготы
            M = Const.pi2 * MathEx.Frac(0.993133 + 99.997361 * T);
            L = Const.pi2 * MathEx.Frac(0.7859453 + M / Const.pi2 + (6893.0 * Math.Sin(M) + 72.0 * Math.Sin(2.0 * M) + 6191.2 * T) / 1296.0e3);

            // Экваториальные координаты
            Mat3D mat3d = new Mat3D();
            e_Sun = Mat3D.R_x(-eps) * new Vec3D(new Polar(L, 0.0));

            RA = e_Sun[Vec3DPolIndex.phi];
            Dec = e_Sun[Vec3DPolIndex.theta];
        }
    }
}
