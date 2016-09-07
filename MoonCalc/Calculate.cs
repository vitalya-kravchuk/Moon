using System;
using System.Collections.Generic;
using System.Reflection;

namespace MoonCalc
{
    /// <summary>
    /// Символ лунного дня
    /// </summary>
    public enum SymbolDay
    {
        Lamp,
        Cornucopia,
        Leopard,
        TreeKnowledge,
        Unicorn,
        Crane,
        WindRose,
        Phoenix,
        Bat,
        Fountain,
        Corona,
        Heart,
        Ring,
        Pipe,
        Snakes,
        Dove,
        Grape,
        Mirror,
        Spider,
        Eagle,
        Horse,
        Elephant,
        Crocodile,
        Bear,
        Turtle,
        Frog,
        Ship,
        Lotus,
        Octopus,
        GoldenSwan
    }

    /// <summary>
    /// Вычисления лунных ритмов
    /// </summary>
    [Serializable]
    public class Calculate
    {
        #region Properties
        public System.DateTime dateTime { get; set; }

        public int lunarDay { get; set; }
        public System.DateTime dtLunarDay { get; set; }
        public System.DateTime dtLunarDayEnd { get; set; }
        public int individualLunarDay { get; set; }

        public System.DateTime dtRise { get; set; }
        public System.DateTime dtSet { get; set; }

        public Phase phase { get; set; }
        public System.DateTime dtPhase { get; set; }
        public System.DateTime dtPhaseEnd { get; set; }

        public Eclipse eclipse { get; set; }

        public ZodiacSign zodiac { get; set; }
        public System.DateTime dtZodiac { get; set; }
        public System.DateTime dtZodiacEnd { get; set; }

        public bool newYear { get; set; }
        #endregion

        public override string ToString()
        {
            string s = string.Empty;
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                s += p.Name + ": " + p.GetValue(this, null) + " \t";
            }
            return s;
        }

        /// <summary>
        /// Лунные ритмы на момент даты рождения
        /// </summary>
        /// <param name="settings">Настройки вычислений лунных ритмов</param>
        /// <returns>Возвращает null, если не указана дата рождения в местоположении</returns>
        public static Calculate GetBirthday(Settings settings)
        {
            // День рожденье
            if (settings.Birthday == System.DateTime.MinValue) return null;
            System.DateTime dateTime = settings.Birthday;

            // Поиск лунных суток
            List<Calculate> cList = GetList(dateTime, settings, 0, true);
            return cList.FindLast(c => c.dtLunarDay <= dateTime);
        }

        /// <summary>
        /// Лунные ритмы на определенную дату. Учитывает проверки и поправки
        /// </summary>
        /// <param name="cList">Список вычислений лунных ритмов на месяц + вспомогательные вычисления</param>
        /// <param name="dateTime">Определенная дата</param>
        public static Calculate Get(List<Calculate> cList, System.DateTime dateTime)
        {
            Calculate calc = cList.FindLast(c => c.dateTime <= dateTime);

            // Поправка относительно начала лунного дня
            calc.dtLunarDayEnd = cList.Find(c => c.dtLunarDay > calc.dtLunarDay).dtLunarDay;
            bool correctLunarDay = false;
            Calculate _cLD = null;
            if (dateTime < calc.dtLunarDay)
            {
                _cLD = cList.FindLast(c => c.dtLunarDay <= dateTime);
                calc.dtLunarDay = _cLD.dtLunarDay;
                calc.dtLunarDayEnd = cList.Find(c => c.dtLunarDay > calc.dtLunarDay).dtLunarDay;
                correctLunarDay = true;
            }
            if (dateTime > calc.dtLunarDayEnd)
            {
                _cLD = cList.Find(c => c.dtLunarDay > dateTime);
                _cLD = cList.FindLast(c => c.dtLunarDay < _cLD.dtLunarDay);
                calc.dtLunarDay = _cLD.dtLunarDay;
                calc.dtLunarDayEnd = cList.Find(c => c.dtLunarDay > calc.dtLunarDay).dtLunarDay;
                correctLunarDay = true;
            }
            if (correctLunarDay)
            {
                calc.lunarDay = _cLD.lunarDay;
                calc.individualLunarDay = _cLD.individualLunarDay;
                calc.dtRise = _cLD.dtRise;
                calc.dtSet = _cLD.dtSet;
                calc.newYear = _cLD.newYear;
            }

            // Поправка относительно начала фазы
            //if (dateTime < calc.dtPhase)
            {
                Calculate _c = cList.FindLast(c => c.dtPhase <= dateTime);
                calc.dtPhase = _c.dtPhase;
                calc.phase = _c.phase;
                calc.eclipse = _c.eclipse;
            }
            calc.dtPhaseEnd = cList.Find(c => c.dtPhase > calc.dtPhase).dtPhase;

            // Поправка относительно транзита Луны
            //if (dateTime < calc.dtZodiac)
            {
                Calculate _c = cList.FindLast(c => c.dtZodiac <= dateTime);
                calc.dtZodiac = _c.dtZodiac;
                calc.zodiac = _c.zodiac;
            }
            calc.dtZodiacEnd = cList.Find(c => c.dtZodiac > calc.dtZodiac).dtZodiac;

            return calc;
        }

