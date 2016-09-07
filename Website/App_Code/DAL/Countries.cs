using System.IO;
using System.Reflection;
using System.Text;
using SqlDataAccess;

public class Countries
{
    #region Properties
    public int CountryID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double TimeZone { get; set; }
    public bool DST { get; set; }
    #endregion

    public override string ToString()
    {
        string s = string.Empty;
        foreach (PropertyInfo p in this.GetType().GetProperties())
        {
            s += p.Name + ": " + p.GetValue(this, null) + "; ";
        }
        return s;
    }

    public static Countries GetByCode(string code)
    {
        StoredProcedure sp = new StoredProcedure("Countries_GetByCode");
        sp["Code"].Value = code;
        Countries country = sp.ExecuteSingle<Countries>();
        if (country != null)
            Logger.Log.Info(code + " => " + country.ToString());
        else
            Logger.Log.Info(code + " => not found");
        return country;
    }

    public static void CreateScenarioData(string sqlFilePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("set identity_insert Countries on");
        sb.AppendLine("go");
        StoredProcedure sp = new StoredProcedure("Countries_GetAll");
        foreach (Countries country in sp.ExecuteList<Countries>())
        {
            string fields = "";
            string values = "";
            foreach (PropertyInfo p in country.GetType().GetProperties())
            {
                fields += string.Format("[{0}], ", p.Name);
                values += string.Format("'{0}', ", Helper.ToString(p.GetValue(country, null)));
            }
            fields = fields.Remove(fields.Length - 2, 2);
            values = values.Remove(values.Length - 2, 2);
            sb.AppendLine(string.Format("insert into Countries ({0}) values ({1})", fields, values));
        }
        sb.AppendLine("go");
        File.WriteAllText(sqlFilePath, sb.ToString());
        Logger.Log.Info(sqlFilePath);
    }
}
