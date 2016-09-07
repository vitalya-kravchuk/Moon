using System;

namespace MoonCalc
{
    /// <summary>
    /// Технические и научные функции
    /// </summary>
    class MathEx
    {
        /// <summary>
        /// Дробная часть числа
        /// </summary>
        public static double Frac(double x)
        {
            return x - Math.Floor(x);
        }

        /// <summary>
        /// Вычисляет x mod y
        /// </summary>
        public static double Modulo(double x, double y)
        {
            return y * Frac(x / y);
        }

        /// <summary>
        /// Перевод угловых градусов, минут и секунд дуги в десятичные углы
        /// </summary>
        /// <param name="D">Угловой градус</param>
        /// <param name="M">Угловая минута</param>
        /// <param name="S">Угловая секунда</param>
        /// <returns>Угол в десятичном представлении</returns>
        public static double Ddd(int D, int M, double S)
        {
            double sign;
            if ((D < 0) || (M < 0) || (S < 0)) sign = -1.0; else sign = 1.0;
            return sign * (Math.Abs(D) + Math.Abs(M) / 60.0 + Math.Abs(S) / 3600.0);
        }

        /// <summary>
        /// Находит градусы, минуты и секунды дуги на заданный угол
        /// </summary>
        /// <param name="Dd">Угол в градусах в десятичном представлении</param>
        /// <param name="D">Угловых градусов</param>
        /// <param name="M">Угловых минут</param>
        /// <param name="S">Угловых секунд</param>
        public static void DMS(double Dd, ref int D, ref int M, ref double S)
        {
            double x = 0;
            x = Math.Abs(Dd);
            D = Helper.DoubleToInt32(x);
            x = (x - D) * 60.0;
            M = Helper.DoubleToInt32(x);
            S = (x - M) * 60.0;
            if (Dd < 0.0)
            {
                if (D != 0) D *= -1; else if (M != 0) M *= -1; else S *= -1.0;
            }
        }

        /// <summary>
        /// Расчет cos(alpha+beta) и sin(alpha+beta) с помощью теоремы сложения
        /// </summary>
        /// <param name="c1">cos(alpha)</param>
        /// <param name="s1">sin(alpha)</param>
        /// <param name="c2">cos(beta)</param>
        /// <param name="s2">sin(beta)</param>
        /// <param name="c">cos(alpha+beta)</param>
        /// <param name="s">sin(alpha+beta)</param>
        public static void AddThe(double c1, double s1, double c2, double s2, ref double c, ref double s)
        {
            c = c1 * c2 - s1 * s2;
            s = s1 * c2 + c1 * s2;
        }

        /// <summary>
        /// Прототип функции Pegasus: double f(double x);
        /// </summary>
        public delegate double PegasusFunct(double x);

        /// <summary>
        /// Поиск методом Пегаса
        /// </summary>
        /// <param name="f">Прототип функции Pegasus</param>
        /// <param name="LowerBound">Нижняя граница поиска</param>
        /// <param name="UpperBound">Верхняя граница поиска</param>
        /// <param name="Accuracy">Точность</param>
        /// <param name="Root">Результат</param>
        /// <param name="Success">Флаг успеха</param>
        public static void Pegasus(PegasusFunct f, double LowerBound, double UpperBound, double Accuracy, ref double Root, ref bool Success)
        {
            const int MaxIterat = 30;

            double x1 = LowerBound; double f1 = f(x1);
            double x2 = UpperBound; double f2 = f(x2);
            double x3 = 0.0; double f3 = 0.0;

            int Iterat = 0;

            // Инициализация
            Success = false;
            Root = x1;

            // Итерация
            if (f1 * f2 < 0.0)
            {
                do
                {
                    // Апроксимация методом интерполяции
                    x3 = x2 - f2 / ((f2 - f1) / (x2 - x1)); f3 = f(x3);

                    if (f3 * f2 <= 0.0)
                    {
                        x1 = x2; f1 = f2;
                        x2 = x3; f2 = f3;
                    }
                    else
                    {
                        f1 = f1 * f2 / (f2 + f3);
                        x2 = x3; f2 = f3;
                    }

                    if (Math.Abs(f1) < Math.Abs(f2))
                        Root = x1;
                    else
                        Root = x2;

                    Success = (Math.Abs(x2 - x1) <= Accuracy);
                    Iterat++;
                }
                while (!Success && (Iterat < MaxIterat));
            }
        }

        /// <summary>
        /// Квадратичная интерполяция.
        /// Выполняет поиск корней и поиск экстремальных значений на основе трех равноотстоящих значений функции
        /// </summary>
        /// <param name="y_minus">Значение функции в x = -1</param>
        /// <param name="y_0">Значение функции в x = 0</param>
        /// <param name="y_plus">Значение функции в x = 1</param>
        /// <param name="xe">По оси абсцисс экстремума (может быть за пределами [-1, 1])</param>
        /// <param name="ye">Значение функции в точке хе</param>
        /// <param name="root1">Первый корень найден</param>
        /// <param name="root2">Второй корень найден</param>
        /// <param name="n_root">Число найденных корней в [-1, 1]</param>
        public static void Quad(double y_minus, double y_0, double y_plus, ref double xe, ref double ye, ref double root1, ref double root2, ref int n_root)
        {
            double a, b, c, dis, dx;

            n_root = 0;

            // Коэффициенты интерполяции параболы y=a*x^2+b*x+c
            a = 0.5 * (y_plus + y_minus) - y_0;
            b = 0.5 * (y_plus - y_minus);
            c = y_0;

            // Поиск значений экстремума
            xe = -b / (2.0 * a);
            ye = (a * xe + b) * xe + c;

            dis = b * b - 4.0 * a * c; // Дискриминант y=a*x^2+b*x+c

            if (dis >= 0) // Парабола имеет корни
            {
                dx = 0.5 * Math.Sqrt(dis) / Math.Abs(a);

                root1 = xe - dx;
                root2 = xe + dx;

                if (Math.Abs(root1) <= 1.0) ++n_root;
                if (Math.Abs(root2) <= 1.0) ++n_root;
                if (root1 < -1.0) root1 = root2;
            }
        }
    }
}
