using System;
using SqlDataAccess;

namespace DAL
{
    public class TasksStat
    {
        public static int Save(DateTime dateTime)
        {
            StoredProcedure sp = new StoredProcedure("TasksStat_Save");
            sp["DateTime"].Value = dateTime;
            return Convert.ToInt32(sp.ExecuteScalar());
        }
    }
}
