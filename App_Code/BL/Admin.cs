using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Admin
/// </summary>
public class Admin:Teacher
{
    public Admin()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Admin(int sqlID, int activeStatusCode, string activeStatus, string id, string pass, string firstName, string lastName, string mobileNumber, string mail, string notificationID, string platform, int genderCode, string gender, int cityCode, string city, int streetCode, string street, string register, string birth, string img)
        : base(sqlID, activeStatusCode, activeStatus, id, pass, firstName, lastName, mobileNumber, mail,notificationID, platform, genderCode, gender, cityCode, city, streetCode, street, register, birth, img)
    {

    }
   

    public static void ApproveStudent(string stud_id, string teach_id)
    {
        SqlParameter[] parameters = new SqlParameter[2];

        parameters[0] = new SqlParameter("@studentId", stud_id);
        parameters[1] = new SqlParameter("@teacherId", teach_id);

        db.ExecuteQuery("AdminApproveNewStudents", System.Data.CommandType.StoredProcedure, parameters);
    }
}