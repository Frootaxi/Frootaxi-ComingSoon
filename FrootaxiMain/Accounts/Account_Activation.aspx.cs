using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EngineClass;
using DAL;

namespace FrootaxiMain.Accounts
{
    public partial class Account_Activation : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        SMSMessageHandler smh = new SMSMessageHandler();
        Registration reg = new Registration();

        client_activation_code cac = new client_activation_code();
        client_activation_code ncac = new client_activation_code();
        error_log el = new error_log();
        client c = new client();
        static user u = new user();
        int client_id = 0;
        string activation_code = "";
        bool result = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (!IsPostBack)
            {
                lblActivateAccount.Visible = false;

                if (Session["user"] != null)
                {
                    try
                    {
                        client_id = (int)Session["user"];
                        u = dp.GetUserById(client_id);

                        if (u == null)
                        {
                            el.client_id = 0;
                            el.source_of_error = "Page_Load";
                            el.location_of_source = "Account_Activation.aspx.cs";
                            el.error_message = "Client id successfully added in Register.aspx.cs but does not exist in database after redirect.";
                            el.notification_message_sent = false;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);

                            //Response.Redirect("Login.aspx", true);
                            Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Page_Load";
                        el.location_of_source = "Account_Activation.aspx.cs";
                        el.error_message = "Client id not successfully added in Register.aspx.cs so session('user') is null. Error msg: " + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        //Response.Redirect("Login.aspx", true);
                        Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
                    }
                }
            }
            
            txtActivationCode.Focus();

            
            if(Session["user"] == null)
            {
                //Response.Redirect("Login.aspx", true);
                Response.Redirect("https://beta.frootaxi.com/Accounts/Login.aspx", true);
            } 
            
            activation_code = txtActivationCode.Text;*/
        }

        protected void btnActivateAccount_Click(object sender, EventArgs e)
        {
            cac = dp.GetClientActivationCodeByUser(u);

            if (cac != null)
                ncac = dp.GetClientActivationCodeByMobileActivationCode(activation_code);
            else
            {
                el.client_id = u.client_id;
                el.source_of_error = "btnActivateAccount_Click";
                el.location_of_source = "Account_Activation.aspx.cs";
                el.error_message = "Activation codes were not generated for user";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                reg.GenerateEmailActivationGuid(u);
                reg.GenerateMobileActivationCode(u);
                reg.AddActivationCodes();

                smh.SendMobileActivationCode(u);

                emg.showErrorMessage("Your activation code is incorrect. Check your SMS messages for your activation code.", txtActivationCode);
            }

            if (ncac != null)
            {
                if (cac == ncac)
                {
                    updateActivationStatus();
                    if (!result)
                    {
                        emg.showErrorMessage("Your account cannot be activated at this time. Try again later", lblActivateAccount, upActivateAccount, false);
                    }
                    else
                    {
                        Session.Add("user", u.client_id);
                        Session.Add("mobile_activation_status", 1);
                        switch (u.role_id)
                        {
                            case 1:
                                //Response.Redirect("../Default.aspx", true);
                                Response.Redirect("https://beta.frootaxi.com/Default.aspx", true);
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                    }
                }
                else
                {
                    emg.showErrorMessage("Your activation code is incorrect. If you have not received the SMS with your activation code, request for another one by clicking the 'Send activation code again' button below", txtActivationCode);
                    txtActivationCode.Text = "";
                }
            }
            else
            {
                emg.showErrorMessage("Your activation code is incorrect. If you have not received the SMS with your activation code, request for another one by clicking the 'Send activation code again' button below", txtActivationCode);
                txtActivationCode.Text = "";
            }
        }

        private void updateActivationStatus()
        {
            try
            {
                cac.activated_mobile = 1;
                dp.UpdateClientMobileActivationStatus(cac);

                if (cac.activated_mobile == 1 && cac.activated_email == 1)
                {
                    u.account_activated = true;
                    dp.UpdateUserActivationStatus(u);
                }
                result = true;
                
            }
            catch (Exception ex)
            {
                el.client_id = u.client_id;
                el.source_of_error = "btnActivateAccount_Click";
                el.location_of_source = "Account_Activation.aspx.cs";
                el.error_message = "Updating of mobile activation code and/or overall activation code malfunction. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
            }
        }

        protected void btnResendActivationCode_Click(object sender, EventArgs e)
        {
            cac = dp.GetClientActivationCodeByUser(u);

            if (cac.client_id != 0)
            {
                reg.ResendMobileActivationCode(u);
            }
            else
            {
                el.client_id = u.client_id;
                el.source_of_error = "btnResendActivationCode_Click";
                el.location_of_source = "Account_Activation.aspx.cs";
                el.error_message = "Activation codes were not generated for user u";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                reg.GenerateEmailActivationGuid(u);
                reg.GenerateMobileActivationCode(u);
                reg.AddActivationCodes();
            }
            smh.SendMobileActivationCode(u);

            emg.hideErrorMessage(txtActivationCode);
            txtActivationCode.Text = "";
            emg.showErrorMessage("An activation code has been sent to your mobile number.", lblActivateAccount, upActivateAccount, true);
        }
    }
}