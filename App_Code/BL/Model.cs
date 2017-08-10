using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Model
/// </summary>
public class Model
{
    static DbService db = new DbService();

    int code, manufacturerCode;
    string name;

    public Model()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Model(int code, int manufacturerCode, string name)
    {
        Code = code;
        ManufacturerCode = manufacturerCode;
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

    public int ManufacturerCode
    {
        get
        {
            return manufacturerCode;
        }

        set
        {
            manufacturerCode = value;
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

    public static List<Model> GetAllModels(int manufacturer)
    {
        List<Model> models = new List<Model>();

        SqlParameter parameter = new SqlParameter("@manuID", manufacturer);

        DataSet ds = db.GetDataSetByQuery("GetModels", System.Data.CommandType.StoredProcedure, parameter);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Model m = new Model();
            m.Code = int.Parse(dr[1].ToString());
            m.ManufacturerCode = int.Parse(dr[0].ToString());
            m.Name = dr[2].ToString();

            models.Add(m);
        }

        return models;
    }

    public static void AddNewCarModel(string manuName, string modelName)
    {
        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@manuName", manuName);
        parameters[1] = new SqlParameter("@modelName", modelName);

        db.ExecuteQuery("AddNewCarModel", System.Data.CommandType.StoredProcedure, parameters);

    }
}