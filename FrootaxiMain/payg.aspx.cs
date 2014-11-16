using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using EngineClass;

namespace FrootaxiMain
{
    public partial class payg : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        EmailMessageHandler emh = new EmailMessageHandler();
        Registration reg = new Registration();
        Validator v = new Validator();
        Location l = new Location();

        payment_account_type paymentType = new payment_account_type();
        client_activation_code cac = new client_activation_code();
        SMSMessageHandler smh = new SMSMessageHandler();
        client_location cl = new client_location();
        trip_detail td = new trip_detail();
        client cresult = new client();
        error_log el = new error_log();
        user mresult = new user();
        user eresult = new user();
        user guest = new user();
        email e = new email();
        user u = new user();
        
        string user_id, email, mobileNum, pswd, cpswd = "";
        StringBuilder emailMessage = new StringBuilder();
        bool result = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user"] != null)
                {
                    user_id = Session["user"].ToString();
                    guest = dp.GetUserByEmail(user_id);
                    td = dp.GetTripDetailsByUser(guest);

                    LoadUserJourneyCoordinates();
                    LoadTripDetails();
                }
            }
        }

        private void LoadTripDetails()
        {
            if (td != null)
            {
                txtCost.Text = ((decimal)td.cost).ToString();
                txtTripType.Text = td.trip_type.name.ToString();

                string duration = td.trip_duration.ToString();
                if (duration == "")
                    txtDuration.Text = "n/a";
                else
                    txtDuration.Text = duration;

                DateTime pt = (DateTime)td.request_datetimestamp;

                txtPickTime.Text = pt.ToString("HH:mm tt");
            }
        }

        private void LoadUserJourneyCoordinates()
        {
            if (td != null)
                txtTripCoordinates.Value = td.trip_coordinates;
        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Session.Abandon();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ValidateEmailAddress();
            if (result)
            {
                guest.email_address = txtEmailAddress.Text;
                guest.primary_mobile_number = txtMobileNumber.Text;
                guest.password = txtPassword.Text;
                guest.role_id = 1;
                guest.account_activated = false;

                UpdateClientInformation();
                if (!result)
                    return;

                SendActivationCodes();
                if (!result)
                    return;
            }
            else
                return;

            dp.AddUser(guest);

            Response.Redirect("my_trip.aspx", true);
        }

        private void UpdateClientInformation()
        {
            string client_id = Session["id"].ToString();
            string loc = Session["coords"].ToString();
            int location_id, payment_account_id = 0;


            List<string> list = new List<string>();
            list = GetCustomerLocation(loc);
            if (result)
            {
                cl.gps_coordinate = list[0];
                dp.AddClientLocation(cl);
            }
            else
            {
                emg.showErrorMessage("An error occured while creating your account. Please refresh this page and reenter your information. If the isssue continues contact support@frootaxi.com.", lblErrorMessage, upErrorMessage, false);
                return;
            }

            location_id = dp.GetClientLocationByKey(cl).id;

            string pt = GetSelectedPaymentType();
            if (pt != null)
            {
                paymentType = dp.GetPaymentTypeByName(pt);

                if (paymentType.type == "mpower")
                {
                    payment_account pa = new payment_account();
                    pa.type_id = paymentType.id;
                    pa.account_number = txtMPowerAccountNumber.Text;

                    dp.AddPaymentAccount(pa);
                }
                
                payment_account_id = paymentType.id;
            }
            else
            {
                emg.showErrorMessage("Please select a payment type.", lblErrorMessage, upErrorMessage, false);
                return;
            }

            cresult = dp.GetClientByFirstName(client_id);
            if (cresult != null)
            {
                cresult.location_id = location_id;
                cresult.payment_account_id = payment_account_id;

                dp.UpdateClient(cresult);

                result = true;
            }
            else
            {
                result = false;

                el.client_id = 0;
                el.source_of_error = "UpdateClientInformation";
                el.location_of_source = "Payg.aspx.cs";
                el.error_message = "Client was not created on Default page when request was being made." + " Client id: " + client_id;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                emg.showErrorMessage("An error occured while processing your request. Please try again in a while.", lblErrorMessage, upErrorMessage, false);
            }
        }

        private string GetSelectedPaymentType()
        {
            string s = "";

            if (rbtnCash.Checked)
                s = "cash";
            if (rbtnMPower.Checked)
                s = "mpower";
            if (rbtnExpressPay.Checked)
                s = "expresspaygh";

            return s;
        }

        private List<string> GetCustomerLocation(string loc)
        {
            List<string> list = new List<string>();

            list = l.SplitCoordinates(txtTripCoordinates.Value);

            return list;
        }

        private void ValidateEmailAddress()
        {
            email = txtEmailAddress.Text;

            CheckEntriesForEmptyString(email, txtEmailAddress);
            if (!result)
                return;

            result = v.Validate(email, validationType.email);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid email address", txtEmailAddress);
                txtEmailAddress.Focus();
                return;
            }
            else
            {
                eresult = dp.GetUserByEmail(email);
                if (eresult != null)
                {
                    Session.Add("redirectMessage", 1);
                    Response.Redirect("Accounts/Login.aspx?returnurl=payg.aspx");
                }
                else
                {
                    emg.hideErrorMessage(txtEmailAddress);
                    result = true;
                }
            }
        }

        private void ValidateMobileNumber()
        {
            mobileNum = txtMobileNumber.Text;

            CheckEntriesForEmptyString(mobileNum, txtMobileNumber);
            if (!result)
                return;

            result = v.Validate(mobileNum, validationType.phoneNum);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid mobile number. Do not add +233 to the beginning of your mobile number", txtMobileNumber);
                txtMobileNumber.Focus();
                return;
            }
            else
            {
                mresult = dp.GetUserByMobileNumber(mobileNum);
                if (mresult != null)
                {
                    Session.Add("redirectMessage", 1);
                    Response.Redirect("Accounts/Login.aspx?returnurl=payg.aspx");
                }
                else
                {
                    emg.hideErrorMessage(txtMobileNumber);
                    result = true;
                }
            }

        }

        private void ValidatePassword()
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
                return;
            }
            else
            {
                emg.hideErrorMessage(txtPassword);
                txtPassword.Attributes.Add("value", pswd);
            }
        }

        private void ValidateConfirmPassword()
        {
            CheckEntriesForEmptyString(cpswd, txtConfirmPassword);
            if (!result)
                return;

            if (result && !(cpswd == pswd))
            {
                emg.showErrorMessage("Your passwords do not match", txtConfirmPassword);
                txtConfirmPassword.Attributes.Add("value", cpswd);
                txtConfirmPassword.Focus();
                return;
            }
            else
            {
                emg.hideErrorMessage(txtConfirmPassword);
                txtConfirmPassword.Attributes.Add("value", cpswd);
            }

        }

        private void SendActivationCodes()
        {
            u = dp.GetUserById(int.Parse(user_id));

            if (u != null)
            {
                reg.GenerateMobileActivationCode(u);
                reg.GenerateEmailActivationGuid(u);
                reg.AddActivationCodes();
                result = true;
            }
            else
            {
                el.client_id = u.client_id;
                el.source_of_error = "SendActivationCodes";
                el.location_of_source = "Payg.aspx.cs";
                el.error_message = "While registering client, user object contains null";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
            }

            cac = dp.GetClientActivationCode(u);
            if (cac != null)
            {
                result = true;

                smh.SendMobileActivationCode(u);

                e.recipient_email_address = u.email_address;
                e.subject = "Frootaxi Account Verification";

                emailMessage = emh.CreateAccountActivationEmailBody(cac);
                e.message = emailMessage.ToString();

                e.message_format = "HTML";
                e.email_sent = false;
                emh.SendEmailUsingSMTP(e);
            }
            else
            {
                result = false;

                el.client_id = u.client_id;
                el.source_of_error = "SendActivationCodes";
                el.location_of_source = "Register.aspx.cs";
                el.error_message = "activation codes not generated for user u when registering.";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);
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
    }
}