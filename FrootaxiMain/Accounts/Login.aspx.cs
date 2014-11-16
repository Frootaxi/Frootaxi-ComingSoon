using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EngineClass;
using DAL;

namespace FrootaxiMain.Accounts
{
    public partial class Login : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        SMSMessageHandler smh = new SMSMessageHandler();
        EmailMessageHandler emh = new EmailMessageHandler();
        Registration r = new Registration();
        client_activation_code cac = new client_activation_code();
        List<password_reset> list = new List<password_reset>();
        error_log el = new error_log();
        StringBuilder emailMessage = new StringBuilder();
        client c = new client();
        email e = new email();
        user u = new user();
        string username = "";
        string password = "";
        bool EmailNotActivated = false;
        bool result = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblLoginError.Visible = false;

                if (Session.Count > 0)
                {
                    if (Session["reset_email_sent"] != null)
                    {
                        btnLogin.Enabled = false;
                        emg.showErrorMessage("Your new password request has been sent. Follow the instructions in the email to reset your password", lblLoginError, upLogin, true);
                    }

                    if (Session["password_reset"] != null)
                    {
                        int i = (int)Session["password_reset"];
                        if (i == 1)
                            emg.showErrorMessage("Your password has been successfully reset. You can now login", lblLoginError, upLogin, true);
                        else
                            emg.showErrorMessage("Your password could not be reset. Please request a new link and try again.", lblLoginError, upLogin, true);
                    }

                    if (Session["redirectMessage"] != null)
                    {
                        emg.showErrorMessage("Please login to complete your taxi request.", lblLoginError, upLogin, true);
                        return;
                    }
                }
            }

            username = txtUsername.Text;
            password = txtPassword.Text;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(username);
                u = dp.GetUserByMobileNumber(username);
            }
            catch (Exception ex)
            {
                u = dp.GetUserByEmailAddress(username);
            }

            if (u != null)
                result = PasswordHash.ValidatePassword(password, u.password);
            else
            {
                DisplayLoginError();
                return;
            }

            if (!result)
            {
                DisplayLoginError();
            }
            else
            {
                DeleteAnyExistingPasswordResetTokens();

                Session.Add("user", u.client_id);

                CheckClientActivationCodeInformation();
                if (!result)
                {
                    Session.Add("mobile_activation_status", 0);
                    Response.Redirect("https://beta.frootaxi.com/Accounts/Account_Activation.aspx", true);
                    //Response.Redirect("Account_Activation.aspx", true);
                }
                else
                {
                    if (EmailNotActivated)
                        Session.Add("email_activation_status", 0);

                    RedirectSwitch();
                }

                String s = Request.QueryString["returnurl"];

                if (s != null)
                    Response.Redirect(s);
            }
        }

        private void DeleteAnyExistingPasswordResetTokens()
        {
            list = dp.GetPasswordResetInfoByUser(u);

            if (list.Count > 0)
            {
                foreach (password_reset pr in list)
                {
                    dp.DeleteExpiredPasswordResetToken(pr);
                }

            }
        }

        private void RedirectSwitch()
        {
            //TODO: Switch statement to determine which home page the user sees based on their role
            switch (u.role_id)
            {
                case 1:
                    //Response.Redirect("https://beta.frootaxi.com/Default.aspx", true);
                    Response.Redirect("../Default.aspx", true);
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;
            }
        }

        private void DisplayLoginError()
        {
            emg.showErrorMessage("Login failed. Your username or password is incorrect.", lblLoginError, upLogin, false);
            txtPassword.Attributes.Add("value", password);
            txtUsername.Focus();
        }

        private void CheckClientActivationCodeInformation()
        {

            if (u != null)
                cac = dp.GetClientActivationCodeByUser(u);

            if (!(bool)u.account_activated)
            {
                if (cac != null)
                {
                    if (cac.activated_mobile == 0)
                    {
                        r.ResendMobileActivationCode(u);
                        result = false;
                    }

                    if (cac.activated_email == 0)
                    {
                        EmailNotActivated = true;
                        result = true;
                    }
                }
                else
                {
                    el.client_id = u.client_id;
                    el.source_of_error = "CheckClientActivationCodeInformation";
                    el.location_of_source = "Login.aspx.cs";
                    el.error_message = "activation codes not generated for user u when registering. all resent.";
                    el.notification_message_sent = false;
                    el.date_time_stamp = DateTime.Now;
                    dp.AddErrorLog(el);

                    r.GenerateMobileActivationCode(u);
                    r.GenerateEmailActivationGuid(u);
                    r.AddActivationCodes();

                    cac = dp.GetClientActivationCode(u);
                    if (cac != null)
                    {
                        smh.SendMobileActivationCode(u);

                        e.recipient_email_address = u.email_address;
                        e.subject = "Frootaxi Account Verification";

                        emailMessage = emh.CreateAccountActivationEmailBody(cac);
                        e.message = emailMessage.ToString();

                        e.message_format = "HTML";
                        emh.SendEmailUsingSMTP(e);
                        result = true;
                    }
                    else
                    {
                        el.client_id = u.client_id;
                        el.source_of_error = "CheckClientActivationCodeInformation";
                        el.location_of_source = "Login.aspx.cs";
                        el.error_message = "activation codes not generated for client on second attempt during login";
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);
                    }
                    result = false;
                }
            }
            else
                result = true;
        }
    }
}