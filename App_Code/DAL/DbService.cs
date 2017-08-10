using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// Summary description for DbService
/// </summary>
public class DbService
{
    SqlTransaction tran;
    SqlCommand cmd;
    SqlConnection con;

    SqlDataAdapter adp;

    public DbService()
    {

        con = new SqlConnection(WebConfigurationManager.ConnectionStrings["db"].ConnectionString);
        //
        // TODO: Add constructor logic here
        //        
    }

    public DataSet GetDataSetByQuery(string sqlQuery, CommandType cmdType = CommandType.Text, params SqlParameter[] parametersArray)
    {
        con = new SqlConnection(WebConfigurationManager.ConnectionStrings["db"].ConnectionString);

        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = cmdType;
        DataSet ds = new DataSet();
        adp = new SqlDataAdapter(cmd);

        foreach (SqlParameter s in parametersArray)
        {
            cmd.Parameters.AddWithValue(s.ParameterName, s.Value);

        }

        try
        {
            adp.Fill(ds);
        }
        catch (Exception e)
        {
            //do something with the error
            ds = null;
        }

        return ds;
    }

    public int GetScalarByQuery(string sqlQuery, CommandType cmdType = CommandType.Text, params SqlParameter[] parametersArray)
    {
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = cmdType;
        int res = 0;

        string id = "0";
        foreach (SqlParameter s in parametersArray)
        {
            cmd.Parameters.AddWithValue(s.ParameterName, s.Value);
        }

        try
        {
            con.Open();
            id = cmd.ExecuteScalar().ToString();
            res = Convert.ToInt32(id);



        }
        catch (Exception e)
        {
            //do something with the error
        }
        finally
        {
            con.Close();
        }



        return res;
    }

    public int ExecuteQuery(string sqlQuery, CommandType cmdType = CommandType.Text, params SqlParameter[] parametersArray)
    {
        con = new SqlConnection(WebConfigurationManager.ConnectionStrings["db"].ConnectionString);

        int row_affected = 0;
        using (con)
        {
            con.Open();
            tran = con.BeginTransaction();

            cmd = new SqlCommand(sqlQuery, con, tran);
            cmd.CommandType = cmdType;

            foreach (SqlParameter s in parametersArray)
            {
                cmd.Parameters.AddWithValue(s.ParameterName, s.Value);
            }

            try
            {
                row_affected = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
            }
        }

        return row_affected;
    }

    public object GetObjectScalarByQuery(string sqlQuery, CommandType cmdType = CommandType.Text, params SqlParameter[] parametersArray)
    {
        con = new SqlConnection(WebConfigurationManager.ConnectionStrings["db"].ConnectionString);

        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = cmdType;
        object res = null;

        foreach (SqlParameter s in parametersArray)
        {
            cmd.Parameters.AddWithValue(s.ParameterName, s.Value);
        }

        try
        {
            con.Open();
            res = cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            //do something with the error
        }
        finally
        {
            con.Close();
        }

        return res;
    }

