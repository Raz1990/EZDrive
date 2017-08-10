using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

using PushSharp;
using PushSharp.Android;
using System.Web.Configuration;
//using PushSharp.Apple;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    Person p;

    public WebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public void EmptyRowsChecker()
    //{
    //    DbService db = new DbService();
    //    db.EmptyRowsChecker();
    //}

    [WebMethod]
    public void RegisterPushNumber(string user_id, string platform, string pushID)
    {
        MyPush.InsertPushID(user_id, platform, pushID);
    }

    [WebMethod]
    public void SendPushMessage(string user_id, string msg, string head)
    {
        //Create our push services broker
        var push = new PushBroker();

        MyPush p = MyPush.GetPushInfo(user_id);

        push.RegisterGcmService(new GcmPushChannelSettings(WebConfigurationManager.AppSettings["apiKey"]));
        
        push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(p.Push_str)
                              .WithJson("{\"message\": \" " + msg  + " \", \"title\": \" " + head+ " \"}"));

        //Stop and wait for the queues to drains
        push.StopAllServices();

    }

    //[WebMethod]
    //public void SendPushMessage(string user_id, string msg, string head)
    //{
    //    List<string> id = new List<string>();

    //    MyPush push = MyPush.GetPushInfo(user_id);

    //    id.Add(push.Push_str);

    //    if (push.Platform == "android")
    //    {
    //        AndroidNotification a = new AndroidNotification();
    //        a.setPushNotification(id, msg, head, "", "", "");
    //        a.SendGCMNotification();
    //    }
    //    else
    //    {
    //        IosNotification i = new IosNotification();
    //        i.setPushNotification(id, msg, head, "", "", "");
    //        i.SendGCMNotification();
    //    }
    //}

    [WebMethod]
    public string[] Login(string inputID, string inputPassword)
    {
        string[] returnInfo = new string[7]; //0 = error message OR user type, 1 = id 2 = first name, 3 = last name,  4 = password, 5 = gender, 6 registrtaion

        //if didn't find a user in the DB
        if (!SearchForPerson(inputID, inputPassword))
        {
            returnInfo[0] = "משתמש אינו מוכר במערכת";
            return returnInfo;
        }
        //if found a user, check for active status
        else if (p.ActiveStatusCode == 0)
        {
            returnInfo[0] = "משתמש זה אינו פעיל במערכת. על מנת שמצבך ישתנה לפעיל, על המורה לאשר אותך קודם במערכת";
            return returnInfo;
        }

        if (p is Student)
        {
            returnInfo[0] = "student";
            if (p.Gender == "זכר")
            {
                returnInfo[5] = "images/boyIcon.png";
            }
            else
                returnInfo[5] = "images/girlIcon.png";
        }
        else if (p is Teacher)
        {
            if (p is Admin)
            {
                returnInfo[0] = "admin";
                returnInfo[5] = "images/managerIcon.png";
            }
            else
            {
                returnInfo[0] = "teacher";
                if (p.Gender == "זכר")
                {
                    returnInfo[5] = "images/teachIcon.png";
                }
                else
                    returnInfo[5] = "images/teachfIcon.png";
            }
        }

        returnInfo[1] = p.Id;
        returnInfo[2] = p.FirstName;
        returnInfo[3] = p.LastName;
        returnInfo[4] = p.Password;
        returnInfo[6] = p.Register;
        return returnInfo;

    }

    //searches for a person in the DB with provided filters. 
    //If found, creates a Person and puts it in a session and returns true 
    [WebMethod]
    public bool SearchForPerson(string id, string password)
    {
        //if found, will return either a Student or Teacher class type
        p = Person.SearchForUserInDB(id, password);

        //if a person isn't found
        if (p == null)
        {
            return false;
        }

        return true;
    }

    [WebMethod]
    public int ChangeStatus(string id)
    {
        return Person.ChangeStatus(id);
    }

    [WebMethod]
    public string GetAllCities()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(City.GetCities());
    }

    [WebMethod]
    public string GetAllCitiesWithSchools()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(City.GetCitiesWithSchools());
    }

    [WebMethod]
    public string GetAllSchools()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(School.GetSchools());
    }

    [WebMethod]
    public string GetSchoolsInCity(int cityCode)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(School.GetSchools(cityCode));
    }

    [WebMethod]
    public string GetAllStreets(int cityCode)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Street.GetStreets(cityCode));
    }

    [WebMethod]
    public void CreateNewStudent(string id, string FName, string LName, string pass, string phone, string platform, string mail, int genderCode, int cityCode, int streetCode, int schoolID, string teacherID, string birth, string pic)
    {
        //the 0 means an active status of 0 = לא פעיל
        //will be changed to 1 when the teacher approves

        string[] splitBirth = birth.Split('-');
        string correctedBirth = splitBirth[2] + "/" + splitBirth[1] + "/" + splitBirth[0];
        Student s = new Student(0, id, FName, LName, pass, phone, platform, mail, genderCode, cityCode, streetCode, correctedBirth, pic);
        s.AddUserToDataBase(schoolID, teacherID);

        //PastExperience pastEXP = xp;

        //s.AddPastExperience2(pastEXP);

    }

    [WebMethod]
    public void UpdateUserInfo(string id, string FName, string LName, string pass, string phone, string platform, string mail, int cityCode, int streetCode)
    {
        Person p = new Person(1, id, FName, LName, pass, phone, platform, mail, cityCode, streetCode);
        p.UpdateUserInfo();
    }

    [WebMethod]
    public string GetSchoolFromTeacher(string id)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(School.GetSchoolFromTeacher(id));
    }

    [WebMethod]
    public void CreateNewTeacher(string id, string FName, string LName, string pass, string phone, string platform, string mail, int genderCode, int cityCode, int streetCode, int schoolID, string birth, string pic)
    {
        Teacher t = new Teacher(1, id, FName, LName, pass, phone, platform, mail, genderCode, cityCode, streetCode, birth, pic);
        t.AddUserToDataBase(schoolID);
    }

    [WebMethod]
    public string GetTeachersInSchool(int schoolCode, bool past)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(School.GetTeachersInSchool(schoolCode, past));
    }

    [WebMethod]
    public string GetLicenseInfo()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(License.GetLicenseList());
    }

    [WebMethod]
    public string GetLicenseInSchoolInfo(int schoolCode)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(License.GetLicenseInSchoolList(schoolCode));
    }

    [WebMethod]
    public string GetTeacherOverStudent(string studentID)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Student.GetTeachingTeacher(studentID));
    }

    [WebMethod]
    public string GetAllPersonalRequestsForATeacher(string teacherID, bool done)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(PersonalRequest.GetAllPersonalRequestsForATeacher(teacherID, done));
    }

    [WebMethod]
    public string GetAllPersonalRequestsForAStudent(string studentID, bool done)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(PersonalRequest.GetAllPersonalRequestsForAStudent(studentID, done));
    }

    [WebMethod]
    public void AnswerPersonalRequest(int reqID, string content)
    {
         PersonalRequest.AnswerPersonalRequest(reqID, content);
    }

    [WebMethod]
    public string RestorePassword(string id)
    {
        return Person.RestorePassword(id);
    }

    [WebMethod]
    public int InsertPersonalRequest(string studentId, string teacherId, string content)
    {
        return PersonalRequest.InsertPersonalRequest(studentId, teacherId, content);
    }

    [WebMethod]
    public void DeleteLastPersonalRequest()
    {
        PersonalRequest.DeleteLastPersonalRequest();
    }

    [WebMethod]
    public bool CheckIfOccupied(string teacherid, string starthour, string endhour, string Date)
    {
        return Lesson.CheckIfOccupied(teacherid, starthour, endhour, Date);
    }

    [WebMethod]
    public bool CheckIfElligible(string studentid, string Date)
    {
        return Lesson.CheckIfElligible(studentid, Date);
    } 

    [WebMethod]
    public int InsertAttemptToCreateLesson(string student, string teacherid, string start, string end, string Date)
    {
        return Lesson.InsertAttempt(student,teacherid, start, end, Date);
    }

    [WebMethod]
    public void DeleteAttempt()
    {
        Lesson.DeleteAttempt();
    }

    [WebMethod]
    public int UpdateAttemptToLesson(int eventCode, string title, string place, string endhour,  int reqID)
    {
        return Lesson.UpdateAttemptToLesson(eventCode, title, place, endhour, reqID);
    }

    [WebMethod]
    public void InsertPersonalEvent(string teacherid, string title, string start, string end, string Date)
    {
        Lesson.InsertPersonalEvent(teacherid, title, start, end, Date);
    }

    [WebMethod]
    public string GetAllEventsForASchool(int schoolId)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Lesson.GetAllEventsForASchool(schoolId));
    }

    [WebMethod]
    public string GetAllEventsForATeacher(string teacherid)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Lesson.GetAllEventsForATeacher(teacherid));
    }

    [WebMethod]
    public string GetEvent(int eventId)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Lesson.GetEvent(eventId));
    }

    [WebMethod]
    public string GetPersonalEvent(int eventId)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Lesson.GetPersonalEvent(eventId));
    }

    [WebMethod]
    public int AddNewLesson(string teacherID, string studentID, string date, bool payed)
    {
        return Lesson.AddNewLesson(teacherID, studentID, date, payed);
    }

    [WebMethod]
    public int GetLastLesson(string teacherID, string studentID)
    {
        return Lesson.GetLastLesson(teacherID, studentID);
    }

    [WebMethod]
    public string GetFirstLesson(string studentID)
    {
        return Lesson.GetFirstLesson(studentID);
    }

    [WebMethod]
    public int CalcMoneyFromStudent(string studentID)
    {
        return Lesson.CalcMoneyFromStudent(studentID);
    }

    [WebMethod]
    public int DeleteLesson(int eventId)
    {
        return Lesson.DeleteLesson(eventId);
    }

    [WebMethod]
    public void DeleteEvent(int eventId)
    {
        Lesson.DeleteEvent(eventId);
    }

    [WebMethod]
    public string GetAllPendingRequestsForATeacher(string teacherid)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Teacher.GetAllPendingRequestsForATeacher(teacherid));
    }

    [WebMethod]
    public void ApproveStudent(string id)
    {
        Teacher.ApproveStudent(id);
    }

    [WebMethod]
    public void AdminApproveStudent(string stud_id,string teach_id)
    {
        Admin.ApproveStudent(stud_id, teach_id);
    }

    [WebMethod]
    public void DISApproveStudent(string id)
    {
        Teacher.DISApproveStudent(id);
    }

    [WebMethod]
    public string GetAllStudentsUnderATeacher(string teacherid, bool past = false)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Teacher.GetAllStudentsUnderATeacher(teacherid, past));
    }

    [WebMethod]
    public string GetAStudentUnderATeacher(string teacherid, string first="", string last="")
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Teacher.GetAStudent(teacherid, first,last));
    }

    [WebMethod]
    public string GetAllTeachersInSchool(int schoolId, bool past)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(School.GetTeachersInSchool(schoolId, past));
    }

    [WebMethod]
    public string GetPerson(string id)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Person.SearchForUserInDB(id));
    }

    [WebMethod]
    public string GetPersonName(string id)
    {
        Person p = Person.SearchForUserInDB(id);
        return p.FirstName + " " + p.LastName;
    }

    [WebMethod]
    public string GetAllManufacturers()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Manufacturer.GetAllManufacturers());
    }

    [WebMethod]
    public string GetAllModels(int manufacturer)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Model.GetAllModels(manufacturer));
    }

    [WebMethod]
    public string GetAllGears()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Gear.GetAllGears());
    }

    [WebMethod]
    public string GetTeachersCars(string teacherid)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Car.GetTeachersCars(teacherid));
    }

    [WebMethod]
    public string GetSchoolCars(int schoolID)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Car.GetAllCarsInSchool(schoolID));
    }

    [WebMethod]
    public string GetCar(string licenseNumber)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(Car.GetCar(licenseNumber));
    }

    [WebMethod]
    public void CreateNewCar(int manuCode, int modelCode, int gearCode, int schoolId, string teacherId, string insuranceDate, string licenseDate, string lastTipul, string licenseNumber, string color, string img, int year, int km, int nefa)
    {
        Car c = new Car();
        c.InsuranceDate = insuranceDate;
        c.LicenseDate = licenseDate;
        c.LastTipul = lastTipul;
        c.LicenseNumber = licenseNumber.Substring(0,2) + "-" + licenseNumber.Substring(2, 3) +"-" +licenseNumber.Substring(5, 2);
        c.Color = color;
        c.ImgLocation = img;
        c.ProductionYear = year;
        c.Km = km;
        c.Nefah = nefa;

        c.CreateNewCar(manuCode, modelCode, gearCode,schoolId, teacherId);
    }

    [WebMethod]
    public void UpdateCar(string teacherId, string insuranceDate, string licenseDate, string lastTipul, string licenseNumber,string img, int km)
    {
        Car c = new Car();
        c.InsuranceDate = insuranceDate;
        c.LicenseDate = licenseDate;
        c.LastTipul = lastTipul;
        c.LicenseNumber = licenseNumber;
        c.ImgLocation = img;
        c.Km = km;

        c.UpdateCar(teacherId);
    }

    [WebMethod]
    public void DeleteCar(string licenseNumber)
    {
        Car.DeleteCar(licenseNumber);
    }

    [WebMethod]
    public string GetEpisodes()
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(LearningEpisode.GetEpisodes());
    }

    [WebMethod]
    public List<int> GetStudentProgress(string id)
    {
        return Student.GetStudentProgress(id);
    }

    [WebMethod]
    public void AddStudentProgress(string id, int[] progress)
    {
        Student.AddProgress(id,progress);
    }

    [WebMethod]
    public List<string> GetTimeRange(string id)
    {
        return Teacher.GetTimeRange(id);
    }

    [WebMethod]
    public void UpdateTimeRange(string id, string start, string end)
    {
        Teacher.UpdateTimeRange(id, start, end);
    }

    [WebMethod]
    public string GetGreenForm(string id)
    {
        JavaScriptSerializer j = new JavaScriptSerializer();

        return j.Serialize(GreenForm.GetGreenForm(id));
    }

    [WebMethod]
    public void UpdateGreenForm(string id, string produce, string doctor, string eyes, string theory, string img)
    {
        GreenForm.Update(id, produce, doctor, eyes, theory, img);
    }

    [WebMethod]
    public void AddNewCarModel(string manuName, string modelName)
    {
        Model.AddNewCarModel(manuName, modelName);
    }
}
