using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for School
/// </summary>
public class School
{
    static DbService db = new DbService();

    int code;
    string name;
    City city;
    Street street;
    List<Teacher> teachers;

    #region Props & Constructors

    public School()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public School(int code, string name)
    {
        Code = code;
        Name = name;
    }

    public School(int code, string name, City city, Street street)
    {
        Code = code;
        Name = name;
        City = city;
        Street = street;
    }

    public School(int code, string name, City city, Street street, List<Teacher> teachers)
    {
        Code = code;
        Name = name;
        City = city;
        Street = street;
        Teachers = teachers;
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

    public City City
    {
        get
        {
            return city;
        }

        set
        {
            city = value;
        }
    }

    public Street Street
    {
        get
        {
            return street;
        }

        set
        {
            street = value;
        }
    }

    public List<Teacher> Teachers
    {
        get
        {
            return teachers;
        }

        set
        {
            teachers = value;
        }
    }

    #endregion

    public static List<School> GetSchools(int cityCode = 0)
    {
        DataSet ds;
        List<School> schools = new List<School>();
        if (cityCode == 0)
        {
            ds = db.GetDataSetByQuery("GetSchools", System.Data.CommandType.StoredProcedure);
        }
        else
        {
            SqlParameter parameter = new SqlParameter("@CityCode", cityCode);
            ds = db.GetDataSetByQuery("GetSchoolsInCity", System.Data.CommandType.StoredProcedure, parameter);
        }
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            School s = new School();
            s.Code = int.Parse(dr[0].ToString());
            s.Name = dr[1].ToString();
            s.City = new City(int.Parse(dr[2].ToString()), dr[3].ToString());
            s.Street = new Street(int.Parse(dr[4].ToString()), int.Parse(dr[2].ToString()), dr[5].ToString());

            schools.Add(s);
        }

        return schools;
    }

    public static List<Teacher> GetTeachersInSchool(int schoolCode, bool past)
    {
        DataSet ds;
        List<Teacher> teachers = new List<Teacher>();

        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@SchoolId", schoolCode);
        parameters[1] = new SqlParameter("@past", past);

        ds = db.GetDataSetByQuery("GetTeachersInSchool", System.Data.CommandType.StoredProcedure, parameters);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Teacher t = Person.SearchForUserInDB(dr[0].ToString()) as Teacher;

            teachers.Add(t);
        }

        return teachers;
    }

    public static School GetSchoolFromTeacher(string id)
    {
        DataSet ds;

        SqlParameter parameter = new SqlParameter("@id", id);
        ds = db.GetDataSetByQuery("GetSchoolForATeacher", System.Data.CommandType.StoredProcedure, parameter);

        DataRow dr = ds.Tables[0].Rows[0];

        School school = new School(int.Parse(dr[0].ToString()), dr[1].ToString());

        return school;
    }
}