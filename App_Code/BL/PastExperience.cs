using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PastExperience
/// </summary>
public class PastExperience
{
    public int PreviousLicenses { get; set; }
    public int LastLessonsTook { get; set; }
    public int LastTestPassed { get; set; }
    public int TheoryNumberPassed { get; set; }
    public int WantedLicenseCode { get; set; }
    public int CarsAmount { get; set; }
    public int LessonsInWeekWanted { get; set; }
    public bool Technical { get; set; }
    public string Working { get; set; }
    

    public PastExperience()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public PastExperience(int previousLicenses, int lastLessonsTook, int lastTestPassed, int theoryNumberPassed, int wantedLicenseCode, int carsAmount, int lessonsInWeekWanted, bool technical, string working)
    {
        PreviousLicenses = previousLicenses;
        LastLessonsTook = lastLessonsTook;
        LastTestPassed = lastTestPassed;
        TheoryNumberPassed = theoryNumberPassed;
        WantedLicenseCode = wantedLicenseCode;
        CarsAmount = carsAmount;
        LessonsInWeekWanted = lessonsInWeekWanted;
        Technical = technical;
        Working = working;
    }

}