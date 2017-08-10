using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LearningEpisodePhase
/// </summary>
public class LearningEpisodePhase
{
    static DbService db = new DbService();

    int code, episodeCode;
    string name;

    public LearningEpisodePhase()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public LearningEpisodePhase(int code, int episodeCode, string name)
    {
        Code = code;
        EpisodeCode = episodeCode;
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

    public int EpisodeCode
    {
        get
        {
            return episodeCode;
        }

        set
        {
            episodeCode = value;
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

    public static List<LearningEpisodePhase> GetPhases(int episodeCode)
    {
        List<LearningEpisodePhase> phases = new List<LearningEpisodePhase>();

        SqlParameter parameter = new SqlParameter("@epiCode", episodeCode);

        DataSet ds = db.GetDataSetByQuery("GetPhases", System.Data.CommandType.StoredProcedure,parameter);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            LearningEpisodePhase phase = new LearningEpisodePhase();
            phase.Code = int.Parse(dr[0].ToString());
            phase.EpisodeCode = int.Parse(dr[1].ToString());
            phase.Name = dr[2].ToString();

            phases.Add(phase);
        }

        return phases;
    }
}