using System;
using SqlDataAccess;

namespace DAL
{
    public class Persons
    {
        #region Properties
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Birth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsEmailConfirm { get; set; }
        public DateTime LastActivity { get; set; }
        #endregion

        public Persons()
        {
            Name = "Мое имя";
        }

        public static Persons Get(string email, string password)
        {
            StoredProcedure sp = new StoredProcedure("Persons_Get");
            sp["Email"].Value = email;
            sp["Password"].Value = password;
            Logger.Log.InfoFormat("Email: {0}, Password: {1}", email, password);
            return sp.ExecuteSingle<Persons>();
        }

        public static Persons GetByID(int personID)
        {
            StoredProcedure sp = new StoredProcedure("Persons_GetByID");
            sp["PersonID"].Value = personID;
            Persons person = sp.ExecuteSingle<Persons>();
            if (person != null)
                Logger.Log.Info(personID + ", " + person.Name + ", " + person.Email);
            else
                Logger.Log.Info(personID);
            return person;
        }

        public static bool EmailExists(int personID, string email)
        {
            StoredProcedure sp = new StoredProcedure("Persons_EmailExists");
            sp["PersonID"].Value = personID;
            sp["Email"].Value = email;
            Logger.Log.InfoFormat("PersonID: {0}, Email: {1}", personID, email);
            return Convert.ToInt32(sp.ExecuteScalar()) > 0;
        }

        public static Persons GetByEmail(string email)
        {
            StoredProcedure sp = new StoredProcedure("Persons_GetByEmail");
            sp["Email"].Value = email;
            Logger.Log.Info(email);
            return sp.ExecuteSingle<Persons>();
        }

        public static Persons EmailConfirm(string email)
        {
            StoredProcedure sp = new StoredProcedure("Persons_EmailConfirm");
            sp["Email"].Value = email;
            Logger.Log.Info(email);
            return sp.ExecuteSingle<Persons>();
        }

        public int Save()
        {
            StoredProcedure sp = new StoredProcedure("Persons_Save");
            sp["PersonID"].Value = PersonID;
            sp["Name"].Value = Name;
            sp["Email"].Value = Email;
            sp["Password"].Value = Password;
            sp["Birth"].Value = Birth;
            sp["RegistrationDate"].Value = RegistrationDate;
            sp["IsEmailConfirm"].Value = IsEmailConfirm;
            Logger.Log.InfoFormat("PersonID: {0}, Email: {1}", PersonID, Email);
            return Convert.ToInt32(sp.ExecuteScalar());
        }
    }
}
