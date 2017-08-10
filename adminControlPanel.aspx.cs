using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adminControlPanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_Start(object sender, EventArgs e)
    {
        int time = Convert.ToInt32(timeSeconds.Text) * 1000;

        ASP.global_asax.SetTimerTime(time);
    }

    protected void btn_End(object sender, EventArgs e)
    {
        ASP.global_asax.StopTimer();
    }

    protected void btnDef_Click(object sender, EventArgs e)
    {
        int time = 60 * 1000 * 5;

        ASP.global_asax.SetTimerTime(time);
    }
}