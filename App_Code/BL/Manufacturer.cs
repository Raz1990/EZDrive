using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Manufacturer
/// </summary>
public class Manufacturer
{
    static DbService db = new DbService();

    int code;
    string name;

    public Manufacturer()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Manufacturer(int code, string name)
    {
        Code = code;
        Name = name;
    }

    public int Code
    {
        get
        {
            return code;
        }

        set
        {
            code = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public static List<Manufacturer> GetAllManufacturers()
    {
        List<Manufacturer> manus = new List<Manufacturer>();
        DataSet ds = db.GetDataSetByQuery("GetManufacturers", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Manufacturer m = new Manufacturer();
            m.Code = int.Parse(dr[0].ToString());
            m.Name = dr[1].ToString();

            manus.Add(m);
        }

        return manus;
    }
}