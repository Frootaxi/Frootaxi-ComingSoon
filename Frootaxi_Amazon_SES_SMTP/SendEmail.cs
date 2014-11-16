using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using DAL;

namespace Frootaxi_Amazon_SES_SMTP
{
    public class SendEmail
    {
        DataProvider dp = DataProvider.GetInstance();
        List<email> list = new List<email>();
        error_log el = new error_log();
        email em = new email();

        public void Send()
        {
            em = dp.GetEmailMessage();

            if (em != null)
            {
                const String FROM = "support@frootaxi.com";   // Replace with your "From" address. This address must be verified.
                String TO = em.recipient_email_address;  // Replace with a "To" address. If you have not yet requested
                // production access, this address must be verified.

                MailMessage message = new MailMessage(FROM, TO);
                message.Subject = em.subject;
                message.Body = em.message;
                message.IsBodyHtml = true;

                // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
                const String SMTP_USERNAME = "AKIAIQQZ6UYNLMGQUEGQ";  // Replace with your SMTP username credential. 
                const String SMTP_PASSWORD = "Ao3/JPi2dJ5/2fcutZ+l1hiqUAgxDXW0PK8lOLsa1Rpi";  // Replace with your SMTP password.

                // Amazon SES SMTP host name.
                const String HOST = "email-smtp.us-east-1.amazonaws.com";

                // Port we will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
                // STARTTLS to encrypt the connection.
                const int PORT = 587;

                // Create an SMTP client with the specified host name and port.
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
                {
                    // Create a network credential with your SMTP user name and password.
                    client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                    // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then 
                    // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                    client.EnableSsl = true;

                    // Send the email. 
                    try
                    {
                        client.Send(message);

                        em.email_sent = true;
                        em.time_sent = DateTime.Now;
                        dp.UpdateEmailStatus(em);
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Send";
                        el.location_of_source = "SendEmail.cs";
                        el.error_message = ex.Message;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);
                    }
                }
            }
        }

        public void ClearSentEmails()
        {
            try
            {
                list = dp.GetSentEmailMessage();

                foreach (email em in list)
                {
                    dp.ClearAllSentEmails(em);
                }
            }
            catch (Exception ex)
            {
                el.client_id = 0;
                el.source_of_error = "ClearSentEmails";
                el.location_of_source = "SendEmail.cs";
                el.error_message = ex.Message;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
            }
        }
    }
}
