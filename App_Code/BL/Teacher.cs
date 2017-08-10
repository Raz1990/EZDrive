using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Teacher
/// </summary>
public class Teacher: Person
{
    protected DateTime employmentStart;
    protected List<Student> students;


    #region Props and Constructor

    public Teacher()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Teacher(string id, string firstName, string lastName, string mobileNumber) 
        :base(id, firstName, lastName, mobileNumber)
    {

    }

    public Teacher(int activeStatusCode, string id, string firstName, string lastName, string pass, string mobileNumber, string platform, string mail, int genderCode, int cityCode, int streetCode,string birth, string pic) 
        :base(activeStatusCode, id, firstName, lastName, pass, mobileNumber, platform, mail, genderCode, cityCode, streetCode, birth, pic)
    {

    }

    public Teacher(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street)
        : base(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, platform, mail, genderCode, gender, cityCode, city, streetCode, street)
    {

    }

    public Teacher(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string mail, string notificationID, string platform, int genderCode, string gender, int cityCode, string city, int streetCode, string street)
    : base(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, notificationID, platform, mail, genderCode, gender, cityCode, city, streetCode, street)
    {

    }

    public Teacher(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string mail, string notificationID, string platform, int genderCode, string gender, int cityCode, string city, int streetCode, string street, string register, string birth, string img)
        : base(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, notificationID, platform, mail, genderCode, gender, cityCode, city, streetCode, street, register, birth, img)
    {

    }

    public DateTime EmploymentStart
    {
        get
        {
            return employmentStart;
        }

        set
        {
            employmentStart = value;
        }
    }

    public List<Student> Students
    {
        get
        {
            return students;
        }

        set
        {
            students = value;
        }
    }


    #endregion

    public List<Student> GetStudents()
    {
        Students = new List<Student>();

        SqlParameter teacherID = new SqlParameter("@teacherID",Id);

        DataSet ds = db.GetDataSetByQuery("GetStudents", System.Data.CommandType.StoredProcedure,teacherID);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Student s = new Student();
            
            //COMPLETE MAKING OF STUDENT

            Students.Add(s);
        }

        return Students;
    }

    public void SetEmployment(int year, int month, int day)
    {
        DateTime t = new DateTime(year, month, day);
        EmploymentStart = t;
    }

    public int AddUserToDataBase(int schoolID)
    {
        SqlParameter[] parameters = new SqlParameter[15];
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
        parameters[13] = new SqlParameter("@birthdate", Birthdate);
        parameters[14] = new SqlParameter("@img", ImgLocation);

        return db.ExecuteQuery("InsertTeacher", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static List<Student> GetAllPendingRequestsForATeacher(string teacherid)
    {
        List < Student > reqs = new List<Student>();

        SqlParameter teacherID = new SqlParameter("@id", teacherid);

        DataSet ds = db.GetDataSetByQuery("GetAllPendingRequestsForATeacher", System.Data.CommandType.StoredProcedure, teacherID);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string studentID = dr[0].ToString();

            Student s = SearchForUserInDB(studentID) as Student;

            reqs.Add(s);
        }

        return reqs;
    }

    public static void ApproveStudent(string id)
    {
        SqlParameter stuID = new SqlParameter("@studentId", id);

        db.ExecuteQuery("ApproveNewStudents", System.Data.CommandType.StoredProcedure, stuID);
    }

    public static void DISApproveStudent(string id)
    {
        SqlParameter stuID = new SqlParameter("@studentId", id);

        db.ExecuteQuery("DISApproveNewStudents", System.Data.CommandType.StoredProcedure, stuID);
    }

    public static List<Student> GetAllStudentsUnderATeacher(string teacherid, bool past)
    {
        List<Student> studs = new List<Student>();

        SqlParameter[] parameters = new SqlParameter[2];
        parameters[0] = new SqlParameter("@teacherID", teacherid);
        parameters[1] = new SqlParameter("@past", past);

        DataSet ds = db.GetDataSetByQuery("GetStudentsUnderTeacher", System.Data.CommandType.StoredProcedure, parameters);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string studentID = dr[0].ToString();

            Student s = SearchForUserInDB(studentID) as Student;

            studs.Add(s);
        }

        return studs;
    }

    public static Student GetAStudent(string teacherid, string first, string last)
    {
        List<Student> studs = new List<Student>();

        SqlParameter[] parameters = new SqlParameter[3];
        parameters[0] = new SqlParameter("@teacherID", teacherid);
        parameters[1] = new SqlParameter("@first", first);
        parameters[2] = new SqlParameter("@last", last);

        string current_id = (string)db.GetObjectScalarByQuery("GetSpecificStudent", System.Data.CommandType.StoredProcedure, parameters);

        Student s = SearchForUserInDB(current_id) as Student;

        return s;
    }

    public static List<string> GetTimeRange(string id)
    {
        SqlParameter parameter = new SqlParameter("@teacherID", id);

        DataSet ds = db.GetDataSetByQuery("GetTimeRange", System.Data.CommandType.StoredProcedure, parameter);

        DataRow dr = ds.Tables[0].Rows[0];

        List<string> timeRange = new List<string>();
        timeRange.Add(dr[0].ToString()); //beginning
        timeRange.Add(dr[1].ToString()); //ending

        return timeRange;
    }

    public static void UpdateTimeRange(string id, string start, string end)
    {
        SqlParameter[] parameters = new SqlParameter[3];
        parameters[0] = new SqlParameter("@teacherID", id);
        parameters[1] = new SqlParameter("@start", start);
        parameters[2] = new SqlParameter("@end", end);

        db.ExecuteQuery("UpdateTimeRange", System.Data.CommandType.StoredProcedure, parameters);

    }
}