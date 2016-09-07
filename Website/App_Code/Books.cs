using System;
using System.Collections.Generic;
using System.Data;

public class Books
{
    private string xmlFilePath;

    #region Properties
    public string Key { get; set; }
    public int CategoryID { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string CoverURI { get; set; }
    public string DownloadURI { get; set; }
    public string OrderURL { get; set; }
    #endregion

    private Books()
    {
    }

    public Books(string appDataPathPhysical)
    {
        xmlFilePath = appDataPathPhysical + "Books.xml";
    }

    public List<Books> GetAll()
    {
        DataSet ds = new DataSet();
        ds.ReadXml(xmlFilePath);
        List<Books> bList = new List<Books>();
        List<string> catList = new List<string>();
        foreach (DataTable dt in ds.Tables)
        {
            foreach (DataRow dr in dt.Rows)
            {
                switch (dt.TableName)
                {
                    case "Category":
                        catList.Add(dr["Name"].ToString());
                        break;

                    case "Book":
                        int categoryID = Convert.ToInt32(dr["Category_Id"]);
                        bList.Add(new Books()
                        {
                            Key = dr["Key"].ToString(),
                            CategoryID = categoryID,
                            Category = catList[categoryID],
                            Name = dr["Name"].ToString(),
                            Author = dr["Author"].ToString(),
                            Description = dr["Description"].ToString(),
                            CoverURI = dr["CoverURI"].ToString(),
                            DownloadURI = dr["DownloadURI"].ToString(),
                            OrderURL = dr["OrderURL"].ToString()
                        });
                        break;
                }
            }
        }
        return bList;
    }

    public Books Get(string key)
    {
        List<Books> bList = GetAll();
        return bList.Find(b => b.Key == key);
    }

    public List<string> GetCategories()
    {
        DataSet ds = new DataSet();
        ds.ReadXml(xmlFilePath);
        List<string> catList = new List<string>();
        foreach (DataTable dt in ds.Tables)
        {
            if (dt.TableName == "Category")
            {
                foreach (DataRow dr in dt.Rows)
                    catList.Add(dr["Name"].ToString());
            }
        }
        return catList;
    }

    public List<Books> GetByCategoryID(int categoryID)
    {
        List<Books> bList = GetAll();
        return bList.FindAll(b => b.CategoryID == categoryID);
    }
}
