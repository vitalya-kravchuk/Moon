using System;

namespace MoonCalc
{
    /// <summary>
    /// Прототип функции Vec3D f(double t)
    /// </summary>
    delegate Vec3D C3Dfunct(double t);

    /// <summary>
    /// Аппроксимация функции полиномами Чебышева
    /// </summary>
    class Cheb3D
    {
        C3Dfunct m_f = null;            // Прототип функции Vec3D f(double t)
        int m_n = 0;                    // Градус
        bool m_Valid = false;           // Достоверность коэффициентов Чебышева
        double m_dt = 0;                // Размер интервала
        double m_ta = 0, m_tb = 0;      // Интервал
        double[] Cx, Cy, Cz;            // Коэффициенты Чебышева

        public Cheb3D(C3Dfunct f, int n, double dt)
        {
            m_f = f;
            m_n = n;
            m_dt = dt;

            Cx = new double[n + 1];
            Cy = new double[n + 1];
            Cz = new double[n + 1];
        }

        /// <summary>
        /// Аппроксимация функции
        /// </summary>
        public void Fit(double ta, double tb)
        {
            int j, k;
            double tau, t, fac;
            double[] T = new double[m_n + 1];
            Vec3D f;

            // Очистить все коэффициенты
            for (j = 0; j <= m_n; j++) Cx[j] = Cy[j] = Cz[j] = 0.0;

            // Цикл по корням T^(n+1)
            for (k = 0; k <= m_n; k++)
            {

                tau = Math.Cos((2 * k + 1) * Const.pi / (2 * m_n + 2));
                t = ((tb - ta) / 2.0) * tau + ((tb + ta) / 2.0);
                f = m_f(t);

                // Вычислить коэффициенты C_j с помощью рекуррентной формулы
                for (j = 0; j <= m_n; j++)
                {
                    switch (j)
                    {
                        case 0: T[j] = 1.0; break;
                        case 1: T[j] = tau; break;
                        default: T[j] = 2.0 * tau * T[j - 1] - T[j - 2]; break;
                    };
                    // Прирост коэффициента C_j на f(t)*T_j(tau)
                    Cx[j] += f[Vec3DIndex.x] * T[j]; Cy[j] += f[Vec3DIndex.y] * T[j]; Cz[j] += f[Vec3DIndex.z] * T[j];
                };
            };

            fac = 2.0 / (m_n + 1);
            for (j = 0; j <= m_n; j++) { Cx[j] *= fac; Cy[j] *= fac; Cz[j] *= fac; }

            m_ta = ta;
            m_tb = tb;

            m_Valid = true;
        }

        /// <summary>
        /// Оценка приближения над аргументом T. Выполняет повторную аппроксимацию в случае необходимости
        /// </summary>
        public Vec3D Value(double t)
        {
            const double eps = 0.01; // Дробное перекрытие

            Vec3D f1, f2, old_f1;
            double tau, k;

            // Генерировать новые коэффициенты по мере необходимости
            if (!m_Valid || (t < m_ta) || (m_tb < t))
            {
                k = Math.Floor(t / m_dt);
                Fit((k - eps) * m_dt, (k + 1 + eps) * m_dt);
            }

            // Оценка приближения
            tau = (2.0 * t - m_ta - m_tb) / (m_tb - m_ta);

            f1 = new Vec3D(0, 0, 0);
            f2 = new Vec3D(0, 0, 0);

            for (int i = m_n; i >= 1; i--)
            {
                old_f1 = f1;
                f1 = 2.0 * tau * f1 - f2 + (new Vec3D(Cx[i], Cy[i], Cz[i]));
                f2 = old_f1;
            };

            return tau * f1 - f2 + 0.5 * (new Vec3D(Cx[0], Cy[0], Cz[0]));
        }
    }
}
