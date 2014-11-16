using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using EngineClass;
using DAL;

namespace FrootaxiMain.Accounts
{
    public partial class New_Password : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        static password_reset pr = new password_reset();
        error_log el = new error_log();
        email em = new email();
        user u = new user();

        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        EmailMessageHandler emh = new EmailMessageHandler();
        Validator v = new Validator();

        NameValueCollection passwordResetToken = new NameValueCollection();

        string pswd, cpswd, pt = "";
        bool result, passwordMatch = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    passwordResetToken = Request.QueryString;

                    VerifyActivationCode();

                    if (pr == null)
                        Response.Redirect("https://beta.frootaxi.com/Accounts/Forgot_Password.aspx", true);
                        //Response.Redirect("Forgot_Password.aspx", true);

                    if(!result)
                        Response.Redirect("https://beta.frootaxi.com/Default.aspx", true);
                        //Response.Redirect("Default.aspx", true);
                }
                else
                    Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
                    //Response.Redirect("Login.aspx", true);
            }
            pswd = txtPassword.Text;
            cpswd = txtConfirmPassword.Text;*/
        }

        protected void btnNewPassword_Click(object sender, EventArgs e)
        {
            txtPassword_TextChanged(this, e);
            txtConfirmPassword_TextChanged(this, e);

            if (passwordMatch)
            {
                try
                {
                    int i = (int)Session["user"];
                    u = dp.GetUserById(i);
                    u.password = PasswordHash.CreateHash(pswd);

                    dp.UpdateUserPassword(u);

                    Session.Add("password_reset", 1);
                }
                catch
                {
                    el.client_id = pr.client_id;
                    el.source_of_error = "VerifyActivationCode";
                    el.location_of_source = "New_Password.aspx.cs";
                    el.error_message = "User cannot reset password. Either user does not exist in db or password update function throwing error.";
                    el.notification_message_sent = false;
                    el.date_time_stamp = DateTime.Now;
                    dp.AddErrorLog(el);
                    Session.Add("password_reset", 0);
                    Response.Redirect("https://beta.frootaxi.com/Accounts/Forgot_Password.aspx", true);
                    //Response.Redirect("Forgot_Password.aspx", true);
                }
                Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
                //Response.Redirect("Login.aspx", true);
            }
            else
            {
                emg.showErrorMessage("Your passwords do not match", txtConfirmPassword);
                txtPassword.Attributes.Add("value", pswd);
                txtConfirmPassword.Attributes.Add("value", cpswd);
                txtConfirmPassword.Focus();
                return;
            }
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {
            CheckEntriesForEmptyString(pswd, txtPassword);
            if (!result)
                return;

            result = v.Validate(pswd, validationType.password);

            if (!result)
            {
                emg.showErrorMessage("Your password must be between 6 and 10 characters and contain at least one digit", txtPassword);
                txtPassword.Attributes.Add("value", pswd);
                txtPassword.Focus();
            }
            else
            {
                emg.hideErrorMessage(txtPassword);
                txtPassword.Attributes.Add("value", pswd);
                txtConfirmPassword.Focus();
            }
        }

        protected void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            CheckEntriesForEmptyString(cpswd, txtConfirmPassword);
            if (!result)
                return;

            if (result && !(cpswd == pswd))
            {
                emg.showErrorMessage("Your passwords do not match", txtConfirmPassword);
                txtConfirmPassword.Attributes.Add("value", cpswd);
                txtConfirmPassword.Focus();
            }
            else
            {
                emg.hideErrorMessage(txtConfirmPassword);
                passwordMatch = true;
                txtConfirmPassword.Attributes.Add("value", cpswd);
            }
        }

        private void CheckEntriesForEmptyString(string textbox, TextBox t)
        {
            if (textbox == "")
            {
                emg.showErrorMessage("This is a required field", t);
                t.Focus();
                result = false;
            }
            else
                result = true;
        }

        private void VerifyActivationCode()
        {
            string qrystring = HttpUtility.HtmlDecode(passwordResetToken[0]);
            pt = qrystring.Replace("token=", "");

            result = v.Validate(pt, validationType.guid);

            if (!result)
            {
                em.recipient_email_address = "support@frootaxi.com";
                em.message = "Possible attack/hack through new password request with password token" + pt;
                em.time_entered_in_db = DateTime.Now;
                em.subject = "System hacking - new password request";
                em.message_format = "html";
                em.email_sent = false;

                result = emh.SendEmailUsingSMTP(em);
                if (!result)
                    result = emh.SendEmailUsingSMTP(em);
                if (!result)
                    result = emh.SendEmailUsingSMTP(em);

                el.client_id = u.client_id;
                el.source_of_error = "Page_Load";
                el.location_of_source = "New_Password.aspx.cs";
                el.error_message = "Possible attack: " + pt;
                el.notification_message_sent = result;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
                return;
            }

            pr = dp.GetPasswordResetInfoByToken(pt);

            if (pr != null)
            {
                DateTime ed = (DateTime)pr.expiration_date;
                TimeSpan dif = DateTime.Now - ed;

                if (dif.Days >= 1)
                {
                    dp.DeleteExpiredPasswordResetToken(pr);
                    Response.Redirect("https://beta.frootaxi.com/Accounts/Expired_Password_Token.aspx", true);
                    //Response.Redirect("Expired_Password_Token.aspx", true);
                }

                Session.Add("user", pr.client_id);
                dp.DeleteExpiredPasswordResetToken(pr);
            }
            else
            {
                el.client_id = 0;
                el.source_of_error = "VerifyActivationCode";
                el.location_of_source = "New_Password.aspx.cs";
                el.error_message = "User password reset token invalid, could not retrieve password_reset data record. Token id:" + pt;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
                
                Session.Add("password_reset", 0);
            }
        }
    }
}