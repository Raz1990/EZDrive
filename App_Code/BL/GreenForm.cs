using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GreenForm
/// </summary>
public class GreenForm
{
    static DbService db = new DbService();

    string studentId, productionDate, doctor, eyes, theory, img;

    #region ctor and props

    public GreenForm()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string Doctor
    {
        get
        {
            return doctor;
        }

        set
        {
            doctor = value;
        }
    }

    public string Eyes
    {
        get
        {
            return eyes;
        }

        set
        {
            eyes = value;
        }
    }

    public string Img
    {
        get
        {
            return img;
        }

        set
        {
            img = value;
        }
    }

    public string ProductionDate
    {
        get
        {
            return productionDate;
        }

        set
        {
            productionDate = value;
        }
    }

    public string StudentId
    {
        get
        {
            return studentId;
        }

        set
        {
            studentId = value;
        }
    }

    public string Theory
    {
        get
        {
            return theory;
        }

        set
        {
            theory = value;
        }
    }

    #endregion

    public static GreenForm GetGreenForm(string id)
    {
        SqlParameter parameter = new SqlParameter("@studentID", id);

        DataSet ds = db.GetDataSetByQuery("GetGreenForm", System.Data.CommandType.StoredProcedure, parameter);

        DataRow dr = ds.Tables[0].Rows[0];

        GreenForm green = new GreenForm();
        green.StudentId = dr[1].ToString();
        green.ProductionDate = dr[2].ToString();
        green.Doctor = dr[3].ToString();
        green.Eyes = dr[4].ToString();
        green.Theory = dr[5].ToString();
        green.Img = dr[6].ToString();

        return green;
    }

    public static void Update(string id, string produce, string doctor, string eyes, string theory, string img)
    {
        SqlParameter[] parameters = new SqlParameter[6];
        parameters[0] = new SqlParameter("@studentID", id);
        parameters[1] = new SqlParameter("@produce", produce);
        parameters[2] = new SqlParameter("@doc", doctor);
        parameters[3] = new SqlParameter("@eye", eyes);
        parameters[4] = new SqlParameter("@theory", theory);
        parameters[5] = new SqlParameter("@img", img);

        db.ExecuteQuery("UpdateGreenForm", System.Data.CommandType.StoredProcedure, parameters);
    }
}