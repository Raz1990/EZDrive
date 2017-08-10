using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Lesson
/// </summary>
public class Lesson
{
    static DbService db = new DbService();

    int eventCode;
    string eventDate;
    string startingHour;
    string endingHour;
    int price;
    string studentId;
    string teacherId;
    string title;
    string location;
    bool status;
    PersonalRequest request;

    #region ctor and props

    public Lesson()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Lesson(int eventCode, string eventDate, string startingHour, string endingHour, int price, string studentId, string teacherId, string title, string location, PersonalRequest request)
    {
        EventCode = eventCode;
        EventDate = eventDate;
        StartingHour = startingHour;
        EndingHour = endingHour;
        Price = price;
        StudentId = studentId;
        TeacherId = teacherId;
        Title = title;
        Location = location;
        Request = request;
    }

    //ctor to make an attempt to make a lesson
    public Lesson(string eventDate, string startingHour, string studentId, string teacherId)
    {
        EventDate = eventDate;
        StartingHour = startingHour;
        StudentId = studentId;
        TeacherId = teacherId;
    }


    public int EventCode
    {
        get
        {
            return eventCode;
        }

        set
        {
            eventCode = value;
        }
    }

    public string EventDate
    {
        get
        {
            return eventDate;
        }

        set
        {
            eventDate = value;
        }
    }

    public string StartingHour
    {
        get
        {
            return startingHour;
        }

        set
        {
            startingHour = value;
        }
    }

