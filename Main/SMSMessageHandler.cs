using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;


namespace EngineClass
{
    public class SMSMessageHandler
    {
        static DataProvider dp = DataProvider.GetInstance();
        client_activation_code cac = new client_activation_code();
        error_log el = new error_log();

        DateTime date = new DateTime();
        string mobile_number = "";
        string str = "";
        string msg = "";

        public void SendMobileActivationCode(user u)
        {
            if (u != null)
                cac = dp.GetClientActivationCodeByUser(u);
            else
                return;

            if (cac != null)
            {
                msg = "Your activation code is " + cac.mobile_activation_code;
                date = DateTime.Now;

                SendMessage(u, msg);
            }
        }

        public string SendMessage(user u, string message)
        {
            try
            {
                mobile_number = HttpUtility.UrlEncode(u.primary_mobile_number);
                message = HttpUtility.UrlEncode(message);

                HttpWebRequest getrequest = (HttpWebRequest)WebRequest.Create(("http://site.mytxtbox.com/sms_api?username=frootaxisms&password=frootaxisms&msg=" + message + "&to=" + mobile_number + "'"));
                HttpWebResponse getresponse = (HttpWebResponse)getrequest.GetResponse();

                StreamReader reader = new StreamReader(getresponse.GetResponseStream(), Encoding.UTF8);
                str = reader.ReadToEnd();
                str.ToString();

                
            }
            catch (Exception ex)
            {
                el.client_id = u.client_id;
                el.source_of_error = "SendMessage";
                el.location_of_source = "SMSMessageHandler.cs";
                el.error_message = "Error sending SMS message through SMSGH gateway. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
            }
            return str;
        }

        
    }
}
