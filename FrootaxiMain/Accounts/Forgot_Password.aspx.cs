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
    public partial class Forgot_Password : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        List<password_reset> list = new List<password_reset>();
        password_reset pr = new password_reset();
        error_log el = new error_log();
        email em = new email();
        user u = new user();

        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        EmailMessageHandler emh = new EmailMessageHandler();
        Validator v = new Validator();

        Guid pswdResetToken = new Guid();
        StringBuilder sb = new StringBuilder();
        string emailAdd = "";
        bool result = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["password_reset"] != null)
            {
                emg.showErrorMessage("Your password could not be reset. Please request a new link and try again.", lblErrorMessage, upErrorMessage, false);
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (txtEmailAddress.Text != string.Empty)
            {
                emailAdd = txtEmailAddress.Text;
                result = v.Validate(emailAdd, validationType.email);

                if (!result)
                {
                    emg.showErrorMessage("Invalid email address. Please check your entry and try again.", txtEmailAddress);
                    txtEmailAddress.Focus();
                    return;
                }

                u = dp.GetUserByEmail(emailAdd);

                if (u != null)
                {
                    DeleteAnyExistingPasswordResetTokens();
                    
                    CreatePasswordResetToken();
                    
                    sb = emh.CreateForgotPasswordEmailBody(u, pr);
                    
                    em.recipient_email_address = emailAdd;
                    em.message = sb.ToString();
                    em.subject = "Frootaxi Account Password Reset";
                    em.message_format = "HTML";
                    em.email_sent = false;

                    emh.SendEmailUsingSMTP(em);

                    Session.Add("reset_email_sent", 1);
                    Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
                }
                else
                {
                    emg.showErrorMessage("This email address is not registered. Please check your entry and try again.", txtEmailAddress);
                    txtEmailAddress.Focus();
                    return;
                }
            }
        }

        private void CreatePasswordResetToken()
        {
            pr.client_id = u.client_id;
            pr.expiration_date = DateTime.Now.AddHours(24);

            pswdResetToken = Guid.NewGuid();
            pr.password_reset_token = pswdResetToken.ToString();

            dp.AddPasswordReset(pr);
        }

        private void DeleteAnyExistingPasswordResetTokens()
        {
            list = dp.GetPasswordResetInfoByUser(u);
            if (list.Count > 0)
            {
                foreach (password_reset existing_pr in list)
                {
                    dp.DeleteExpiredPasswordResetToken(existing_pr);
                }
            }
        }
    }
}