    public string EndingHour
    {
        get
        {
            return endingHour;
        }

        set
        {
            endingHour = value;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
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

    public string TeacherId
    {
        get
        {
            return teacherId;
        }

        set
        {
            teacherId = value;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public string Location
    {
        get
        {
            return location;
        }

        set
        {
            location = value;
        }
    }

    public PersonalRequest Request
    {
        get
        {
            return request;
        }

        set
        {
            request = value;
        }
    }

    public bool Status
    {
        get
        {
            return status;
        }

        set
        {
            status = value;
        }
    }
    #endregion


    public static bool CheckIfOccupied(string teacherId, string startHour, string endhour, string fullDate)
    {
        SqlParameter[] parameters = new SqlParameter[4];
        parameters[0] = new SqlParameter("@TeacherID", teacherId);
        parameters[1] = new SqlParameter("@EventDate", fullDate);
        parameters[2] = new SqlParameter("@StartingHour", startHour);
        parameters[3] = new SqlParameter("@EndingHour", endhour);

        DataSet ds = db.GetDataSetByQuery("IsEventSlotTaken", System.Data.CommandType.StoredProcedure, parameters);

        return ds.Tables[0].Rows.Count > 0;

    }

    public static bool CheckIfElligible(string studentid, string fullDate)
    {
        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@StudentID", studentid);
        parameters[1] = new SqlParameter("@EventDate", fullDate);

        DataSet ds = db.GetDataSetByQuery("IsElligibleForLesson", System.Data.CommandType.StoredProcedure, parameters);

        return ds.Tables[0].Rows.Count > 0;

    }

    public static int InsertAttempt(string studentId, string teacherId, string startHour, string endHour, string fullDate)
    {
        SqlParameter[] parameters = new SqlParameter[5];
        parameters[0] = new SqlParameter("@TeacherID", teacherId);
        parameters[1] = new SqlParameter("@StudentId", studentId);
        parameters[2] = new SqlParameter("@EventDate", fullDate);
        parameters[3] = new SqlParameter("@StartingHour", startHour);
        parameters[4] = new SqlParameter("@EndingHour", endHour);

        //will insert the attempt and return the event code generated for that event
        int rows = (int)db.GetObjectScalarByQuery("InsertAttemptForLesson", System.Data.CommandType.StoredProcedure, parameters);
        return rows;
    }

    public static int UpdateAttemptToLesson(int EventCode, string title, string place, string endHour, int reqID)
    {
        SqlParameter[] parameters = new SqlParameter[5];
        parameters[0] = new SqlParameter("@EventCode", EventCode);
        parameters[1] = new SqlParameter("@title", title);
        parameters[2] = new SqlParameter("@place", place);
        parameters[3] = new SqlParameter("@EndingHour", endHour);
        parameters[4] = new SqlParameter("@RequestID", reqID);

        //will insert the attempt and return the event code generated for that event
        int code = db.ExecuteQuery("AddCalendarEvent", System.Data.CommandType.StoredProcedure, parameters);
        return code;

    }

    public static void DeleteAttempt()
    {
        db.ExecuteQuery("DeleteAttempt", System.Data.CommandType.StoredProcedure);
    }

    public static List<Lesson> GetAllEventsForASchool(int schoolId)
    {
        List<Lesson> lessonList = new List<Lesson>();

        SqlParameter parameter = new SqlParameter("@schoolId", schoolId);

        DataSet ds = db.GetDataSetByQuery("GetAllEventsForASchool", System.Data.CommandType.StoredProcedure, parameter);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Lesson shiur = new Lesson();
            shiur.EventCode = int.Parse(dr[0].ToString());
            shiur.StudentId = dr[1].ToString();
            shiur.TeacherId = dr[2].ToString();
            shiur.Title = dr[3].ToString();
            shiur.EventDate = dr[4].ToString();
            shiur.StartingHour = dr[5].ToString();
            shiur.EndingHour = dr[6].ToString();
            shiur.Location = dr[7].ToString();
            shiur.Status = (bool)dr[9];

            PersonalRequest thisRequest;
            int reqID = int.Parse(dr[8].ToString());

            parameter = new SqlParameter("@reqID", reqID);

            DataSet reqs = db.GetDataSetByQuery("GetPersonalRequestForALesson", System.Data.CommandType.StoredProcedure, parameter);

            if (reqs.Tables[0].Rows.Count > 0)
            {
                string maker = (string)reqs.Tables[0].Rows[0]["StudentID"];
                string content = (string)reqs.Tables[0].Rows[0]["RequestContent"];
                string answer = (string)reqs.Tables[0].Rows[0]["AnswerContent"];
                bool done = false;
                if ((bool)reqs.Tables[0].Rows[0]["answered"])
                {
                    done = true;
                }
                string date = (string)reqs.Tables[0].Rows[0]["PostingDate"];
                string hour = (string)reqs.Tables[0].Rows[0]["PostingTime"];
                thisRequest = new PersonalRequest(reqID, maker, content, answer, done, date, hour);
            }
            else
                thisRequest = new PersonalRequest();
            shiur.Request = thisRequest;

            lessonList.Add(shiur);
        }

        //personal teacher events
        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Lesson shiur = new Lesson();
                shiur.EventCode = int.Parse(dr[0].ToString());
                shiur.TeacherId = dr[1].ToString();
                shiur.Title = dr[2].ToString();
                shiur.EventDate = dr[3].ToString();
                shiur.StartingHour = dr[4].ToString();
                shiur.EndingHour = dr[5].ToString();
                shiur.StudentId = "";

                lessonList.Add(shiur);
            }
        }

        return lessonList;
    }

    public static List<Lesson> GetAllEventsForATeacher(string teacherid)
    {
        List<Lesson> lessonList = new List<Lesson>();

        SqlParameter parameter = new SqlParameter("@teacherId", teacherid);

        DataSet ds = db.GetDataSetByQuery("GetAllEventsForATeacher", System.Data.CommandType.StoredProcedure, parameter);

        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Lesson shiur = new Lesson();
                shiur.EventCode = int.Parse(dr[0].ToString());
                shiur.StudentId = dr[1].ToString();
                shiur.TeacherId = dr[2].ToString();
                shiur.Title = dr[3].ToString();
                shiur.EventDate = dr[4].ToString();
                shiur.StartingHour = dr[5].ToString();
                shiur.EndingHour = dr[6].ToString();
                shiur.Location = dr[7].ToString();
                shiur.Status = (bool)dr[9];

                PersonalRequest thisRequest;
                int reqID = int.Parse(dr[8].ToString());

                parameter = new SqlParameter("@reqID", reqID);

                DataSet requests = db.GetDataSetByQuery("GetPersonalRequestForALesson", System.Data.CommandType.StoredProcedure, parameter);

                if (requests.Tables[0].Rows.Count > 0)
                {
                    string maker = (string)requests.Tables[0].Rows[0]["StudentID"];
                    string content = (string)requests.Tables[0].Rows[0]["RequestContent"];
                    string answer = (string)requests.Tables[0].Rows[0]["AnswerContent"];
                    bool done = false;
                    if ((bool)requests.Tables[0].Rows[0]["answered"])
                    {
                        done = true;
                    }
                    string date = (string)requests.Tables[0].Rows[0]["PostingDate"];
                    string hour = (string)requests.Tables[0].Rows[0]["PostingTime"];
                    thisRequest = new PersonalRequest(reqID, maker, content, answer, done, date, hour);
                }
                else
                    thisRequest = new PersonalRequest();
                shiur.Request = thisRequest;

                lessonList.Add(shiur);
            }
        }

        //personal teacher events
        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Lesson shiur = new Lesson();
                shiur.EventCode = int.Parse(dr[0].ToString());
                shiur.TeacherId = dr[1].ToString();
                shiur.Title = dr[2].ToString();
                shiur.EventDate = dr[3].ToString();
                shiur.StartingHour = dr[4].ToString();
                shiur.EndingHour = dr[5].ToString();
                shiur.StudentId = "";