        /// <summary>
        /// Список вычислений лунных ритмов на месяц
        /// </summary>
        /// <param name="dateTime">Определенная дата</param>
        /// <param name="settings">Настройки вычислений лунных ритмов</param>
        /// <param name="birthdayLunarDay">Лунные сутки на момент даты рождения</param>
        public static List<Calculate> GetList(System.DateTime dateTime, Settings settings, int birthdayLunarDay)
        {
            return GetList(dateTime, settings, birthdayLunarDay, false);
        }

        /// <summary>
        /// Список вычислений лунных ритмов на месяц
        /// </summary>
        /// <param name="dateTime">Определенная дата</param>
        /// <param name="settings">Настройки вычислений лунных ритмов</param>
        /// <param name="birthdayLunarDay">Лунные сутки на момент даты рождения</param>
        /// <param name="helperCalc">Вспомогательные вычисления</param>
        public static List<Calculate> GetList(System.DateTime dateTime, Settings settings, int birthdayLunarDay, bool helperCalc)
        {
            // Переход на летнее время
            DaylightSavingTime daylightSavingTime = null;
            if (settings.DST)
                daylightSavingTime = DaylightSavingTime.GetDaylightSavingTime(dateTime);

            // Фаза и затмение
            List<PhaseEclipse> peList = PhaseEclipse.Get(dateTime.Year, dateTime.Month - 1, dateTime.Month + 1, settings.TimeZone, daylightSavingTime);
            System.DateTime dtNewMoon = (peList.Find(p => p.dateTime <= dateTime && p.phase == Phase.NewMoon)).dateTime;

            // Восход и закат
            System.DateTime dtRiseSetTo = new System.DateTime(dateTime.Year, dateTime.Month,
                System.DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
            TimeSpan tsRiseSetPeriod = dtRiseSetTo - dtNewMoon;
            int countRiseSetDays = Convert.ToInt32(tsRiseSetPeriod.TotalDays) + 3;
            if (helperCalc) countRiseSetDays += 30;
            List<RiseSet> rsList = RiseSet.Get(dtNewMoon.Year, dtNewMoon.Month, dtNewMoon.Day,
                settings.Latitude, settings.Longitude, settings.TimeZone, daylightSavingTime, countRiseSetDays);

            // Зодиак
            List<Zodiac> zList = Zodiac.Get(dateTime, settings.TimeZone, daylightSavingTime, helperCalc);

            // Подготовка
            List<Calculate> cList = new List<Calculate>();
            PhaseEclipse phaseEclipse = peList[0];
            RiseSet riseSet = rsList[0];
            Zodiac zodiac = zList[0];
            int lunarDay = 1;
            cList.Add(GetFirstLunarDay(phaseEclipse, riseSet, zodiac,
                GetIndividualLunarDay(lunarDay, birthdayLunarDay)));

            int i_from = 0;
            System.DateTime dtLunarDay2 = rsList[0].riseDateTime;
            if (dtLunarDay2 == System.DateTime.MinValue)
                dtLunarDay2 = rsList[0].setDateTime;
            if (dtLunarDay2 < cList[0].dtLunarDay)
                i_from = 1;

            // Формирование списка вычислений
            for (int i = i_from; i < rsList.Count; i++)
            {
                riseSet = rsList[i];

                // 
                System.DateTime dt = riseSet.setDateTime;
                if (riseSet.riseDateTime > System.DateTime.MinValue)
                {
                    dt = riseSet.riseDateTime;
                    lunarDay++;
                    System.DateTime dtFindPhase = new System.DateTime(riseSet.riseDateTime.Year, riseSet.riseDateTime.Month, riseSet.riseDateTime.Day, 23, 59, 59);
                    phaseEclipse = peList.FindLast(p => p.dateTime <= dtFindPhase);
                }

                // 
                System.DateTime dtFindZodiac = new System.DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                Zodiac findZodiac = zList.FindLast(z => z.dateTime <= dtFindZodiac);
                if (findZodiac != null) zodiac = findZodiac;

                if (lunarDay >= 28)
                {
                    if (phaseEclipse.phase == Phase.NewMoon)
                    {
                        if (dt > phaseEclipse.dateTime)
                        {
                            lunarDay = 1;
                            cList.Add(GetFirstLunarDay(phaseEclipse, riseSet, zodiac,
                                GetIndividualLunarDay(lunarDay, birthdayLunarDay)));
                            lunarDay = 2;
                        }
                    }
                }

                //
                bool correctLunarDay2 = false;
                if (lunarDay == 2)
                {
                    if (riseSet.setDateTime != System.DateTime.MinValue)
                    {
                        if (riseSet.setDateTime == cList[cList.Count - 1].dtSet)
                        {
                            correctLunarDay2 = true;
                        }
                    }
                }

                //
                System.DateTime dtLunarDay = riseSet.riseDateTime;
                if (dtLunarDay == System.DateTime.MinValue)
                {
                    if (cList.Count > 0)
                        dtLunarDay = cList[cList.Count - 1].dtLunarDay;
                }

                //
                Calculate calculate = new Calculate()
                {
                    dateTime = dt,
                    lunarDay = lunarDay,
                    dtLunarDay = dtLunarDay,
                    individualLunarDay = GetIndividualLunarDay(lunarDay, birthdayLunarDay),
                    phase = phaseEclipse.phase,
                    dtPhase = phaseEclipse.dateTime,
                    eclipse = phaseEclipse.eclipse,
                    dtZodiac = zodiac.dateTime,
                    zodiac = zodiac.sign
                };

                //
                if (correctLunarDay2 == false)
                {
                    calculate.dtRise = riseSet.riseDateTime;
                    calculate.dtSet = riseSet.setDateTime;
                }

                // долгая лунная ночь
                if (cList.Count > 0)
                {
                    System.DateTime dtLN1 = cList[cList.Count - 1].dtLunarDay;
                    System.DateTime dtLN2 = dt;
                    dtLN1 = new System.DateTime(dtLN1.Year, dtLN1.Month, dtLN1.Day, 0, 0, 0);
                    dtLN2 = new System.DateTime(dtLN2.Year, dtLN2.Month, dtLN2.Day, 0, 0, 0);
                    TimeSpan tsLN = dtLN2 - dtLN1;
                    if (tsLN.TotalDays > 1)
                    {
                        Calculate cLN = new Calculate();
                        Helper.CopyObject(cList[cList.Count - 1], ref cLN);
                        cLN.dateTime = cLN.dateTime.AddDays(1);
                        // обновить знак зодиака
                        System.DateTime dtFindZodiacLN = new System.DateTime(cLN.dateTime.Year, cLN.dateTime.Month, cLN.dateTime.Day, 23, 59, 59);
                        Zodiac findZodiacLN = zList.FindLast(z => z.dateTime <= dtFindZodiacLN);
                        if (findZodiacLN != null)
                        {
                            cLN.dtZodiac = findZodiacLN.dateTime;
                            cLN.zodiac = findZodiacLN.sign;
                        }
                        //
                        cList.Add(cLN);
                    }
                }

                //
                cList.Add(calculate);

                //
                if ((dt.Month > dateTime.Month || dt.Year > dateTime.Year) && i > rsList.Count - 5)
                    break;
            }

            //
            if (helperCalc)
            {
                return cList;
            }
            else
            {
                System.DateTime dtFrom = new System.DateTime(dateTime.Year, dateTime.Month, 1);
                System.DateTime dtTo = new System.DateTime(dateTime.Year, dateTime.Month,
                    System.DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59);
                return cList.FindAll(c => c.dateTime >= dtFrom && c.dateTime <= dtTo);
            }
        }

        /// <summary>
        /// Формирует данные о первом лунном дне
        /// </summary>
        /// <param name="pe">Фаза и затмение</param>
        /// <param name="riseSet">Восход и закат</param>
        /// <param name="zodiac">Зодиак</param>
        /// <param name="individualLunarDay">Индивидуальные лунные сутки</param>
        static Calculate GetFirstLunarDay(PhaseEclipse pe, RiseSet riseSet, Zodiac zodiac, int individualLunarDay)
        {
            Calculate calc = new Calculate()
            {
                dateTime = pe.dateTime,
                lunarDay = 1,
                dtLunarDay = pe.dateTime,
                individualLunarDay = individualLunarDay,
                phase = Phase.NewMoon,
                dtPhase = pe.dateTime,
                eclipse = pe.eclipse,
                dtZodiac = zodiac.dateTime,
                zodiac = zodiac.sign
            };

            // Восход и закат
            if (riseSet.riseDateTime.Day == pe.dateTime.Day)
            {
                if (riseSet.riseDateTime.TimeOfDay >= pe.dateTime.TimeOfDay)
                {
                    calc.dtRise = riseSet.riseDateTime;
                    calc.dtSet = riseSet.setDateTime;
                }
            }

            // Новый Лунный Год
            if (calc.dtLunarDay.Month == 1)
            {
                if (calc.dtLunarDay.Day >= 21) calc.newYear = true;
            }
            if (calc.newYear == false)
            {
                if (calc.dtLunarDay.Month == 2)
                {
                    if (calc.dtLunarDay.Day <= 19) calc.newYear = true;
                }
            }

            return calc;
        }

        /// <summary>
        /// Индивидуальные лунные сутки
        /// </summary>
        /// <param name="lunarDay">Текущие лунные сутки</param>
        /// <param name="birthdayLunarDay">Лунные сутки на момент даты рождения</param>
        /// <returns>Возвращает 0, если не указаны лунные сутки на момент даты рождения</returns>
        static int GetIndividualLunarDay(int lunarDay, int birthdayLunarDay)
        {
            if (birthdayLunarDay <= 0)
                return 0;
            else
            {
                int i = (lunarDay + birthdayLunarDay) - 1;
                if (i > 30) i -= 30;
                return i;
            }
        }
    }
}
