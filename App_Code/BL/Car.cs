using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Car
/// </summary>
public class Car
{
    static DbService db = new DbService();

    int code, km, nefah;
    int productionYear;
    string imgLocation, color, licenseDate, lastTipul, insuranceDate, licenseNumber;

    Manufacturer manufaturer;
    Model model;
    Gear gear;
    School schoolBelonged;
    Teacher user;

    #region props and ctor

    public Car()
    {
        //
        // TODO: Add constructor logic here
        //
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

    public int Km
    {
        get
        {
            return km;
        }

        set
        {
            km = value;
        }
    }

    public int Nefah
    {
        get
        {
            return nefah;
        }

        set
        {
            nefah = value;
        }
    }

    public int ProductionYear
    {
        get
        {
            return productionYear;
        }

        set
        {
            productionYear = value;
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

    public string Color
    {
        get
        {
            return color;
        }

        set
        {
            color = value;
        }
    }

    public string LicenseDate
    {
        get
        {
            return licenseDate;
        }

        set
        {
            licenseDate = value;
        }
    }

    public string LastTipul
    {
        get
        {
            return lastTipul;
        }

        set
        {
            lastTipul = value;
        }
    }

    public Manufacturer Manufaturer
    {
        get
        {
            return manufaturer;
        }

        set
        {
            manufaturer = value;
        }
    }

    public Model Model
    {
        get
        {
            return model;
        }

        set
        {
            model = value;
        }
    }

    public Gear Gear
    {
        get
        {
            return gear;
        }

        set
        {
            gear = value;
        }
    }

    public School SchoolBelonged
    {
        get
        {
            return schoolBelonged;
        }

        set
        {
            schoolBelonged = value;
        }
    }

    public Teacher User
    {
        get
        {
            return user;
        }

        set
        {
            user = value;
        }
    }

    public string LicenseNumber
    {
        get
        {
            return licenseNumber;
        }

        set
        {
            licenseNumber = value;
        }
    }

    public string InsuranceDate
    {
        get
        {
            return insuranceDate;
        }

        set
        {
            insuranceDate = value;
        }
    }

    #endregion

    public static List<Car> GetTeachersCars(string teacherid)
    {
        List<Car> cars = new List<Car>();

        SqlParameter parameter = new SqlParameter("@teacherID", teacherid);

        DataSet ds = db.GetDataSetByQuery("GetTeachersCars", System.Data.CommandType.StoredProcedure, parameter);

        DataTable dt;
        DataRow dtRow;

        string query;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Car c = new Car();
            c.Code = int.Parse(dr["CarCode"].ToString());
            c.InsuranceDate = dr["InsuranceDate"].ToString();
            c.LicenseDate = dr["LicenseDate"].ToString();
            c.LastTipul = dr["LastTipul"].ToString();
            c.LicenseNumber = dr["LicenseNumber"].ToString();
            c.Color = dr["color"].ToString();
            c.ImgLocation = dr["img"].ToString();
            c.ProductionYear = int.Parse(dr["ProductionYear"].ToString());
            c.Km = int.Parse(dr["KMTraveled"].ToString());
            c.Nefah = int.Parse(dr["nefah"].ToString());

            int manuCode = int.Parse(dr["ManufacturerCode"].ToString());
            query = "select * from [dbo].[CarManufacturer] where [ManufacturerCode] = " + manuCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Manufacturer manu1 = new Manufacturer(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

            int modelCode = int.Parse(dr["ModelCode"].ToString());
            query = "select * from [dbo].[CarModel] where [ModelCode] = " + modelCode + " and ManufacturerCode = " + manuCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Model model1 = new Model(int.Parse(dtRow[1].ToString()), int.Parse(dtRow[0].ToString()), dtRow[2].ToString());

            int gearCode = int.Parse(dr["GearCode"].ToString());
            query = "select * from [dbo].[CarGear] where [GearCode] = " + gearCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Gear gear1 = new Gear(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

            Teacher t1 = Person.SearchForUserInDB(teacherid) as Teacher;

            int schoolId = int.Parse(dr["BelongsToSchoolID"].ToString());
            query = "select * from [dbo].[School] where [SchoolID] = " + schoolId;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];

            School s1 = new School(schoolId, dtRow[1].ToString());

            c.User = t1;
            c.SchoolBelonged = s1;
            c.Manufaturer = manu1;
            c.Model = model1;
            c.Gear = gear1;

            cars.Add(c);
        }

        return cars;
    }

    public static List<Car> GetAllCarsInSchool(int schoolID)
    {
        List<Car> cars = new List<Car>();

        SqlParameter parameter = new SqlParameter("@schoolID", schoolID);

        DataSet ds = db.GetDataSetByQuery("GetAllCarsInSchool", System.Data.CommandType.StoredProcedure, parameter);

        DataTable dt;
        DataRow dtRow;

        string query;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Car c = new Car();
            c.Code = int.Parse(dr["CarCode"].ToString());
            c.InsuranceDate = dr["InsuranceDate"].ToString();
            c.LicenseDate = dr["LicenseDate"].ToString();
            c.LastTipul = dr["LastTipul"].ToString();
            c.LicenseNumber = dr["LicenseNumber"].ToString();
            c.Color = dr["color"].ToString();
            c.imgLocation = dr["img"].ToString();
            c.ProductionYear = int.Parse(dr["ProductionYear"].ToString());
            c.Km = int.Parse(dr["KMTraveled"].ToString());
            c.Nefah = int.Parse(dr["nefah"].ToString());

            int manuCode = int.Parse(dr["ManufacturerCode"].ToString());
            query = "select * from [dbo].[CarManufacturer] where [ManufacturerCode] = " + manuCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Manufacturer manu1 = new Manufacturer(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

            int modelCode = int.Parse(dr["ModelCode"].ToString());
            query = "select * from [dbo].[CarModel] where [ModelCode] = " + modelCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Model model1 = new Model(int.Parse(dtRow[1].ToString()), int.Parse(dtRow[0].ToString()), dtRow[2].ToString());

            int gearCode = int.Parse(dr["GearCode"].ToString());
            query = "select * from [dbo].[CarGear] where [GearCode] = " + gearCode;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];
            Gear gear1 = new Gear(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

            string teacherId = dr["UsedByID"].ToString();

            Teacher t1 = Person.SearchForUserInDB(teacherId) as Teacher;

            query = "select * from [dbo].[School] where [SchoolID] = " + schoolID;

            dt = db.GetDataSetByQuery(query).Tables[0];
            dtRow = dt.Rows[0];

            School s1 = new School(schoolID, dtRow[1].ToString());

            c.User = t1;
            c.SchoolBelonged = s1;
            c.Manufaturer = manu1;
            c.Model = model1;
            c.Gear = gear1;

            cars.Add(c);
        }

        return cars;
    }

    public static Car GetCar(string licenseNumber)
    {
        SqlParameter parameter = new SqlParameter("@licenseNumber", licenseNumber);

        DataSet ds = db.GetDataSetByQuery("GetCar", System.Data.CommandType.StoredProcedure, parameter);

        DataTable dt;
        DataRow dtRow;

        string query;

        DataRow dr = ds.Tables[0].Rows[0];

        Car c = new Car();
        c.Code = int.Parse(dr["CarCode"].ToString());
        c.InsuranceDate = dr["InsuranceDate"].ToString();
        c.LicenseDate = dr["LicenseDate"].ToString();
        c.LastTipul = dr["LastTipul"].ToString();
        c.LicenseNumber = dr["LicenseNumber"].ToString();
        c.Color = dr["color"].ToString();
        c.ImgLocation = dr["img"].ToString();
        c.ProductionYear = int.Parse(dr["ProductionYear"].ToString());
        c.Km = int.Parse(dr["KMTraveled"].ToString());
        c.Nefah = int.Parse(dr["nefah"].ToString());

        string teacherid = dr["UsedByID"].ToString();

        int manuCode = int.Parse(dr["ManufacturerCode"].ToString());
        query = "select * from [dbo].[CarManufacturer] where [ManufacturerCode] = " + manuCode;

        dt = db.GetDataSetByQuery(query).Tables[0];
        dtRow = dt.Rows[0];
        Manufacturer manu1 = new Manufacturer(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

        int modelCode = int.Parse(dr["ModelCode"].ToString());
        query = "select * from [dbo].[CarModel] where [ModelCode] = " + modelCode;

        dt = db.GetDataSetByQuery(query).Tables[0];
        dtRow = dt.Rows[0];
        Model model1 = new Model(int.Parse(dtRow[1].ToString()), int.Parse(dtRow[0].ToString()), dtRow[2].ToString());

        int gearCode = int.Parse(dr["GearCode"].ToString());
        query = "select * from [dbo].[CarGear] where [GearCode] = " + gearCode;

        dt = db.GetDataSetByQuery(query).Tables[0];
        dtRow = dt.Rows[0];
        Gear gear1 = new Gear(int.Parse(dtRow[0].ToString()), dtRow[1].ToString());

        Teacher t1 = Person.SearchForUserInDB(teacherid) as Teacher;

        int schoolId = int.Parse(dr["BelongsToSchoolID"].ToString());
        query = "select * from [dbo].[School] where [SchoolID] = " + schoolId;

        dt = db.GetDataSetByQuery(query).Tables[0];
        dtRow = dt.Rows[0];

        School s1 = new School(schoolId, dtRow[1].ToString());

        c.User = t1;
        c.SchoolBelonged = s1;
        c.Manufaturer = manu1;
        c.Model = model1;
        c.Gear = gear1;

        return c;
    }

    public int CreateNewCar(int manuCode, int modelCode, int gearCode, int schoolId, string teacherId)
    {
        SqlParameter[] parameters = new SqlParameter[14];
        parameters[0] = new SqlParameter("@manu", manuCode);
        parameters[1] = new SqlParameter("@model", modelCode);
        parameters[2] = new SqlParameter("@year", ProductionYear);
        parameters[3] = new SqlParameter("@gear", gearCode);
        parameters[4] = new SqlParameter("@color", Color);
        parameters[5] = new SqlParameter("@licenseNumber", LicenseNumber);
        parameters[6] = new SqlParameter("@insurance", InsuranceDate);
        parameters[7] = new SqlParameter("@licenseDate", LicenseDate);
        parameters[8] = new SqlParameter("@tipul", LastTipul);
        parameters[9] = new SqlParameter("@km", Km);
        parameters[10] = new SqlParameter("@nefa", Nefah);
        parameters[11] = new SqlParameter("@teacher", teacherId);
        parameters[12] = new SqlParameter("@school", schoolId);
        parameters[13] = new SqlParameter("@img", ImgLocation);

        return db.ExecuteQuery("AddCar", System.Data.CommandType.StoredProcedure, parameters);

    }

    public void UpdateCar(string teacherId)
    {
        SqlParameter[] parameters = new SqlParameter[7];
        parameters[0] = new SqlParameter("@licenseNumber", LicenseNumber);
        parameters[1] = new SqlParameter("@insurance", InsuranceDate);
        parameters[2] = new SqlParameter("@licenseDate", LicenseDate);
        parameters[3] = new SqlParameter("@tipul", LastTipul);
        parameters[4] = new SqlParameter("@km", Km);
        parameters[5] = new SqlParameter("@teacher", teacherId);
        parameters[6] = new SqlParameter("@img", ImgLocation);

        db.ExecuteQuery("UpdateCar", System.Data.CommandType.StoredProcedure, parameters);

    }

    public static void DeleteCar(string licenseNumber)
    {
        SqlParameter parameter = new SqlParameter("@licenseNumber", licenseNumber);

        db.ExecuteQuery("DeleteCar", System.Data.CommandType.StoredProcedure, parameter);

    }
}