                lessonList.Add(shiur);
            }
        }

        return lessonList;
    }

    public static Lesson GetEvent(int eventId)
    {
        SqlParameter parameter = new SqlParameter("@eventId", eventId);

        DataSet ds = db.GetDataSetByQuery("GetEvent", System.Data.CommandType.StoredProcedure, parameter);

        DataRow dr = ds.Tables[0].Rows[0];

        Lesson shiur = new Lesson();
        shiur.EventCode = int.Parse(dr[0].ToString());
        shiur.StudentId = dr[1].ToString();
        shiur.TeacherId = dr[2].ToString();
        shiur.Title = dr[3].ToString();
        shiur.EventDate = dr[4].ToString();
        shiur.StartingHour = dr[5].ToString();
        shiur.EndingHour = dr[6].ToString();
        shiur.Location = dr[7].ToString();
        shiur.Status = (bool)dr[9];

        PersonalRequest thisRequest;
        int reqID = int.Parse(dr[8].ToString());

        parameter = new SqlParameter("@reqID", reqID);

        ds = db.GetDataSetByQuery("GetPersonalRequestForALesson", System.Data.CommandType.StoredProcedure, parameter);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string maker = (string)ds.Tables[0].Rows[0]["StudentID"];
            string content = (string)ds.Tables[0].Rows[0]["RequestContent"];
            string answer = (string)ds.Tables[0].Rows[0]["AnswerContent"];
            bool done = false;
            if ((bool)ds.Tables[0].Rows[0]["answered"])
            {
                done = true;
            }
            string date = (string)ds.Tables[0].Rows[0]["PostingDate"];
            string hour = (string)ds.Tables[0].Rows[0]["PostingTime"];
            thisRequest = new PersonalRequest(reqID, maker, content, answer, done, date, hour);
        }
        else
        {
            thisRequest = new PersonalRequest();
            thisRequest.Content = "";
        }
        shiur.Request = thisRequest;

        return shiur;
    }

    public static void InsertPersonalEvent(string teacherId, string title, string startHour, string endHour, string fullDate)
    {
        SqlParameter[] parameters = new SqlParameter[5];
        parameters[0] = new SqlParameter("@TeacherID", teacherId);
        parameters[1] = new SqlParameter("@title", title);
        parameters[2] = new SqlParameter("@EventDate", fullDate);
        parameters[3] = new SqlParameter("@StartingHour", startHour);
        parameters[4] = new SqlParameter("@EndingHour", endHour);

        db.GetObjectScalarByQuery("AddTeacherCalendarEvent", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static Lesson GetPersonalEvent(int eventId)
    {
        SqlParameter parameter = new SqlParameter("@eventId", eventId);

        DataSet ds = db.GetDataSetByQuery("GetPersonalEvent", System.Data.CommandType.StoredProcedure, parameter);

        DataRow dr = ds.Tables[0].Rows[0];

        Lesson shiur = new Lesson();
        shiur.EventCode = int.Parse(dr[0].ToString());
        shiur.TeacherId = dr[1].ToString();
        shiur.Title = dr[2].ToString();
        shiur.EventDate = dr[3].ToString();
        shiur.StartingHour = dr[4].ToString();
        shiur.EndingHour = dr[5].ToString();
        
        return shiur;
    }

    public static int DeleteLesson(int eventId)
    {
        SqlParameter parameter = new SqlParameter("@EventCode", eventId);

        return db.ExecuteQuery("RemoveEventFromCalendar", System.Data.CommandType.StoredProcedure, parameter);
    }

    public static void DeleteEvent(int eventId)
    {
        SqlParameter parameter = new SqlParameter("@EventCode", eventId);

        db.ExecuteQuery("RemovePersonalEventFromCalendar", System.Data.CommandType.StoredProcedure, parameter);
    }

    public static int AddNewLesson(string teacherID, string studentID, string date, bool payed)
    {
        SqlParameter[] parameters = new SqlParameter[4];
        parameters[0] = new SqlParameter("@teacherID", teacherID);
        parameters[1] = new SqlParameter("@studentID", studentID);
        parameters[2] = new SqlParameter("@date", date);
        parameters[3] = new SqlParameter("@payed", payed);

        return db.ExecuteQuery("AddLesson", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static int GetLastLesson(string teacherID, string studentID)
    {
        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@teacherID", teacherID);
        parameters[1] = new SqlParameter("@studentID", studentID);

        return (int)db.GetObjectScalarByQuery("GetLastLesson", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static string GetFirstLesson(string studentID)
    {
        SqlParameter parameter = new SqlParameter("@studentID", studentID);

        return (string)db.GetObjectScalarByQuery("GetFirstLesson", System.Data.CommandType.StoredProcedure, parameter);
    }

    public static int CalcMoneyFromStudent(string studentID)
    {
        SqlParameter parameter = new SqlParameter("@studentID", studentID);

        return (int)db.GetObjectScalarByQuery("CalcTotalMoneyFromStudent", System.Data.CommandType.StoredProcedure, parameter);
    }

}