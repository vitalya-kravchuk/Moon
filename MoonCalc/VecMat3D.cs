using System;

namespace MoonCalc
{
    /// <summary>
    /// Идентификатор по компонентам объекта Vec3D
    /// </summary>
    enum Vec3DIndex
    {
        x = 0,
        y = 1,
        z = 2
    }

    /// <summary>
    /// Полярные углы объекта Vec3D
    /// </summary>
    enum Vec3DPolIndex
    {
        phi = 0,    // азимут вектора
        theta = 1,  // высота вектора
        r = 2       // нормаль вектора
    }

    /// <summary>
    /// Полярный угол
    /// </summary>
    class Polar
    {
        public Polar(double Az, double Elev)
        {
            phi = Az;
            theta = Elev;
            r = 1.0;
        }
        public Polar(double Az, double Elev, double R)
        {
            phi = Az;
            theta = Elev;
            r = R;
        }

        public double phi;    // азимут вектора
        public double theta;  // высота вектора
        public double r;      // нормаль вектора
    }

    /// <summary>
    /// Трехмерные векторы
    /// </summary>
    class Vec3D
    {
        // компоненты вектора
        public double[] m_Vec = new double[3];

        double m_phi;                   // полярный угол (азимут)
        double m_theta;                 // полярный угол (высота)
        double m_r;                     // нормаль вектора
        bool m_bPolarValid = false;     // статус флага за достоверность полярных координат

        // Расчет полярных координат
        void CalcPolarAngles()
        {
            // долгота проекции в X-Y-плоскости
            double rhoSqr = m_Vec[0] * m_Vec[0] + m_Vec[1] * m_Vec[1];

            // нормаль вектора
            m_r = Math.Sqrt(rhoSqr + m_Vec[2] * m_Vec[2]);

            // азимут вектора
            if ((m_Vec[0] == 0.0) && (m_Vec[1] == 0.0))
                m_phi = 0.0;
            else
                m_phi = Math.Atan2(m_Vec[1], m_Vec[0]);
            if (m_phi < 0.0) m_phi += 2.0 * Const.pi;

            // высота вектора
            double rho = Math.Sqrt(rhoSqr);
            if ((m_Vec[2] == 0.0) && (rho == 0.0))
                m_theta = 0.0;
            else
                m_theta = Math.Atan2(m_Vec[2], rho);
        }

        public Vec3D(double X, double Y, double Z)
        {
            m_Vec[0] = X;
            m_Vec[1] = Y;
            m_Vec[2] = Z;
        }

        public Vec3D(Polar polar)
        {
            m_bPolarValid = true;
            m_theta = polar.theta;
            m_phi = polar.phi;
            m_r = polar.r;

            double cosEl = Math.Cos(m_theta);

            m_Vec[0] = polar.r * Math.Cos(m_phi) * cosEl;
            m_Vec[1] = polar.r * Math.Sin(m_phi) * cosEl;
            m_Vec[2] = polar.r * Math.Sin(m_theta);
        }

        // x, y, z
        public double this[Vec3DIndex index]
        {
            get
            {
                return m_Vec[(int)index];
            }
        }

        // r, theta, phi
        public double this[Vec3DPolIndex index]
        {
            get
            {
                if (!m_bPolarValid)
                {
                    // Пересчитать нормаль и полярный угол вектора
                    CalcPolarAngles();
                    m_bPolarValid = true;
                }
                switch (index)
                {
                    case Vec3DPolIndex.r: return m_r;
                    case Vec3DPolIndex.theta: return m_theta;
                    default: return m_phi;
                }
            }
        }

        public static Vec3D operator *(double fScalar, Vec3D Vec)
        {
            return new Vec3D(
                fScalar * Vec.m_Vec[0],
                fScalar * Vec.m_Vec[1],
                fScalar * Vec.m_Vec[2]);
        }

        public static Vec3D operator *(Vec3D Vec, double fScalar)
        {
            return fScalar * Vec;
        }

        public static Vec3D operator +(Vec3D left, Vec3D right)
        {
            return new Vec3D(left.m_Vec[0] + right.m_Vec[0],
                 left.m_Vec[1] + right.m_Vec[1],
                 left.m_Vec[2] + right.m_Vec[2]);
        }

