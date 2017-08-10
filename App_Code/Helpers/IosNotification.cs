using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for IosNotification
/// </summary>
public class IosNotification
{
    public IosNotification()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<string> registration_ids = new List<string>();
    public PushNotification notification = new PushNotification();



    public void setPushNotification(List<string> DeviceId, string Message, string headerMessage, string img, string sty, string summary,
        string sender = "")
    {

        foreach (string d in DeviceId)
        {
            registration_ids.Add(d);
        }

        notification.title = headerMessage;
        notification.body = Message;
        notification.notId = (DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond).ToString();
        notification.picture = img;
        notification.style = sty;
        notification.summaryText = summary;
    }

    public string SendGCMNotification()
    {
        if (registration_ids.Count == 0)
            return "";

        var serializer = new JavaScriptSerializer();

        var serializedResult = serializer.Serialize(this);

        //  MESSAGE CONTENT
        byte[] byteArray = Encoding.UTF8.GetBytes(serializedResult);

        //  CREATE REQUEST
        string requestTo = "https://gcm-http.googleapis.com/gcm/send";

        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(requestTo);
        Request.Method = "POST";
        Request.KeepAlive = false;
        Request.ContentType = "application/json";
        Request.Headers.Add(string.Format("Authorization: key={0}", WebConfigurationManager.AppSettings["apiKey"]));
        Request.ContentLength = byteArray.Length;

        Stream dataStream = Request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        //  SEND MESSAGE
        try
        {
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                //var text = "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                //var text = "Response from web service isn't OK";
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadToEnd();
            Reader.Close();

            return responseLine;
        }
        catch (Exception e)
        {
        }
        return "error";
    }

}