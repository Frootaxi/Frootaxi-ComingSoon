using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAL;

namespace EngineClass
{
    public class Validator
    {
        DataProvider dp = DataProvider.GetInstance();
        error_log el = new error_log();
        
        bool IsValid = false;
        int switchExpression = 0;

        public bool Validate(string text, validationType v)
        {
            switchExpression = (int)v;

            switch (switchExpression)
            {
                //0:Check validity of first name and surname entries
                case 0:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^[a-zA-Z][a-zA-Z\-']*[a-zA-Z]{1,40}$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with first name/last name regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //1:Check validity of email address entries
                case 1:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with email address regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //2:Check validity of phone number entries
                case 2:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^(\d{5}-\d{4}|\d{10}|\d{10})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with phone number regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //3:Check validity of password entries
                case 3:
                    try
                    {
                        if (!Regex.IsMatch(text, @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,10})$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with password regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //4:Check validity of promotion codes entries
                case 4:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^[a-zA-Z0-9]+$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with promotion code regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;

                //5:Check validity of GUID entries
                case 5:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with GUID entry regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //6:Check validity of money
                case 6:
                    try
                    {
                        if (!Regex.IsMatch(text, @"(?=.)^\$?(([1-9][0-9]{0,2}(,[0-9]{3})*)|[0-9]+)?(\.[0-9]{1,2})?$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with money entry regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;

                    //7:Check validity of numbers
                case 7:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^[0-9]+$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with number entry regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
                //8:Check validity of dates
                case 8:
                    try
                    {
                        if (!Regex.IsMatch(text, @"^(19|20)\d\d([- /.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])$"))
                        {
                            IsValid = false;
                        }
                        else
                            IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        el.client_id = 0;
                        el.source_of_error = "Validate";
                        el.location_of_source = "Validator.cs";
                        el.error_message = "Validation error with date entry regex test. Error msg:" + ex;
                        el.notification_message_sent = false;
                        el.date_time_stamp = DateTime.Now;
                        dp.AddErrorLog(el);

                        IsValid = false;
                    }
                    break;
            }
            return IsValid;
        }

        public bool Validate(string acctNum, payment_account_type paymentType)
        {
            switch (paymentType.type.Trim())
            {
                case "mpower":
                    //validate mpower account number
                    try
                    {
                        int i = int.Parse(acctNum);

                        //Validate phone number
                        try
                        {
                            if (!Regex.IsMatch(acctNum, @"^(\d{5}-\d{4}|\d{10}|\d{10})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$"))
                            {
                                IsValid = false;
                            }
                            else
                                IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            el.client_id = 0;
                            el.source_of_error = "Validate";
                            el.location_of_source = "Validator.cs";
                            el.error_message = "Validation error with MPower account number regex test. Error msg:" + ex;
                            el.notification_message_sent = false;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);

                            IsValid = false;
                        }

                    }
                    catch
                    {
                        //Validate username
                        try
                        {
                            if (!Regex.IsMatch(acctNum, @"^[a-zA-Z0-9]+$"))
                            {
                                IsValid = false;
                            }
                            else
                                IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            el.client_id = 0;
                            el.source_of_error = "Validate";
                            el.location_of_source = "Validator.cs";
                            el.error_message = "Validation error with MPower username regex test. Error msg:" + ex;
                            el.notification_message_sent = false;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);

                            IsValid = false;
                        }
                    }
                    break;

                case "cash":
                    //validate mpower account number
                    try
                    {
                        int i = int.Parse(acctNum);

                        //Validate phone number
                        try
                        {
                            if (!Regex.IsMatch(acctNum, @"^(\d{5}-\d{4}|\d{10}|\d{10})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$"))
                            {
                                IsValid = false;
                            }
                            else
                                IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            el.client_id = 0;
                            el.source_of_error = "Validate";
                            el.location_of_source = "Validator.cs";
                            el.error_message = "Validation error with phone number regex test. Error msg:" + ex;
                            el.notification_message_sent = false;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);

                            IsValid = false;
                        }

                    }
                    catch
                    {
                        //Validate username
                        try
                        {
                            if (!Regex.IsMatch(acctNum, @"^[a-zA-Z0-9]+$"))
                            {
                                IsValid = false;
                            }
                            else
                                IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            el.client_id = 0;
                            el.source_of_error = "Validate";
                            el.location_of_source = "Validator.cs";
                            el.error_message = "Validation error with MPower username regex test. Error msg:" + ex;
                            el.notification_message_sent = false;
                            el.date_time_stamp = DateTime.Now;
                            dp.AddErrorLog(el);

                            IsValid = false;
                        }
                    }
                    break;

                case "mtn mobile money":
                    //validate mtn mobile money account number
                    IsValid = false;
                    break;

                case "tigo cash":
                    //validate tigo cash account number
                    IsValid = false;
                    break;

                case "airtel money":
                    //validate airtel money account number
                    IsValid = false;
                    break;

            }
            return IsValid;
        }

        public bool AuthenticateAccount(string acctNum, payment_account_type paymentType)
        {
            switch (paymentType.type.Trim())
            {
                case "mpower":
                    //authenticate mpower account
                    IsValid = true;
                    break;
                case "mtn mobile money":
                    //authenticate mtn mobile money account
                    IsValid = false;
                    break;
                case "tigo cash":
                    //authenticate tigo cash account
                    IsValid = false;
                    break;
                case "airtel money":
                    //authenticatae airtel money account
                    IsValid = false;
                    break;
            }
            return IsValid;
        }
    }

    /*private void loadDays(int year, int month)
    {           
        int days = DateTime.DaysInMonth(year, month);
        for (int i = 1; i <= days; i++)
        {
            listOfdays.Add(i);
        }

        ddlDay.DataSource = listOfdays;
        ddlDay.DataBind();
    }
     
     loadDays(year, month);
     ddlDay.SelectedValue = DateTime.Now.Day.ToString();
     
     
    public string setMonth(int mth)
    {
        string name = "";
            
        switch (mth)
        {
            case 1:
                name = "January";
                break;
            case 2:
                name = "February";
                break;
            case 3:
                name = "March";
                break;
            case 4:
                name = "April";
                break;
            case 5:
                name = "May";
                break;
            case 6:
                name = "June";
                break;
            case 7:
                name = "July";
                break;
            case 8:
                name = "August";
                break;
            case 9:
                name = "September";
                break;
            case 10:
                name = "October";
                break;
            case 11:
                name = "November";
                break;
            case 12:
                name = "December";
                break;
        }
        return name;
    }*/

    /*private DateTime GetDateTime(string date, string hour, string minute, string timeOfDay)
    {
        DateTime time = new DateTime();
        if (timeOfDay == "AM")
        {
            time = DateTime.Parse(string.Format("{0} {1}:{2}", date, hour, minute));
        }

        if (timeOfDay == "PM")
        {
            int newhour = ConvertHour(int.Parse(hour));
            time = DateTime.Parse(string.Format("{0} {1}:{2}", date, newhour, minute));
        }
        return time;
    }*/

    public enum validationType
    {
        name = 0,
        email = 1,
        phoneNum = 2,
        password = 3,
        promotionCode = 4,
        guid = 5,
        money = 6,
        number = 7,
        date = 8
    }

    public enum Month
    {
        january = 1,
        february = 2,
        march = 3,
        april = 4,
        may = 5,
        june = 6,
        july = 7,
        august = 8,
        september = 9,
        october = 10,
        november = 11,
        december = 12
    }
}