        public static Vec3D operator -(Vec3D Vec)
        {
            return new Vec3D(-Vec.m_Vec[0], -Vec.m_Vec[1], -Vec.m_Vec[2]);
        }

        public static Vec3D operator -(Vec3D left, Vec3D right)
        {
            return new Vec3D(left.m_Vec[0] - right.m_Vec[0],
                 left.m_Vec[1] - right.m_Vec[1],
                 left.m_Vec[2] - right.m_Vec[2]);
        }
    }

    /// <summary>
    /// Преобразования трехмерной матрицы
    /// </summary>
    class Mat3D
    {
        // элементы матрицы
        public double[,] m_Mat = new double[3, 3];

        public Mat3D()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    m_Mat[i, j] = 0.0;
        }

        public Mat3D(Vec3D e_1, Vec3D e_2, Vec3D e_3)
        {
            for (int i = 0; i < 3; i++)
            {
                m_Mat[i, 0] = e_1.m_Vec[i];
                m_Mat[i, 1] = e_2.m_Vec[i];
                m_Mat[i, 2] = e_3.m_Vec[i];
            }
        }

        public static Mat3D R_x(double RotAngle)
        {
            double S = Math.Sin(RotAngle);
            double C = Math.Cos(RotAngle);

            Mat3D U = new Mat3D();

            U.m_Mat[0, 0] = 1.0; U.m_Mat[0, 1] = 0.0; U.m_Mat[0, 2] = 0.0;
            U.m_Mat[1, 0] = 0.0; U.m_Mat[1, 1] = +C; U.m_Mat[1, 2] = +S;
            U.m_Mat[2, 0] = 0.0; U.m_Mat[2, 1] = -S; U.m_Mat[2, 2] = +C;

            return U;
        }

        public static Mat3D R_z(double RotAngle)
        {
            double S = Math.Sin(RotAngle);
            double C = Math.Cos(RotAngle);

            Mat3D U = new Mat3D();

            U.m_Mat[0, 0] = +C; U.m_Mat[0, 1] = +S; U.m_Mat[0, 2] = 0.0;
            U.m_Mat[1, 0] = -S; U.m_Mat[1, 1] = +C; U.m_Mat[1, 2] = 0.0;
            U.m_Mat[2, 0] = 0.0; U.m_Mat[2, 1] = 0.0; U.m_Mat[2, 2] = 1.0;

            return U;
        }

        public static Vec3D operator *(Mat3D Mat, Vec3D Vec)
        {
            Vec3D Result = new Vec3D(Vec.m_Vec[0], Vec.m_Vec[1], Vec.m_Vec[2]);

            for (int i = 0; i < 3; i++)
            {
                double Scalp = 0.0;
                for (int j = 0; j < 3; j++)
                    Scalp += Mat.m_Mat[i, j] * Vec.m_Vec[j];
                Result.m_Vec[i] = Scalp;
            }

            return Result;
        }

        public static Mat3D operator *(double fScalar, Mat3D Mat)
        {
            Mat3D Result = new Mat3D();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Result.m_Mat[i, j] = fScalar * Mat.m_Mat[i, j];

            return Result;
        }

        public static Mat3D operator *(Mat3D Mat, double fScalar)
        {
            return fScalar * Mat;
        }

        public static Vec3D operator *(Vec3D Vec, Mat3D Mat)
        {
            Vec3D Result = new Vec3D(0, 0, 0);

            for (int j = 0; j < 3; j++)
            {
                double Scalp = 0.0;
                for (int i = 0; i < 3; i++)
                    Scalp += Vec.m_Vec[i] * Mat.m_Mat[i, j];
                Result.m_Vec[j] = Scalp;
            }

            return Result;
        }

        public static Mat3D operator *(Mat3D left, Mat3D right)
        {
            Mat3D Result = new Mat3D();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double Scalp = 0.0;
                    for (int k = 0; k < 3; k++)
                        Scalp += left.m_Mat[i, k] * right.m_Mat[k, j];
                    Result.m_Mat[i, j] = Scalp;
                }
            }

            return Result;
        }

        public static Mat3D Transp(Mat3D Mat)
        {
            Mat3D T = new Mat3D();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    T.m_Mat[i, j] = Mat.m_Mat[j, i];

            return T;
        }
    }
}
