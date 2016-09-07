using System;
using MoonCalc;
using SqlDataAccess;

namespace DAL
{
    public class Rhythms
    {
        #region Properties
        public int RhythmID { get; set; }
        public int LunarDay { get; set; }
        public int IndividualLunarDay { get; set; }
        public ZodiacSign Zodiac { get; set; }
        public Phase Phase { get; set; }
        public Eclipse Eclipse { get; set; }
        public bool NewYear { get; set; }
        #endregion

        public Rhythms(Calculate calc)
        {
            LunarDay = calc.lunarDay;
            IndividualLunarDay = calc.individualLunarDay;
            Zodiac = calc.zodiac;
            Phase = calc.phase;
            Eclipse = calc.eclipse;
            NewYear = calc.newYear;
        }

        public static int GetID(Calculate calc)
        {
            Rhythms rhythm = new Rhythms(calc);
            StoredProcedure sp = new StoredProcedure("Rhythms_GetID");
            sp["LunarDay"].Value = rhythm.LunarDay;
            sp["IndividualLunarDay"].Value = rhythm.IndividualLunarDay;
            sp["Zodiac"].Value = (int)rhythm.Zodiac;
            sp["Phase"].Value = (int)rhythm.Phase;
            sp["Eclipse"].Value = (int)rhythm.Eclipse;
            sp["NewYear"].Value = rhythm.NewYear;
            int id = Convert.ToInt32(sp.ExecuteScalar());
            Logger.Log.Info(id);
            return id;
        }

        public int Save()
        {
            StoredProcedure sp = new StoredProcedure("Rhythms_Save");
            sp["RhythmID"].Value = RhythmID;
            sp["LunarDay"].Value = LunarDay;
            sp["IndividualLunarDay"].Value = IndividualLunarDay;
            sp["Zodiac"].Value = (int)Zodiac;
            sp["Phase"].Value = (int)Phase;
            sp["Eclipse"].Value = (int)Eclipse;
            sp["NewYear"].Value = NewYear;
            int id = Convert.ToInt32(sp.ExecuteScalar());
            Logger.Log.Info(id);
            return id;
        }

        public static int Save(Calculate calc)
        {
            return new Rhythms(calc).Save();
        }
    }
}
