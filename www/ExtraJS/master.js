
//will look for keywords and return *false* if found nothing to match
function VoiceCommands(command)
{
    if (command == "התנתק" || command == "ביי" || command == "צא" || command == "להתנתק") {
        Hitnatkut();
        return true;
    }
    return false;
}

function Hitnatkut() {
    alert("להתראות");
    document.cookie = "autoLogin=; expires=Thu, 01 Jan 1970 00:00:00 UTC";
    GENERAL.USER.logOutUser();
    location.href = "loginPage.html";
}

events_arr = [];

var GENERAL = {

    webServerAddress: "../WebService.asmx",
    //webServerAddress: "http://proj.ruppin.ac.il/bgroup45/prod/WebService.asmx",
    //uploadFileAddress: "../ReturnValue.ashx",
    uploadFileAddress: "http://proj.ruppin.ac.il/bgroup45/prod/ReturnValue.ashx",

    PushNotificationSenderID: "554906213045",

    PICTURE: {
        path: "http://proj.ruppin.ac.il/bgroup45/prod/www/images/"
    },

    REMEMBER: {
        setRememberMe: function () {
            localStorage.remember = "true";
        },
        setForgetMe: function () {
            localStorage.remember = "false";
        },
        getRemember: function () {
            return localStorage.remember;
        },
        setLogon: function () {
            localStorage.logon = "true";
        },
        setLogoff: function () {
            localStorage.logon = "false";
        },
        getLogon: function () {
            return localStorage.logon;
        },
    },

    USER: {
        //whole user functions
        setUserInfo: function (userInfo) {
            localStorage.user = JSON.stringify(userInfo);
        },
        getUserInfo: function () {
            try {
                InfoToReturn = JSON.parse(localStorage.user);
            } catch (e) {
                return null;
            }
            return InfoToReturn;
        },
        getUserInfoSTRING: function () {
            return localStorage.user;
        },
        logOutUser: function () {
            localStorage.StudentTeacher = null;
            localStorage.user = null;
        },

        //specific info functions
        getUserFirstName: function () {
            return GENERAL.USER.getUserInfo().firstName;
        },
        getUserLastName: function () {
            return GENERAL.USER.getUserInfo().lastName;
        },
        getUserFullName: function () {
            return GENERAL.USER.getUserInfo().firstName + " " + GENERAL.USER.getUserInfo().lastName;
        },
        getPassword: function () {
            return GENERAL.USER.getUserInfo().password;
        },
    }
    ,
    ADMIN: {
        setAdminSchool: function (AdminSchool) {
            localStorage.AdminSchool = JSON.stringify(AdminSchool);
        },
        getAdminSchool: function () {
            try {
                AdminToReturn = JSON.parse(localStorage.AdminSchool);
            } catch (e) {
                return null;
            }
            return AdminToReturn;
        },
    }
    ,
    STUDENT: {
        setStudentTeacher: function (StudentTeacher) {
            localStorage.StudentTeacher = JSON.stringify(StudentTeacher);
        },
        getStudentTeacher: function () {
            try {
                TeacherToReturn = JSON.parse(localStorage.StudentTeacher);
            } catch (e) {
                return null;
            }
            return TeacherToReturn;
        },

        setCurrentStudent: function (Student) {
            localStorage.CurrentStudent = JSON.stringify(Student);
        },
        getCurrentStudent: function () {
            try {
                StudentToReturn = JSON.parse(localStorage.CurrentStudent);
            } catch (e) {
                return null;
            }
            return StudentToReturn;
        },
    }
    ,
    EVENT: {
        setEventInfo: function (eventInfo) {
            events_arr.push(eventInfo);
            localStorage.events = JSON.stringify(events_arr);
        },
        getEventsInfo: function () {
            try {
                EventsToReturn = JSON.parse(localStorage.events);
            } catch (e) {
                return null;
            }
            return EventsToReturn;
        },
        resetEvents: function ()
        {
            localStorage.events = null;
            events_arr = [];
        }
    }
    ,
    CAR: {
        setCarInfo: function (carInfo) {
            localStorage.carInfo = JSON.stringify(carInfo);
        },
        getCarInfo: function () {
            try {
                CarInfoReturn = JSON.parse(localStorage.carInfo);
            } catch (e) {
                return null;
            }
            return CarInfoReturn;
        }
    }
}
