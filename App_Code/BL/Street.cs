using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Street
/// </summary>
public class Street
{
    static DbService db = new DbService();

    int code;
    int cityCode;
    string name;

    #region Constructor & Props

    public Street()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Street(int code, int cityCode, string name)
    {
        Code = code;
        Name = name;
        CityCode = cityCode;
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

    public int CityCode
    {
        get
        {
            return cityCode;
        }

        set
        {
            cityCode = value;
        }
    }

    #endregion

    public static List<Street> GetStreets(int cityCode = 0)
    {
        List<Street> streets = new List<Street>();

        SqlParameter parameter = new SqlParameter("@CityCode", cityCode);

        DataSet ds = db.GetDataSetByQuery("GetStreets", System.Data.CommandType.StoredProcedure, parameter);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Street s = new Street();
            s.Code = int.Parse(dr[0].ToString());
            s.CityCode = int.Parse(dr[1].ToString());
            s.Name = dr[2].ToString();

            streets.Add(s);
        }

        return streets;
    }
}