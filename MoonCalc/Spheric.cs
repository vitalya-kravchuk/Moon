namespace MoonCalc
{
    /// <summary>
    /// Преобразования сферической астрономии
    /// </summary>
    class Spheric
    {
        /// <summary>
        /// Преобразование экваториальных в эклиптичиские координаты
        /// </summary>
        /// <param name="T">Юлианский календарь J2000</param>
        /// <returns>Матрица преобразования</returns>
        public static Mat3D Equ2EclMatrix(double T)
        {
            double eps = (23.43929111 - (46.8150 + (0.00059 - 0.001813 * T) * T) * T / 3600.0) * Const.Rad;
            return Mat3D.R_x(eps);
        }

        /// <summary>
        /// Преобразование эклиптических в экваториальные координаты
        /// </summary>
        /// <param name="T">Юлианский календарь J2000</param>
        /// <returns>Матрица преобразования</returns>
        public static Mat3D Ecl2EquMatrix(double T)
        {
            return Mat3D.Transp(Equ2EclMatrix(T));
        }
    }
}
