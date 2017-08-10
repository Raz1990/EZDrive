using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for City
/// </summary>
public class City
{
    static DbService db = new DbService();

    int code;
    string name;

    #region Constructor & Props

    public City()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public City(int code, string name)
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

    #endregion

    public static List<City> GetCities()
    {
        List<City> cities = new List<City>();
        DataSet ds = db.GetDataSetByQuery("GetCities", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            City c = new City();
            c.Code = int.Parse(dr[0].ToString());
            c.Name = dr[1].ToString();

            cities.Add(c);
        }

        return cities;
    }

    public static List<City> GetCitiesWithSchools()
    {
        List<City> cities = new List<City>();
        DataSet ds = db.GetDataSetByQuery("GetCitiesWithSchools", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            City c = new City();
            c.Code = int.Parse(dr[0].ToString());
            c.Name = dr[1].ToString();

            cities.Add(c);
        }

        return cities;
    }
}
