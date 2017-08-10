using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Person
/// </summary>
public class Person
{
    protected static DbService db = new DbService();

    protected int sqlID;
    protected int activeStatusCode;
    protected string activeStatus;
    protected string id;
    protected string password;
    protected string firstName;
    protected string lastName;
    protected string mobileNumber;
    protected string notificationNumber = "", platform;
    protected string email;
    protected int genderCode;
    protected string gender;
    protected int cityCode;
    protected string city;
    protected int streetCode;
    protected string street;
    protected string register;
    protected int type;
    protected string birthdate;
    protected string imgLocation;

    #region Props and Constructor

    public Person()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Person(string id, string firstName, string lastName, string mobileNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
    }

    //ctor to create a new person just to insert in the DB
    public Person(int activeStatusCode, string id, string firstName, string lastName, string pass, string mobileNumber, string platform, string mail, int genderCode, int cityCode, int streetCode, string birth, string pic)
    {
        ActiveStatusCode = activeStatusCode;
        Id = id;
        Password = pass;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        Platform = platform;
        Email = mail;
        GenderCode = genderCode;
        CityCode = cityCode;
        StreetCode = streetCode;
        Birthdate = birth;
        ImgLocation = pic;
    }

    public Person(int activeStatusCode, string id, string firstName, string lastName, string pass, string mobileNumber, string platform, string mail, int cityCode, int streetCode)
    {
        ActiveStatusCode = activeStatusCode;
        Id = id;
        Password = pass;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        Platform = platform;
        Email = mail;
        CityCode = cityCode;
        StreetCode = streetCode;
    }

    public Person(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street)
    {
        SqlID = sqlID;
        ActiveStatusCode = activeStatusCode;
        ActiveStatus = activeStatus;
        Id = id;
        Password = pass;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        Platform = platform;
        Email = mail;
        GenderCode = genderCode;
        Gender = gender;
        CityCode = cityCode;
        City = city;
        StreetCode = streetCode;
        Street = street;
    }
  
    public Person(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string notificationNumber, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street)
    {
        SqlID = sqlID;
        ActiveStatusCode = activeStatusCode;
        ActiveStatus = activeStatus;
        Id = id;
        Password = pass;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        NotificationNumber = notificationNumber;
        Platform = platform;
        Email = mail;
        GenderCode = genderCode;
        Gender = gender;
        CityCode = cityCode;
        City = city;
        StreetCode = streetCode;
        Street = street;
    }

    public Person(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string notificationNumber, string platform, string mail, int genderCode, string gender, int cityCode, string city, int streetCode, string street, string register, string birth, string img)
    {
        SqlID = sqlID;
        ActiveStatusCode = activeStatusCode;
        ActiveStatus = activeStatus;
        Id = id;
        Password = pass;
        FirstName = firstName;
        LastName = lastName;
        MobileNumber = mobileNumber;
        NotificationNumber = notificationNumber;
        Platform = platform;
        Email = mail;
        GenderCode = genderCode;
        Gender = gender;
        CityCode = cityCode;
        City = city;
        StreetCode = streetCode;
        Street = street;
        Register = register;
        Birthdate = birth;
        ImgLocation = img;
    }

    public int SqlID
    {
        get
        {
            return sqlID;
        }

        set
        {
            sqlID = value;
        }
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

    public string Password
    {
        get
        {
            return password;
        }

        set
        {
            password = value;
        }
    }

    public string FirstName
    {
        get
        {
            return firstName;
        }

        set
        {
            firstName = value;
        }
    }

    public string LastName
    {
        get
        {
            return lastName;
        }

        set
        {
            lastName = value;
        }
    }

    public string MobileNumber
    {
        get
        {
            return mobileNumber;
        }

        set
        {
            mobileNumber = value;
        }
    }

    public string NotificationNumber
    {
        get
        {
            return notificationNumber;
        }

        set
        {
            notificationNumber = value;
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

    public string Gender
    {
        get
        {
            return gender;
        }

        set
        {
            gender = value;
        }
    }

    public int GenderCode
    {
        get
        {
            return genderCode;
        }

        set
        {
            genderCode = value;
        }
    }

    public string City
    {
        get
        {
            return city;
        }

        set
        {
            city = value;
        }
    }

    public int CityCode
    {
        get
        {
            return cityCode;
        }

        set
        {
            cityCode = value;
        }
    }

    public string Street
    {
        get
        {
            return street;
        }

        set
        {
            street = value;
        }
    }

    public int StreetCode
    {
        get
        {
            return streetCode;
        }

        set
        {
            streetCode = value;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }

        set
        {
            email = value;
        }
    }

    public int ActiveStatusCode
    {
        get
        {
            return activeStatusCode;
        }

        set
        {
            activeStatusCode = value;
        }
    }

    public string ActiveStatus
    {
        get
        {
            return activeStatus;
        }

        set
        {
            activeStatus = value;
        }
    }

    public string Register
    {
        get
        {
            return register;
        }

        set
        {
            register = value;
        }
    }

    public int Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public string Birthdate
    {
        get
        {
            return birthdate;
        }

        set
        {
            birthdate = value;
        }
    }

    public string ImgLocation
    {
        get
        {
            return imgLocation;
        }

        set
        {
            imgLocation = value;
        }
    }

    #endregion


    public static Person SearchForUserInDB(string tz, string pass="")
    {
        return db.GetUser(tz,pass);
    }

    public static string RestorePassword(string id)
    {
        SqlParameter parameter = new SqlParameter("@id",id);

        return (string)db.GetObjectScalarByQuery("RestorePassword", System.Data.CommandType.StoredProcedure, parameter);
    }

    public void UpdateUserInfo()
    {
        SqlParameter[] parameters = new SqlParameter[10];
        parameters[0] = new SqlParameter("@ID", Id);
        parameters[1] = new SqlParameter("@First_Name", FirstName);
        parameters[2] = new SqlParameter("@Last_Name", LastName);
        parameters[3] = new SqlParameter("@MobileNumber", MobileNumber);
        parameters[4] = new SqlParameter("@PhonePlatform", Platform);
        parameters[5] = new SqlParameter("@notificationNumber", notificationNumber);
        parameters[6] = new SqlParameter("@EMail", Email);
        parameters[7] = new SqlParameter("@CityCode", CityCode);
        parameters[8] = new SqlParameter("@StreetCode", StreetCode);
        parameters[9] = new SqlParameter("@Password", Password);

        db.ExecuteQuery("UpdateUserInfo", System.Data.CommandType.StoredProcedure, parameters);
    }

    public static int ChangeStatus(string id)
    {
        SqlParameter parameter = new SqlParameter("@id", id);

        return (int)db.GetObjectScalarByQuery("ChangeActiveStatus", System.Data.CommandType.StoredProcedure, parameter);

    }
}

