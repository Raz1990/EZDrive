using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PersonalRequest
/// </summary>
public class PersonalRequest
{
    static DbService db = new DbService();

    int requestId;
    string makerId, content, answer, date, hour;
    bool done;
    Student writer;
    Lesson lessonTiedTo;

    #region ctor and props

    public PersonalRequest()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public PersonalRequest(int requestId, string makerId, string content, string answer, bool done, string date, string hour)
    {
        RequestId = requestId;
        MakerId = makerId;
        Content = content;
        Answer = answer;
        Done = done;
        Date = date;
        Hour = hour;
    }

    public int RequestId
    {
        get
        {
            return requestId;
        }

        set
        {
            requestId = value;
        }
    }

    public string Content
    {
        get
        {
            return content;
        }

        set
        {
            content = value;
        }
    }

    public string Answer
    {
        get
        {
            return answer;
        }

        set
        {
            answer = value;
        }
    }

    public bool Done
    {
        get
        {
            return done;
        }

        set
        {
            done = value;
        }
    }

    public string Date
    {
        get
        {
            return date;
        }

        set
        {
            date = value;
        }
    }

    public string Hour
    {
        get
        {
            return hour;
        }

        set
        {
            hour = value;
        }
    }

    public Student Writer
    {
        get
        {
            return writer;
        }

        set
        {
            writer = value;
        }
    }

    public string MakerId
    {
        get
        {
            return makerId;
        }

        set
        {
            makerId = value;
        }
    }

    public Lesson LessonTiedTo
    {
        get
        {
            return lessonTiedTo;
        }

        set
        {
            lessonTiedTo = value;
        }
    }

    #endregion

    public static List<PersonalRequest> GetAllPersonalRequestsForATeacher(string teacherID, bool done)
    {
        List<PersonalRequest> requests = new List<PersonalRequest>();

        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@teacherID", teacherID);
        parameters[1] = new SqlParameter("@done", done);

        DataSet ds = db.GetDataSetByQuery("GetAllPersonalRequestsForATeacher", System.Data.CommandType.StoredProcedure, parameters);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            PersonalRequest req = new PersonalRequest();
            req.RequestId = int.Parse(dr[0].ToString());
            req.Content = dr[1].ToString();
            req.Date = dr[2].ToString();
            req.Hour = dr[3].ToString();
            req.Answer = dr[4].ToString();
            req.Done = (bool)dr[6];
            req.Writer = Person.SearchForUserInDB(dr[5].ToString()) as Student;

            SqlParameter parameter = new SqlParameter("@reqID", req.RequestId);

            ds = db.GetDataSetByQuery("GetEventDetailesFromAPersonalRequest", System.Data.CommandType.StoredProcedure, parameter);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr2 = ds.Tables[0].Rows[0];

                req.LessonTiedTo = new Lesson();
                req.LessonTiedTo.EventCode = int.Parse(dr2[0].ToString());
                req.LessonTiedTo.EventDate = dr2[4].ToString();
            }

            requests.Add(req);
        }

        return requests;
    }

    public static List<PersonalRequest> GetAllPersonalRequestsForAStudent(string studentID, bool done)
    {
        List<PersonalRequest> requests = new List<PersonalRequest>();

        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] =  new SqlParameter("@studentID", studentID);
        parameters[1] = new SqlParameter("@done", done);

        DataSet ds = db.GetDataSetByQuery("GetAllPersonalRequestsForAStudent", System.Data.CommandType.StoredProcedure,parameters);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            PersonalRequest req = new PersonalRequest();
            req.RequestId = int.Parse(dr[0].ToString());
            req.Content = dr[1].ToString();
            req.Date = dr[2].ToString();
            req.Hour = dr[3].ToString();
            req.Answer = dr[4].ToString();
            req.Done = (bool)dr[5];
            req.Writer = Person.SearchForUserInDB(studentID) as Student;

            SqlParameter parameter = new SqlParameter("@reqID", req.RequestId);

            ds = db.GetDataSetByQuery("GetEventDetailesFromAPersonalRequest", System.Data.CommandType.StoredProcedure, parameter);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr2 = ds.Tables[0].Rows[0];

                req.LessonTiedTo = new Lesson();
                req.LessonTiedTo.EventCode = int.Parse(dr2[0].ToString());
                req.LessonTiedTo.EventDate = dr2[4].ToString();
            }
            requests.Add(req);
        }

        return requests;
    }

    public static void AnswerPersonalRequest(int reqID, string content)
    {
        SqlParameter[] parameters = new SqlParameter[2];

        parameters[0] = new SqlParameter("@RequestID", reqID);
        parameters[1] = new SqlParameter("@Answer", content);

        db.ExecuteQuery("AnswerPersonalRequest", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static int InsertPersonalRequest(string studentId, string teacherId, string content)
    {
        SqlParameter[] parameters = new SqlParameter[3];
        parameters[0] = new SqlParameter("@studentID", studentId);
        parameters[1] = new SqlParameter("@TeacherID", teacherId);
        parameters[2] = new SqlParameter("@RequestContent", content);

        db.ExecuteQuery("AddPersonalRequest", System.Data.CommandType.StoredProcedure, parameters);

        int id= (int)db.GetObjectScalarByQuery("GetLastInsertedPersonalRequest", System.Data.CommandType.StoredProcedure);

        return id;
    }

    public static void DeleteLastPersonalRequest()
    {
        db.ExecuteQuery("DeleteLastInsertedPersonalRequest", System.Data.CommandType.StoredProcedure);
    }
}