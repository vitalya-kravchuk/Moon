namespace MoonCalc
{
    /// <summary>
    /// Настройки вычислений лунных ритмов
    /// </summary>
    public class Settings
    {
        #region Properties
        /// <summary>
        /// День рожденье
        /// </summary>
        public System.DateTime Birthday { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public double TimeZone { get; set; }

        /// <summary>
        /// Определять летнее/зимнее время автоматически
        /// </summary>
        public bool DST { get; set; }
        #endregion

        public Settings()
        {
            Birthday = System.DateTime.MinValue;
            Latitude = 0.0;
            Longitude = 0.0;
            TimeZone = 0;
            DST = true;
        }
    }
}
