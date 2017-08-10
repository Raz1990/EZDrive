<%@ Application Language="C#" %>

<script RunAt="server">

    DbService db = new DbService();
    static System.Timers.Timer emptyRowsChecker = new System.Timers.Timer();

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

        int timeToWait = 60 * 1000 * 5; //60 * 1 second * 5 times = 5 minutes

        SetTimerTime(timeToWait);

        emptyRowsChecker.Elapsed += RowsChecker;

        emptyRowsChecker.Start();
    }

    private void RowsChecker(object source, EventArgs myEventArgs)
    {
        db.EmptyRowsChecker();
    }

    public static void SetTimerTime(int time)
    {
        emptyRowsChecker.Interval = time;
    }

    public static void StopTimer()
    {
        emptyRowsChecker.Stop();
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        emptyRowsChecker.Dispose();
        db.EmptyRowsChecker();
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
