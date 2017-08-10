using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Gear
/// </summary>
public class Gear
{
    static DbService db = new DbService();

    int code;
    string name;

    public Gear()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Gear(int code, string name)
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

    public static List<Gear> GetAllGears()
    {
        List<Gear> gears = new List<Gear>();
        DataSet ds = db.GetDataSetByQuery("GetGears", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Gear g = new Gear();
            g.Code = int.Parse(dr[0].ToString());
            g.Name = dr[1].ToString();

            gears.Add(g);
        }

        return gears;
    }
}