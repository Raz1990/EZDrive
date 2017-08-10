using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for License
/// </summary>
public class License
{
    static DbService db = new DbService();

    int code;
    string letter, level, description;

    #region Props and Constructor

    public License()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public License(int code, string level, string letter, string description)
    {
        Code = code;
        Level = level;
        Letter = letter;
        Description = description;
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

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public string Letter
    {
        get
        {
            return letter;
        }

        set
        {
            letter = value;
        }
    }

    public string Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    #endregion

    public static List<License> GetLicenseList()
    {
        List<License> licenseList = new List<License>();
        DataSet ds = db.GetDataSetByQuery("GetLicenseInfo", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            License l = new License();
            l.Code = int.Parse(dr[0].ToString());
            l.Letter = dr[1].ToString();
            l.Level = dr[2].ToString();
            l.Description = dr[3].ToString();

            licenseList.Add(l);
        }

        return licenseList;
    }

    public static List<License> GetLicenseInSchoolList(int schoolCode)
    {
        List<License> licenseList = new List<License>();

        SqlParameter parameter = new SqlParameter("@SchoolID", schoolCode);

        DataSet ds = db.GetDataSetByQuery("GetLicenseTaughtInSchool", System.Data.CommandType.StoredProcedure, parameter);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            License l = new License();
            l.Code = int.Parse(dr[0].ToString());
            l.Letter = dr[1].ToString();
            l.Level = dr[2].ToString();
            l.Description = dr[3].ToString();

            licenseList.Add(l);
        }

        return licenseList;
    }
}