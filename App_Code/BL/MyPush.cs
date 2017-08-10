using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPush
/// </summary>
public class MyPush
{
    protected static DbService db = new DbService();

    string push_str, platform, id;

    #region ctor and props

    public MyPush()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string Platform
    {
        get
        {
            return platform;
        }

        set
        {
            platform = value;
        }
    }

    public string Push_str
    {
        get
        {
            return push_str;
        }

        set
        {
            push_str = value;
        }
    }
    #endregion

    public static void InsertPushID(string user_id, string platform, string pushID)
    {
        SqlParameter[] parameters = new SqlParameter[3];
        parameters[0] = new SqlParameter("@user_id", user_id);
        parameters[1] = new SqlParameter("@platform", platform);
        parameters[2] = new SqlParameter("@pushID", pushID);

        db.ExecuteQuery("InsertPushID", System.Data.CommandType.StoredProcedure, parameters);

    }

    public static MyPush GetPushInfo(string id)
    {
        SqlParameter parameter = new SqlParameter("@id", id);

        MyPush push = new MyPush();

        DataTable dt = db.GetDataSetByQuery("GetPushInfo", System.Data.CommandType.StoredProcedure, parameter).Tables[0];
        DataRow dr;

        try
        {
            dr = dt.Rows[0];
        }
        catch (Exception)
        {
            return null;
        }

        push.Id = (string)dr[0];
        push.Platform = (string)dr[1];
        push.Push_str = (string)dr[2];

        return push;

    }

}