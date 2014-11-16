using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EngineClass;
using DAL;

namespace FrootaxiMain.Accounts
{
    public partial class Register : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        SMSMessageHandler smh = new SMSMessageHandler();
        EmailMessageHandler emh = new EmailMessageHandler();
        Registration reg = new Registration();
        Validator v = new Validator();

        payment_account paymentAcct = new payment_account();
        payment_account paresult = new payment_account();
        List<payment_account_type> p = new List<payment_account_type>();
        List<package> pa = new List<package>();
        payment_account_type paymentType = new payment_account_type();
        client_activation_code cac = new client_activation_code();
        client_location clresult, cl = new client_location();
        List<string> listOfMonths = new List<string>();
        StringBuilder emailMessage = new StringBuilder();
        promotion_code pCodeResult = new promotion_code();
        List<string> listOfYears = new List<string>();
        List<int> listOfdays = new List<int>();
        List<region> r = new List<region>();
        email em = new email();
        error_log el = new error_log();
        DateTime date = new DateTime();
        email e = new email();
        client cresult = new client();
        client c = new client();
        user mresult = new user();
        user eresult = new user();
        user u = new user();
        string mobileNum, acctNum, promoCode, pt, pat = "";
        string fname, lname, email, cpswd, pswd, mth, yr = "";
        int payment_type_id, region_id = 0;
        bool result = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadRegions();
                loadPaymentTypes();
                loadPackages();
                //loadYears();

