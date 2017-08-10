using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LearningEpisode
/// </summary>
public class LearningEpisode
{
    static DbService db = new DbService();

    int code;
    string name;
    List<LearningEpisodePhase> phases;

    #region props and ctor

    public LearningEpisode()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public LearningEpisode(int code, string name, List<LearningEpisodePhase> phases)
    {
        Code = code;
        Name = name;
        Phases = phases;
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

    public List<LearningEpisodePhase> Phases
    {
        get
        {
            return phases;
        }

        set
        {
            phases = value;
        }
    }

    #endregion

    public static List<LearningEpisode> GetEpisodes()
    {
        List<LearningEpisode> episodes = new List<LearningEpisode>();

        DataSet ds = db.GetDataSetByQuery("GetEpisodes", System.Data.CommandType.StoredProcedure);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            LearningEpisode episode = new LearningEpisode();
            episode.Code = int.Parse(dr[0].ToString());
            episode.Name = dr[1].ToString();

            episode.Phases = LearningEpisodePhase.GetPhases(episode.Code);

            episodes.Add(episode);
        }

        return episodes;
    }
}