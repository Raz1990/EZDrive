using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Student
/// </summary>
public class Student:Person
{
    PastExperience exp = new PastExperience();
    List<Lesson> lessonsTaken = new List<Lesson>();

    #region Props and Constructor

    public Student()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Student(int activeStatusCode, string id, string firstName, string lastName, string pass, string mobileNumber, string platform, string mail, int genderCode, int cityCode, int streetCode, string birth, string pic) 
        :base(activeStatusCode, id, firstName, lastName, pass, mobileNumber, platform, mail, genderCode, cityCode, streetCode, birth, pic)
    {

    }

    public Student(string id, string firstName, string lastName, string mobileNumber) 
        :base(id, firstName, lastName, mobileNumber)
    {

    }

    public Student(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street)
    : base(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, platform, mail, genderCode, gender, cityCode, city, streetCode, street)
    {

    }

    public Student(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string notificationID, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street, string register, string birth, string img)
        : base(sqlID, activeStatusCode,activeStatus, id, pass, firstName, lastName, mobileNumber, notificationID, platform, mail, genderCode, gender, cityCode, city, streetCode, street, register, birth, img)
    {

    }

    public PastExperience Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    public List<Lesson> LessonsTaken
    {
        get
        {
            return lessonsTaken;
        }

        set
        {
            lessonsTaken = value;
        }
    }

    #endregion

    public void AddPastExperience(int previousLicenses, int lastLessonsTook, int lastTestPassed, int theoryNumberPassed, int wantedLicenseCode, int carsAmount, int lessonsInWeekWanted, bool technical, string working)
    {
        Exp = new PastExperience(previousLicenses, lastLessonsTook, lastTestPassed, theoryNumberPassed, wantedLicenseCode, carsAmount, lessonsInWeekWanted, technical, working);
    }

    public void AddPastExperience2(PastExperience xp)
    {
        Exp = xp;
    }

    public void AddNewLesson()
    {
        //COMPLETE REAL LESSON CTOR
        Lesson newLesson = new Lesson();

        LessonsTaken.Add(newLesson);
    }

    public static Teacher GetTeachingTeacher(string studentID)
    {
        return db.GetTeacherOverStudent(studentID);
    }

    public int AddUserToDataBase(int schoolID, string teacherID)
    {
        SqlParameter[] parameters = new SqlParameter[16];
        parameters[0] = new SqlParameter("@ID", Id);
        parameters[1] = new SqlParameter("@Password", Password);
        parameters[2] = new SqlParameter("@First_Name", FirstName);
        parameters[3] = new SqlParameter("@Last_Name", LastName);
        parameters[4] = new SqlParameter("@MobileNumber", MobileNumber);
        parameters[5] = new SqlParameter("@NotificationNumber", NotificationNumber);
        parameters[6] = new SqlParameter("@PhonePlatform", Platform);
        parameters[7] = new SqlParameter("@EMail", Email);
        parameters[8] = new SqlParameter("@GenderCode", GenderCode);
        parameters[9] = new SqlParameter("@CityCode", CityCode);
        parameters[10] = new SqlParameter("@StreetCode", StreetCode);
        parameters[11] = new SqlParameter("@ActiveCode", ActiveStatusCode);
        parameters[12] = new SqlParameter("@schoolID", schoolID);
        parameters[13] = new SqlParameter("@teacherID", teacherID);
        parameters[14] = new SqlParameter("@birthdate", Birthdate);
        parameters[15] = new SqlParameter("@img", ImgLocation);

        return db.ExecuteQuery("InsertStudent", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static List<int> GetStudentProgress(string id)
    {
        SqlParameter parameter = new SqlParameter("@studentID", id);

        DataTable dt = db.GetDataSetByQuery("GetProgress", System.Data.CommandType.StoredProcedure, parameter).Tables[0];

        List<int> phases = new List<int>();

        foreach (DataRow dr in dt.Rows)
        {
            phases.Add((int)dr[0]);
        }

        phases.Sort();

        return phases;
    }

    public static void AddProgress(string id, int[] progress)
    {
        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@studentID", id);

        foreach (int level in progress)
        {
            parameters[1] = new SqlParameter("@phaseNumber", level);
            db.ExecuteQuery("AddProgress", System.Data.CommandType.StoredProcedure, parameters);
        }
    }
}