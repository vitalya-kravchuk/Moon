using System;
using SqlDataAccess;

namespace DAL
{
    public class UserSettings
    {
        #region Properties
        public int PersonID { get; set; }
        public eChapter Chapter { get; set; }
        #endregion

        public UserSettings()
        {
            Chapter = eChapter.Common;
        }

        public static UserSettings GetByPersonID(int personID)
        {
            StoredProcedure sp = new StoredProcedure("UserSettings_GetByPersonID");
            sp["PersonID"].Value = personID;
            Logger.Log.Info(personID);
            UserSettings userSettings = sp.ExecuteSingle<UserSettings>();
            if (userSettings == null)
                userSettings = new UserSettings();
            return userSettings;
        }

        public void Save()
        {
            StoredProcedure sp = new StoredProcedure("UserSettings_Save");
            sp["PersonID"].Value = PersonID;
            sp["Chapter"].Value = (int)Chapter;
            Logger.Log.InfoFormat("PersonID: {0}", PersonID);
            sp.ExecuteScalar();
        }
    }
}
