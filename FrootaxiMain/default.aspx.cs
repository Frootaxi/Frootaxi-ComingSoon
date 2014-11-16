using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Threading;
using EngineClass;
using DAL;

namespace FrootaxiMain
{
    public partial class index : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        EmailMessageHandler emh = new EmailMessageHandler();
        EngineClass.Accounts a = new EngineClass.Accounts();
        SMSMessageHandler smh = new SMSMessageHandler();
        Location l = new Location();
        Validator v = new Validator();
        smsHandler sh = new smsHandler();

        client_activation_code cac = new client_activation_code();
        error_log el = new error_log();
        email em = new email();
        user u = new user();
        user driver = new user();
        trip_detail td = new trip_detail();

        string user_id = "";
        string script = "";
        string emailActivationCode = "";
        bool result = false;
        int trip_detail_id = 0;
        string guest = "";
        string _cost = "";
        NameValueCollection userActivationInfo = new NameValueCollection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblErrorMessage.Visible = true;

                //Email verification/activation

                if (Request.QueryString.Count != 0)
                {
                    userActivationInfo = Request.QueryString;
                    VerifyActivationCode();

                    if (!result)
                    {
                        try
                        {
                            em.recipient_email_address = "support@frootaxi.com";
                            em.message = "client with guid " + Request.QueryString[0] + " cannot activate email address. Possible hack.";
                            em.time_entered_in_db = DateTime.Now;
                            em.subject = "client email activation error";
                            em.message_format = "html";

                            result = emh.SendEmailUsingSMTP(em);
                            if (!result)
                                result = emh.SendEmailUsingSMTP(em);
                            if (!result)
                                result = emh.SendEmailUsingSMTP(em);

                            el.client_id = u.client_id;
                            el.source_of_error = "Page_Load";
                            el.location_of_source = "Default.aspx.cs";
                            el.error_message = "User email verification token is not expired but system reading as incorrect. Could also be hacker. Check previous error log entries to verify.";
                            el.notification_message_sent = result;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);
                        }
                        catch
                        { }

                        emg.showErrorMessage("Your verification token is incorrect. An email has been sent to the Frootaxi Support Team. You will receive an email shortly with further instructions.", lblErrorMessage, upErrorMessage, false);
                    }
                    else
                    {
                        emg.showErrorMessage("Your email address has been successfully verified.", lblErrorMessage, upErrorMessage, true);
                    }
                    return;
                }

                if (Session["user"] != null)
                {
                    user_id = Session["user"].ToString();

                    if (user_id.GetType() == typeof(string))
                        u = dp.GetUserByEmailAddress(user_id);
                    else
                        u = dp.GetUserById(int.Parse(user_id));

                    if (u != null)
                        cac = dp.GetClientActivationCodeByUser(u);

                    //Redirect from Account_Activation page

                    if (Session["mobile_activation_status"] != null)
                    {
                        int i = (int)Session["mobile_activation_status"];

                        if (i == 1)
                        {
                            emg.showErrorMessage("Your mobile number has been successfully verified.", lblErrorMessage, upErrorMessage, true);

                            HideRegisterAndToggleSignInLinks();
                            return;
                        }
                    }

                    //Redirect from Login page

                    if (Session["email_activation_status"] != null)
                    {
                        int i = (int)Session["email_activation_status"];

                        if (i == 0)
                        {
                            emg.showErrorMessage("Your account has not been fully activated. Please check your inbox or spam folder for your verification email.", lblErrorMessage, upErrorMessage, false);

                            HideRegisterAndToggleSignInLinks();
                            return;
                        }
                    }

                    //Alternative to Session["email_activation_status"]

                    if (cac != null)
                    {
                        if (cac.activated_email != 1)
                        {
                            emg.showErrorMessage("Your account has not been fully activated. Please check your inbox or spam folder for the email to verify your email address.", lblErrorMessage, upErrorMessage, false);

                            HideRegisterAndToggleSignInLinks();
                        }
                    }
                    else
                    {
                        el.client_id = u.client_id;
                        el.source_of_error = "Page_Load";
                        el.location_of_source = "Default.aspx.cs";
                        el.error_message = "User does not have corresponding CAC entry in db. Is being used to determine activation status to display corresponding message on successful login.";
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        result = false;
                    }

                    HideRegisterAndToggleSignInLinks();
                }

                LoadTimePicker();
                setTime();
            }
        }

        private void LoadTimePicker()
        {
            List<string> hours = new List<string>();
            List<string> minutes = new List<string>();
            List<string> am_pm = new List<string>();

            for (int i = 1; i <= 12; i++)
            {
                hours.Add(i.ToString());
            }

            for (int i = 0; i <= 60; i++)
            {
                if (i < 10)
                    minutes.Add(string.Format("0{0}", i.ToString()));
                else
                    minutes.Add(i.ToString());
            }

            am_pm.Add("AM");
            am_pm.Add("PM");

            ddlHours.DataSource = hours;
            ddlHours.DataBind();

            ddlMinutes.DataSource = minutes;
            ddlMinutes.DataBind();

            ddlAmPmFormat.DataSource = am_pm;
            ddlAmPmFormat.DataBind();
        }

        private void setTime()
        {
            DateTime now = DateTime.Now;

            if (now.Hour > 12) {
                int i = now.Hour - 12;
                ddlHours.Text = i.ToString();               
                ddlAmPmFormat.Text = "PM";
            }
            else if (now.Hour == 12)
            {
                ddlHours.Text = "12";
                ddlAmPmFormat.Text = "PM";
            }
            else if (now.Hour == 0) {
                ddlHours.Text = "12";
                ddlAmPmFormat.Text = "AM";
            }
            else {
                ddlHours.Text = now.Hour.ToString();
                ddlAmPmFormat.Text = "AM";
            }

            if(now.Minute > 10)
                ddlMinutes.Text = now.Minute.ToString();
            else
                ddlMinutes.Text = string.Format("0{0}", now.Minute.ToString());
        }

        private void HideRegisterAndToggleSignInLinks()
        {
            LoginStatus1.LogoutText = "Sign out";
            LoginStatus2.LogoutText = "Sign out";

            script = "document.getElementById('registerLinkTop').style.display = 'none'; document.getElementById('registerLinkBottom').style.display = 'none';";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideRegisterAndToggleSignIn", script, true);
        }

        private void VerifyActivationCode()
        {
            string qrystring = HttpUtility.HtmlDecode(userActivationInfo[0]);
            emailActivationCode = qrystring.Replace("token=","");

            result = v.Validate(emailActivationCode, validationType.guid);

            if (result)
            {
                cac = dp.GetClientActivationCodeByEmailActivationCode(emailActivationCode);

                if (cac != null)
                {
                    if (cac.email_activation_code == emailActivationCode)
                    {
                        UpdateEmailActivationStatus();
                        result = true;
                    }
                    else
                        result = false;
                }
                else
                {
                    em.recipient_email_address = "support@frootaxi.com";
                    em.message = "Possible attack/hack through account activation url with guid " + emailActivationCode;
                    em.time_entered_in_db = DateTime.Now;
                    em.subject = "System hacking - email verification";
                    em.message_format = "html";

                    result = emh.SendEmailUsingSMTP(em);
                    if (!result)
                        result = emh.SendEmailUsingSMTP(em);
                    if (!result)
                        result = emh.SendEmailUsingSMTP(em);

                    el.client_id = u.client_id;
                    el.source_of_error = "Page_Load";
                    el.location_of_source = "Default.aspx.cs";
                    el.error_message = "Possible attack: " + emailActivationCode;
                    el.notification_message_sent = result;
                    el.date_time_stamp = DateTime.Now;
                    dp.AddErrorLog(el);

                    result = false;
                }
            }
            else
            {
                em.recipient_email_address = "support@frootaxi.com";
                em.message = "Possible attack/hack through account activation url with guid" + emailActivationCode;
                em.time_entered_in_db = DateTime.Now;
                em.subject = "System hacking - email verification";
                em.message_format = "html";

                result = emh.SendEmailUsingSMTP(em);
                if (!result)
                    result = emh.SendEmailUsingSMTP(em);
                if (!result)
                    result = emh.SendEmailUsingSMTP(em);

                el.client_id = u.client_id;
                el.source_of_error = "Page_Load";
                el.location_of_source = "Default.aspx.cs";
                el.error_message = "Possible attack: " + emailActivationCode;
                el.notification_message_sent = result;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
            }
        }

        private void UpdateEmailActivationStatus()
        {
            try
            {
                cac.activated_email = 1;
                dp.UpdateClientEmailActivationStatus(cac);

                if (cac.activated_mobile == 1 && cac.activated_email == 1)
                {
                    u = dp.GetUserById(cac.client_id);

                    u.account_activated = true;
                    dp.UpdateUserActivationStatus(u);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (u != null)
                    el.client_id = u.client_id;
                else
                    el.client_id = 0;

                el.source_of_error = "updateActivationStatus";
                el.location_of_source = "Default.aspx.cs";
                el.error_message = "Updating of email activation code and/or overall activation code malfunction. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
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
                    u = dp.GetUserById(cac.client_id);

                    u.account_activated = true;
                    dp.UpdateUserActivationStatus(u);
                    result = true;
                }

            }
            catch (Exception ex)
            {
                if (u != null)
                    el.client_id = u.client_id;
                else
                    el.client_id = 0;

                el.client_id = u.client_id;
                el.source_of_error = "updateActivationStatus";
                el.location_of_source = "Default.aspx.cs";
                el.error_message = "Updating of mobile activation code and/or overall activation code malfunction. Error msg: " + ex;
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
            }
        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Session.Abandon();
        }

        protected void btnHailATaxi_Click(object sender, EventArgs e)
        {
            DateTime rd = new DateTime();
            td = new trip_detail();

            bool result = false;
            result = txtDatePicker_TextChanged(this, e);
            if (!result)
                return;
            else
            {
                rd = DateTime.Parse(string.Format("{0} {1}:{2} {3}", txtDatePicker.Text, ddlHours.SelectedValue, ddlMinutes.SelectedValue, ddlAmPmFormat.SelectedValue));
            }

            if (rd > DateTime.Now)
                td.request_datetimestamp = rd;
            else
            {
                emg.showErrorMessage("Please select a date and time at least 15 - 20 minutes before your pickup time.", txtDatePicker);
                emg.showDialog("unloadDivCover();", upErrorMessage);
                return;
            }

            //GetDriver(td);
            //TODO: A thread will handle the GetDriver(td); function, ensuring the text messages are sent?


            string s = txtTripCoordinates.Value;

            if (s != string.Empty)
            {
                AddTripDetails(s);
                Session["user"] = guest;
            }
            else
            {
                //TODO:Did not receive coordinates. What should happen here?
            }

            LoadTripDetails();
            emg.showDialog("hideWait();loadPopupBox();addUserJourney();", upErrorMessage);
        }

        private void LoadTripDetails()
        {
            string specifier = "GHC 0,0.00";
            decimal d = decimal.Parse(_cost);
            
            txtCost.Text = d.ToString(specifier);

            if (float.Parse(txtAmount.Text) > 1 && rbtOpenTrip.Checked)
                txtDuration.Text = string.Format("{0} Hours", txtAmount.Text);

            if (float.Parse(txtAmount.Text) < 2 && rbtOpenTrip.Checked)
                txtDuration.Text = string.Format("{0} Hour", txtAmount.Text);

            if (rbtOneWay.Checked || rbtRoundTrip.Checked)
                txtDuration.Text = "n/a";

            txtPickTime.Text = string.Format("{0}:{1} {2}", ddlHours.SelectedValue, ddlMinutes.SelectedValue, ddlAmPmFormat.SelectedValue);
        }

        private void GetDriver(trip_detail td)
        {
            string msg = "";

            foreach (user _driver in l.LocateNearestDriver(td))
            {
                driver = dp.GetUserByMobileNumber(_driver.primary_mobile_number);
                smh.SendMessage(driver, msg);
            }
        }

        private void GetUser()
        {
            if (Session["user"] != null)
                u = dp.GetUserByEmailAddress(Session["user"].ToString());
        }

        private void GetTripCost(trip_detail td)
        {
            trip_detail_id = td.id;
            a.CalculateTripCost(td);
            Session.Abandon();
            dp.GetTripDetailsById(trip_detail_id);
        }

        protected void btnMakePayment_Click(object sender, EventArgs e)
        {
            Session.Add("coords", txtTripCoordinates.Value);
            Session.Add("id", txtUuid.Value);

            SendConfirmationTicketToPassenger();

            Response.Redirect("payg.aspx", true);
        }

        private void SendConfirmationTicketToPassenger()
        {
            /*if (driver != null)
                smh.SendMessage();*/
        }

        private void AddTripDetails(string coordinates)
        {
            try
            {
                trip_type tt = new trip_type();

                AddClient();
                AddUser();

                u = new user();
                u = dp.GetUserByEmailAddress(guest);

                if (u != null)
                    td.user_id = u.client_id;

                //GetTripCost(td);
                _cost = txtActualCost.Text;
                td.cost = decimal.Parse(_cost);
                td.trip_coordinates = coordinates;
                td.datetimestamp = DateTime.Now;

                if (rbtOneWay.Checked)
                    txtTripType.Text = "One way trip";
                else if (rbtRoundTrip.Checked)
                    txtTripType.Text = "Round trip";
                else if (rbtOpenTrip.Checked)
                    txtTripType.Text = "Open trip";

                tt = dp.GetTripTypeByName(txtTripType.Text.ToLower());
                if (tt != null)
                    td.trip_type_id = tt.id;

                if (txtAmount.Text != "")
                    td.trip_duration = int.Parse(txtAmount.Text);
            }
            catch(Exception ex)
            {

            }

            dp.AddTripDetails(td);
        }

        private void AddClient()
        {
            client c = new client();

            guest = txtUuid.Value;
            c.first_name = guest;
            c.date_time_stamp = DateTime.Now;

            dp.AddNewClient(c);
        }

        private void AddUser()
        {
            user u = new user();
            role r = new role();
            client c = new client();

            r = dp.GetRoleByName("guest");

            if (r != null)
                u.role_id = r.id;

            u.primary_mobile_number = "0000000000";
            u.email_address = guest;
            u.account_activated = false;

            c = dp.GetClientByFirstName(guest);

            if (c != null)
                u.client_id = c.id;

            dp.AddNewUser(u);
        }

        public class triptype {
            public int id { get; set; }

            public string name { get; set; }
        }

        protected bool txtDatePicker_TextChanged(object sender, EventArgs e)
        {
            bool result = false;
            bool vResult = false;
            DateTime d = new DateTime();
            string newFormat = "";

            try
            {
                d = DateTime.Parse(txtDatePicker.Text);
                newFormat = d.ToString("yyyy-MM-dd");
                emg.hideErrorMessage(txtDatePicker);
            }
            catch (Exception ex)
            {
                emg.showErrorMessage("Please enter a valid date by clicking on the calendar to the right.", txtDatePicker);
                emg.loadScript("unloadDivCover();", upErrorMessage);
                result = false;
            }

            vResult = v.Validate(newFormat.ToString(), validationType.date);
            if (!vResult)
            {
                emg.showErrorMessage("Please enter a valid date by clicking on the calendar to the right.", txtDatePicker);
                emg.loadScript("unloadDivCover();", upErrorMessage);
                result = false;
            }
            else
            {
                emg.hideErrorMessage(txtDatePicker);
                result = true;
            }

            return result;
        }
    }
}