                txtFirstName.Focus();
            }

            fname = txtFirstName.Text.ToLower().Trim();
            lname = txtLastName.Text.ToLower().Trim();
            email = txtEmailAddress.Text;
            mobileNum = txtMobileNumber.Text;
            pswd = txtPassword.Text;
            cpswd = txtConfirmPassword.Text;
            //mth = ddlMonth.SelectedValue.ToString();
            //yr = ddlYear.SelectedValue.ToString();
            region_id = int.Parse(ddlRegion.SelectedValue);
            payment_type_id = int.Parse(ddlPaymentType.SelectedValue);
            pat = ddlPaymentPackage.SelectedItem.ToString().ToLower();
            pt = ddlPaymentType.SelectedItem.ToString().ToLower();
            acctNum = txtAccountNumber.Text;
            promoCode = txtPromotionCode.Text;

        }

        private void loadRegions()
        {
            List<region> list = new List<region>();
            region reg = new region();

            r = dp.ListRegions();
            reg.name = "--Region--";
            reg.id = 0;


            list.Add(reg);

            foreach (region x in r)
            {
                list.Add(x);
            }

            ddlRegion.DataSource = list;
            ddlRegion.DataBind();

            ddlRegion.SelectedValue = reg.name;
        }

        private void loadPaymentTypes()
        {
            List<payment_account_type> list = new List<payment_account_type>();
            payment_account_type pay = new payment_account_type();

            p = dp.ListPaymentTypes();
            pay.type = "--Payment Type--";
            pay.id = 0;

            list.Add(pay);

            foreach (payment_account_type x in p)
            {
                list.Add(x);
            }

            ddlPaymentType.DataSource = list;
            ddlPaymentType.DataBind();

            ddlPaymentType.SelectedValue = pay.type;
        }

        private void loadPackages()
        {
            List<package> list = new List<package>();
            package pack = new package();

            pa = dp.ListPaymentPackages();
            pack.name = "--Package--";
            pack.id = 0;


            list.Add(pack);

            foreach (package x in pa)
            {
                list.Add(x);
            }

            ddlPaymentPackage.DataSource = list;
            ddlPaymentPackage.DataBind();

            ddlPaymentPackage.SelectedValue = pack.name;
        }

        /*private void loadYears()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int yr = 0;

            listOfYears.Add("-YYYY-");
            for (int i = 0; i <= 11; i++)
            {
                yr = year + i;
                listOfYears.Add(yr.ToString());
            }

            ddlYear.DataSource = listOfYears;
            ddlYear.DataBind();

            ddlYear.SelectedValue = listOfYears[0];
            loadMonths();
        }

        private void loadMonths()
        {
            string text = "";

            listOfMonths.Add("-MM-");
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                    text = string.Format("0{0}", i);
                else
                    text = i.ToString();

                listOfMonths.Add(text);
            }

            ddlMonth.DataSource = listOfMonths;
            ddlMonth.DataBind();

            ddlYear.SelectedValue = listOfYears[0];
        }*/

        private void CheckFirstName()
        {
            CheckEntriesForEmptyString(fname, txtFirstName);
            if (!result)
                return;

            result = v.Validate(fname, validationType.name);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid first name", txtFirstName);
                txtFirstName.Focus();
            }
            else
            {
                emg.hideErrorMessage(txtFirstName);
                txtLastName.Focus();
            }

        }

        private void CheckLastName()
        {
            CheckEntriesForEmptyString(lname, txtLastName);
            if (!result)
                return;

            result = v.Validate(lname, validationType.name);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid surname", txtLastName);
                txtLastName.Focus();
            }
            else
            {
                emg.hideErrorMessage(txtLastName);
                txtEmailAddress.Focus();
            }
        }

        private void CheckEmailAddress()
        {
            CheckEntriesForEmptyString(email, txtEmailAddress);
            if (!result)
                return;

            result = v.Validate(email, validationType.email);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid email address", txtEmailAddress);
                txtEmailAddress.Focus();
            }
            else
            {
                eresult = dp.GetUserByEmail(email);
                if (eresult != null)
                {
                    emg.showErrorMessage("This email address is already registered", txtEmailAddress);
                    txtEmailAddress.Focus();
                    result = false;
                    return;
                }
                else
                {
                    emg.hideErrorMessage(txtEmailAddress);
                    txtMobileNumber.Focus();
                    result = true;
                }
            }
        }

        private void CheckMobileNumber()
        {
            CheckEntriesForEmptyString(mobileNum, txtMobileNumber);
            if (!result)
                return;

            result = v.Validate(mobileNum, validationType.phoneNum);

            if (!result)
            {
                emg.showErrorMessage("Enter a valid mobile number. Do not add +233 to the beginning of your mobile number", txtMobileNumber);
                txtMobileNumber.Focus();
            }
            else
            {
                mresult = dp.GetUserByMobileNumber(mobileNum);
                if (mresult != null)
                {
                    emg.showErrorMessage("This mobile number is already registered", txtMobileNumber);
                    txtMobileNumber.Focus();
                    result = false;
                    return;
                }
                else
                {
                    emg.hideErrorMessage(txtMobileNumber);
                    txtPassword.Focus();
                    result = true;
                }
            }
        }

        private void CheckPassword()
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

        private void CheckConfirmPassword()
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
                txtConfirmPassword.Attributes.Add("value", cpswd);
                ddlRegion.Focus();
            }

        }

        private void CheckAccountNumber()
        {
            CheckEntriesForEmptyString(acctNum, txtAccountNumber);
            if (!result)
                return;

            paymentType = dp.GetPaymentTypeByName(pt);
            result = v.Validate(acctNum, paymentType);

            if (!result)
            {
                emg.showErrorMessage("The account number you have entered is invalid. Please check your entry and try again.", txtAccountNumber);
                txtAccountNumber.Focus();
                result = false;
            }
            else
            {
                emg.hideErrorMessage(txtAccountNumber);
                result = true;
                //ddlMonth.Focus();
            }
        }

        /* private void ValidatePaymentAccountExpirationDate()
         {
             int currentMonth = DateTime.Now.Month;
             int currentYear = DateTime.Now.Year;

             try
             {
                 int selectedMonth = int.Parse(mth);
                 int selectedYear = int.Parse(yr);

                 if (selectedMonth < currentMonth && selectedYear <= currentYear)
                 {
                     emg.showErrorMessage("Enter a valid expiration date", lblExpiryMonth, upExpiryMonth);
                     ddlMonth.Focus();
                 }
                 else
                 {
                     emg.hideErrorMessage(lblExpiryMonth, upExpiryMonth);
                 }

                 result = true;
             }
             catch (Exception ex)
             {
                 emg.showErrorMessage("Select an expiration date", lblExpiryMonth, upExpiryMonth);
                 ddlMonth.Focus();
                 result = false;
             }
         }*/

        private void CheckPromotionCode()
        {
            if (txtPromotionCode.Text != string.Empty)
            {
                result = v.Validate(promoCode, validationType.promotionCode);

                if (!result)
                {
                    emg.showErrorMessage("Enter a valid promotion code.", txtPromotionCode);
                    txtPromotionCode.Focus();
                    result = false;
                    return;
                }
                else
                {
                    pCodeResult = dp.GetPromotionCodeByCode(txtPromotionCode.Text);
                    if (pCodeResult == null)
                    {
                        emg.showErrorMessage("This promotion code does not exist", txtPromotionCode);
                        txtPromotionCode.Focus();
                        result = false;
                        return;
                    }
                    else
                    {
                        emg.hideErrorMessage(txtPromotionCode);
                        txtPromotionCode.Focus();
                        result = true;
                    }
                }
            }
            else
                //Promotion Code not a required field so can be empty
                result = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CheckFirstName();
            if (!result)
                return;

            CheckLastName();
            if (!result)
                return;

            CheckEmailAddress();
            if (!result)
                return;

            CheckMobileNumber();
            if (!result)
                return;

            CheckPassword();
            if (!result)
                return;

            CheckConfirmPassword();
            if (!result)
                return;

            CheckDDlsForFirstItemInList(ddlRegion, upRegion);
            if (!result)
                return;
            else
                emg.hideErrorMessage(ddlRegion, upRegion);

            CheckDDlsForFirstItemInList(ddlPaymentPackage, upPaymentPackage);
            if (!result)
                return;
            else
                emg.hideErrorMessage(ddlPaymentPackage, upPaymentPackage);

            CheckDDlsForFirstItemInList(ddlPaymentType, upPaymentType);
            if (!result)
                return;
            else
                emg.hideErrorMessage(ddlPaymentPackage, upPaymentPackage);

            /*ValidatePaymentAccountExpirationDate();
            if (!result)
                return;
            else
                emg.hideErrorMessage(lblExpiryMonth, upExpiryMonth);*/


            CheckAccountNumber();
            if (!result)
                return;

            CheckPromotionCode();
            if (!result)
                return;

            if (result)
            {
                btnSave_Click(this, e);
                if (!result)
                    return;

                c = dp.GetClientByKey(c);
                Session.Add("user", c.id);

                //Response.Redirect("Account_Activation.aspx", true);
                Response.Redirect("https://beta.frootaxi.com/Accounts/Account_Activation.aspx", true);
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

        private void CheckDDlsForFirstItemInList(DropDownList ddl, UpdatePanel up)
        {
            if (ddl.SelectedIndex == 0)
            {
                emg.showErrorMessage("This is a required field", up, ddl);
                ddl.Focus();
                result = false;
            }
            else
                result = true;
        }

        private void SetExpirationDate(string month, string year)
        {
            if (month != "" && year != "")
                date = DateTime.Parse(string.Format("{0}/{1}/{2}", "1", month, year));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (mth != null && yr != null)
            //SetExpirationDate(mth, yr);

            SetAndGetClientLocation();
            if (!result)
                return;

            SetAndGetClientPaymentDetails();
            if (!result)
                return;

            AddClient();
            if (!result)
                return;

            AddLoginCredentials();
            if (!result)
                return;

            SendActivationCodes();
            if (!result)
                return;
        }

        private void AddLoginCredentials()
        {
            if (email != null && pswd != null)
            {
                u.client_id = c.id;
                u.email_address = email;
                u.primary_mobile_number = mobileNum;
                u.password = PasswordHash.CreateHash(pswd);
                u.role_id = 1;
                u.account_activated = false;

                dp.AddUser(u);

                dp.GetUserByKey(u);
                Session.Add("user", u);

                result = true;
            }
            else
            {
                el.client_id = u.client_id;
                el.source_of_error = "AddLoginCredentials";
                el.location_of_source = "Register.aspx.cs";
                el.error_message = "Could not add user login credentials, email and pswd veriables inexplicably null.";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                result = false;
            }
        }

        private void SendActivationCodes()
        {
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
                el.location_of_source = "Register.aspx.cs";
                el.error_message = "After registering client, user object contains null";
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

        private void AddClient()
        {
            if (result)
            {
                c.first_name = fname;
                c.last_name = lname;
                c.location_id = cl.id;
                c.payment_account_id = paymentAcct.id;
                c.date_time_stamp = DateTime.Now;

                cresult = dp.GetClientByKey(c);
                if (cresult != null)
                {
                    emg.showErrorMessage("This customer already exists. You should Login if you want to make changes to your details. Do this by returning to the home page (click on the Frootaxi Logo on the far left).", txtFirstName);
                    txtFirstName.Focus();
                    result = false;
                }
                else
                {
                    dp.AddClient(c);
                    result = true;
                }
            }
        }

        private void SetAndGetClientLocation()
        {
            if (region_id <= 0)
            {
                emg.showErrorMessage("Your details cannot be saved at this time. Please try again later", lblErrorMessage, upErrorMessage, false);
                result = false;

                el.client_id = u.client_id;
                el.source_of_error = "SetAndGetClientLocation";
                el.location_of_source = "Register.aspx.cs";
                el.error_message = "Cannot save client location, region dropdownlist in register.aspx malfunction";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                //Redirect to landing page?
            }
            else
            {
                cl.region_id = region_id;
                clresult = dp.GetClientLocationByKey(cl);

                if (clresult != null)
                {
                    cl = clresult;
                }
                else
                {
                    dp.AddClientLocation(cl);
                    cl = dp.GetClientLocationByKey(cl);
                }
                result = true;
            }
        }

        private void SetAndGetClientPaymentDetails()
        {
            if (payment_type_id < 0)
            {
                emg.showErrorMessage("Your details cannot be saved at this time. Please try again later", lblErrorMessage, upErrorMessage, false);
                result = false;

                el.client_id = u.client_id;
                el.source_of_error = "SetAndGetClientPaymentDetails";
                el.location_of_source = "Register.aspx.cs";
                el.error_message = "Cannot save client payment details, payment dropdownlist im register.aspx malfunction";
                el.notification_message_sent = false;
                el.date_time_stamp = DateTime.Now;
                dp.AddErrorLog(el);

                //Redirect to landing page?
            }
            else
            {
                paymentAcct.type_id = payment_type_id;
                paymentAcct.account_number = acctNum;
                paymentAcct.expiration_date = date;

                paresult = dp.GetPaymentAccountByKey(paymentAcct);

                if (paresult != null)
                {
                    el.client_id = u.client_id;
                    el.source_of_error = "SetAndGetClientPaymentDetails";
                    el.location_of_source = "Register.aspx.cs";
                    el.error_message = "user entering more than one client for payment account in add one user page rather than add family page";
                    el.notification_message_sent = false;
                    el.date_time_stamp = DateTime.Now;
                    dp.AddErrorLog(el);

                    /*emg.showErrorMessage("The payment details entered have been added for this user. Add more users to this payment account in the Family and friends section located at the top right corner under settings.", lblErrorMessage, upErrorMessage, false);
                    result = false;*/

                    paymentAcct = paresult;
                    result = true;
                }
                else
                {
                    dp.AddPaymentAccount(paymentAcct);
                    paymentAcct = dp.GetPaymentAccountByKey(paymentAcct);
                    result = true;
                }
            }
        }

        private void resetAllFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmailAddress.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtAccountNumber.Text = "";
            txtPromotionCode.Text = "";

            //loadRegions();
            //loadPaymentTypes();
            //loadYears();
        }

        private void errorFromSaveFunction()
        {
            em.recipient_email_address = "support@frootaxi.com";
            em.message = "client " + u.client_id + " cannot register.";
            em.time_entered_in_db = DateTime.Now;
            em.subject = "client registration error";
            em.message_format = "html";
            em.email_sent = false;

            result = emh.SendEmailUsingSMTP(em);
            if (!result)
                result = emh.SendEmailUsingSMTP(em);
            if (!result)
                result = emh.SendEmailUsingSMTP(em);

            el.client_id = u.client_id;
            el.source_of_error = "errorFromSaveFunction";
            el.location_of_source = "Register.aspx.cs";
            el.error_message = "User details cannot be saved during registration";
            el.notification_message_sent = result;
            el.date_time_stamp = DateTime.Now;
            dp.AddErrorLog(el);

            emg.showErrorMessage("Your details could not be saved at this time. An email has been sent to the Frootaxi Support Team. You will receive an email with further instructions.", lblErrorMessage, upErrorMessage, false);
            resetAllFields();
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentType.SelectedItem.ToString().ToLower() == "mpower")
                emg.loadScript("loadMPowerDiv();", upPaymentType);
            else if (ddlPaymentType.SelectedItem.ToString().ToLower() == "expresspaygh")
                emg.loadScript("loadExpressPayDiv();", upPaymentType);
            else
                emg.loadScript("unloadAllPaymentTypes();", upPaymentType);
        }
    }
}