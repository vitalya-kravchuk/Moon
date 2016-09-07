namespace MoonCalc
{
    /// <summary>
    /// Математические и астрономические константы
    /// </summary>
    class Const
    {
        public const double pi = 3.14159265358979324;
        public const double pi2 = 2.0 * pi;
        public const double Rad = pi / 180.0;
        public const double Deg = 180.0 / pi;
        public const double Arcs = 3600.0 * 180.0 / pi;

        public const double MJD_J2000 = 51544.5;

        public const double R_Earth = 6378.137;
        public const double R_Sun = 696000.0;
        public const double R_Moon = 1738.0;
    }
}
