using System;
using System.Text;
using System.Web;
using System.Globalization;
using DAL;

namespace EngineClass
{
    public class EmailMessageHandler
    {
        DataProvider dp = DataProvider.GetInstance();
        error_log el = new error_log();
        email e = new email();
        user u = new user();

        StringBuilder emailMessage = new StringBuilder();
        string encodedUrl = "";
        bool result = false;

        public bool SendEmailUsingDBMail(email em)
        {
            try
            {
                if (em != null)
                {
                    AddEmailRecordToDB(em);

                    dp.SendActivationEmail(e);

                    result = true;
                    
                }
            }
            catch (Exception ex)
            {
                u = dp.GetUserByEmailAddress(em.recipient_email_address);

                el.client_id = u.client_id;
                el.source_of_error = "SendEmailUsingDBMail";
                el.location_of_source = "EmailMessageHandler.cs";
                el.error_message = "Error sending email. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
                
                result = false;
            }
            return result;
        }

        private void AddEmailRecordToDB(email em)
        {
            e.recipient_email_address = em.recipient_email_address;
            e.subject = em.subject;
            e.message = em.message;
            e.message_format = em.message_format;
            e.time_entered_in_db = DateTime.Now;
            e.email_sent = false;

            dp.AddEmail(e);
        }

        public bool SendEmailUsingSMTP(email em)
        {
            try
            {
                if (em != null)
                {
                    AddEmailRecordToDB(em);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                u = dp.GetUserByEmailAddress(em.recipient_email_address);

                el.client_id = u.client_id;
                el.source_of_error = "SendEmailUsingSMTP";
                el.location_of_source = "EmailMessageHandler.cs";
                el.error_message = "Error sending email. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
                
                result = false;
            }
            return result;
        }

        public StringBuilder CreateAccountActivationEmailBody(client_activation_code cac)
        {
            emailMessage.Append("<br />");
            emailMessage.Append("Thank you for creating an account with Frootaxi.com");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("Please click the link below to verify your account");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            encodedUrl = string.Format("token={0}", cac.email_activation_code);
            encodedUrl = HttpUtility.UrlEncode(encodedUrl);

            emailMessage.Append(string.Format("<a href='https://beta.frootaxi.com/Default.aspx?{0}'>Verify my email address </a>", encodedUrl));
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("Your email address must be verified to receive receipts of your payment transactions. We suggest adding us to your address book to ensure our emails reach your inbox and not your spam folder.");
            emailMessage.Append("<br />");
            emailMessage.Append("If you have any issues, feel free to drop us a line at support@frootaxi.com or simply reply to this email.");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("Bon Voyage,");
            emailMessage.Append("<br />");
            emailMessage.Append("Frootaxi Support Team");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<div style='background-color:#999; height:100px; min-width:100%; margin: 0 auto; color:#333; padding:10px 0.5%; font-size:14px; font-family:Calibri'>Frootaxi Service <br /><span style='font-size:10px'>P.O. Box CT 9769, <br />Cantonments, Accra, <br />Ghana<br />Tel: +233 (50) 893 9700</span></div>");

            return emailMessage;
        }

        public StringBuilder CreateForgotPasswordEmailBody(user u, password_reset pr)
        {
            string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dp.GetClientByUserId(u.client_id).first_name);
            emailMessage.Append("<br />"); 
            emailMessage.Append("<span style='font-size:22px; font-weight:bold; color:#cdac27'>" + name + "</span>");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("We have been informed that you need to change your password.");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("Please do so by clicking the link below");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            encodedUrl = string.Format("token={0}", pr.password_reset_token);
            encodedUrl = HttpUtility.UrlEncode(encodedUrl);

            emailMessage.Append(string.Format("<a href='https://beta.frootaxi.com/Accounts/New_Password.aspx?{0}'>Reset my password </a>", encodedUrl));
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("The above link is valid for the next 24 hours. ");
            emailMessage.Append("If you did not make this request, please ignore this email and no changes will be made to your account.");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("If you have any issues, feel free to drop us a line at support@frootaxi.com or simply reply to this email.");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("Thank you,<br />");
            emailMessage.Append("Frootaxi Support Team");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<br />");
            emailMessage.Append("<div style='background-color:#999; height:100px; min-width:100%; margin: 0 auto; color:#333; padding:10px 0.5%; font-size:14px; font-family:Calibri'>Frootaxi Service <br /><span style='font-size:10px'>P.O. Box CT 9769, <br />Cantonments, Accra, <br />Ghana<br />Tel: +233 (50) 893 9700</span></div>");

            return emailMessage;
        }
    }
}