    public Person GetUser(string id, string pass="")
    {
        Person p = null;

        int sqlID, activeStatusCode, genderCode, cityCode, streetCode;
        string activeStatus, firstName, lastName, mobileNumber, phonePlatform, email, gender, city, street, pushNumber, register, birth, img;

        string query = "select * from Users where id = '" + id+ "'";

        if (pass!= "")
        {
            query += " AND Password = '" + pass + "'";
        }

        DataTable foundUser = GetDataSetByQuery(query).Tables[0];
        if (foundUser.Rows.Count>0)
        {
            DataRow row = foundUser.Rows[0];

            sqlID = Convert.ToInt32(row["SQLID"]);
            firstName = (string)row["FirstName"];
            lastName = (string)row["LastName"];
            pass = (string)row["Password"];
            mobileNumber = (string)row["MobileNumber"];
            phonePlatform = (string)row["PhonePlatform"];
            email = (string)row["EMail"];
            register = (string)row["RegistrationDate"];
            birth = (string)row["BirthDate"];
            img = (string)row["img"];

            activeStatusCode = Convert.ToInt32(row["ActiveCode"]);

            query = "select * from ActiveDescription where ActiveCode = " + activeStatusCode;
            DataTable ACTIVE = GetDataSetByQuery(query).Tables[0];
            activeStatus = (string)ACTIVE.Rows[0][1];

            genderCode = Convert.ToInt32(row["GenderCode"]);

            query = "select * from Gender where GenderCode = " + genderCode;
            DataTable GENDER = GetDataSetByQuery(query).Tables[0];
            gender = (string)GENDER.Rows[0][1];

            cityCode = Convert.ToInt32(row["CityCode"]);

            query = "select * from Cities where CityCode = " + cityCode;
            DataTable CITY = GetDataSetByQuery(query).Tables[0];
            city = (string)CITY.Rows[0][1];

            streetCode = Convert.ToInt32(row["StreetCode"]);

            query = "select * from Streets where StreetCode = " + streetCode;
            DataTable STREET = GetDataSetByQuery(query).Tables[0];
            street = (string)STREET.Rows[0][2];

            query = "select * from Push_Notification_Numbers where UserID = '" + id + "' AND PhonePlatform = '" + phonePlatform+"'";
            DataTable PUSH = GetDataSetByQuery(query).Tables[0];
            if (PUSH.Rows.Count > 0)
                pushNumber = (string)PUSH.Rows[0][3];
            else
                pushNumber = "-1";


            int type = Convert.ToInt32(row["UserTypeCode"]);

            if (type == 1) //if it's a student
            {
                p = new Student(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, pushNumber, phonePlatform, email, genderCode, gender, cityCode, city, streetCode, street, register, birth, img);
            }
            else if (type == 2) //if teacher
            {
                p = new Teacher(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, email, pushNumber, phonePlatform, genderCode, gender, cityCode, city, streetCode, street, register, birth, img);
            }
            else //if admin
                p = new Admin(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, email, pushNumber, phonePlatform, genderCode, gender, cityCode, city, streetCode, street, register, birth, img);

            return p;
        }
        return null;
    }

    public Teacher GetTeacherOverStudent(string studentID)
    {
        Teacher p = null;

        int sqlID, activeStatusCode, genderCode, cityCode, streetCode;
        string activeStatus, id, password, firstName, lastName, mobileNumber, phonePlatform, email, gender, city, street, pushNumber;
        string query;

        SqlParameter parameter = new SqlParameter("@studentID", studentID);

        DataTable foundUser = GetDataSetByQuery("GetTeacherOverStudent", System.Data.CommandType.StoredProcedure,parameter).Tables[0];

        if (foundUser != null)
        {
            DataRow row = foundUser.Rows[0];

            sqlID = Convert.ToInt32(row["SQLID"]);
            id = (string)row["id"];
            password = (string)row["Password"];
            firstName = (string)row["FirstName"];
            lastName = (string)row["LastName"];
            mobileNumber = (string)row["MobileNumber"];
            phonePlatform = (string)row["PhonePlatform"];
            email = (string)row["EMail"];

            activeStatusCode = Convert.ToInt32(row["ActiveCode"]);

            query = "select * from ActiveDescription where ActiveCode = " + activeStatusCode;
            DataTable ACTIVE = GetDataSetByQuery(query).Tables[0];
            activeStatus = (string)ACTIVE.Rows[0][1];

            genderCode = Convert.ToInt32(row["GenderCode"]);

            query = "select * from Gender where GenderCode = " + genderCode;
            DataTable GENDER = GetDataSetByQuery(query).Tables[0];
            gender = (string)GENDER.Rows[0][1];

            cityCode = Convert.ToInt32(row["CityCode"]);

            query = "select * from Cities where CityCode = " + cityCode;
            DataTable CITY = GetDataSetByQuery(query).Tables[0];
            city = (string)CITY.Rows[0][1];

            streetCode = Convert.ToInt32(row["StreetCode"]);

            query = "select * from Streets where StreetCode = " + streetCode;
            DataTable STREET = GetDataSetByQuery(query).Tables[0];
            street = (string)STREET.Rows[0][2];

            query = "select * from Push_Notification_Numbers where UserID = '" + id + "' AND PhonePlatform = '" + phonePlatform + "'";
            DataTable PUSH = GetDataSetByQuery(query).Tables[0];
            if (PUSH.Rows.Count > 0)
                pushNumber = (string)PUSH.Rows[0][3];
            else
                pushNumber = "-1";

            p = new Teacher(sqlID, activeStatusCode, activeStatus, id, password, firstName, lastName, mobileNumber, pushNumber, phonePlatform, email, genderCode, gender, cityCode, city, streetCode, street);
            
            return p;
        }
        return null;
    }

    public void EmptyRowsChecker()
    {
        ExecuteQuery("SearchForEmptyEventRows", System.Data.CommandType.StoredProcedure);
    }
}