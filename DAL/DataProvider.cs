using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DataProvider
    {
        static DataProvider instance;
        public dbDataContext context = null;

        public static DataProvider GetInstance()
        {
            if (instance == null)
                instance = new DataProvider();
            return instance;
        }

        private DataProvider()
        {
            context = new dbDataContext();
        }

        public subscribe GetEmailAddress(subscribe s)
        {
            return context.subscribes.Where(x => x.email_address == s.email_address).FirstOrDefault();
        }

        public void AddSubscriber(subscribe s)
        {
            context.subscribes.InsertOnSubmit(s);
            context.SubmitChanges();
        }

        public List<region> ListRegions()
        {
            return new List<region>(context.regions.Distinct().AsEnumerable());
        }

        public List<payment_account_type> ListPaymentTypes()
        {
            return new List<payment_account_type>(context.payment_account_types.Distinct().AsEnumerable());
        }

        public List<package> ListPaymentPackages()
        {
            return new List<package>(context.packages.Distinct().AsEnumerable());
        }

        public payment_account_type GetPaymentTypeByName(string paymentType)
        {
            return context.payment_account_types.Where(x => x.type == paymentType).FirstOrDefault();
        }

        public payment_account_type GetPaymentTypeById(int paymentTypeId)
        {
            return context.payment_account_types.Where(x => x.id == paymentTypeId).FirstOrDefault();
        }

        public region GetRegionById(int region_id)
        {
            return context.regions.Where(x => x.id == region_id).FirstOrDefault();
        }

        public user GetUserByKey(user u)
        {
            return context.users.Where(x => x.client_id == u.client_id).FirstOrDefault();
        }

        public client GetClientByKey(client c)
        {
            return context.clients.Where(x => x.first_name == c.first_name && x.last_name == c.last_name && x.location_id == c.location_id && x.payment_account_id == c.payment_account_id).FirstOrDefault();
        }

        public void AddClient(client c)
        {
            context.clients.InsertOnSubmit(c);
            context.SubmitChanges();
        }

        public client_location GetClientLocationByKey(client_location cl)
        {
            var qry = new client_location();

            if (cl.region_id != null)
            {
                if (cl.city_id != null)
                {
                    if (cl.suburb_id != null)
                    {
                        if (cl.area_id != null)
                        {
                            if (cl.landmark_id != null)
                                qry = context.client_locations.Where(x => x.region_id == cl.region_id && x.city_id == cl.city_id && x.suburb_id == cl.suburb_id && x.area_id == cl.area_id && x.landmark_id == cl.landmark_id).FirstOrDefault();
                            else
                                qry = context.client_locations.Where(x => x.region_id == cl.region_id && x.city_id == cl.city_id && x.suburb_id == cl.suburb_id && x.area_id == cl.area_id).FirstOrDefault();
                        }
                        else
                            qry = context.client_locations.Where(x => x.region_id == cl.region_id && x.city_id == cl.city_id && x.suburb_id == cl.suburb_id).FirstOrDefault();
                    }
                    else
                        qry = context.client_locations.Where(x => x.region_id == cl.region_id && x.city_id == cl.city_id).FirstOrDefault();
                }
                else
                    qry = context.client_locations.Where(x => x.region_id == cl.region_id).FirstOrDefault();
            }

            if (cl.gps_coordinate != null)
            {
                qry = context.client_locations.Where(x => x.gps_coordinate == cl.gps_coordinate).FirstOrDefault();
            }

            return qry;
        }

        public void AddClientLocation(client_location cl)
        {
            context.client_locations.InsertOnSubmit(cl);
            context.SubmitChanges();
        }

        public payment_account GetPaymentAccountById(payment_account paymentAcct)
        {
            return context.payment_accounts.Where(x => x.id == paymentAcct.type_id).FirstOrDefault();
        }

        public payment_account GetPaymentAccountByKey(payment_account paymentAcct)
        {
            return context.payment_accounts.Where(x => x.type_id == paymentAcct.type_id && x.account_number == paymentAcct.account_number && x.expiration_date == paymentAcct.expiration_date).FirstOrDefault();
        }

        public void AddPaymentAccount(payment_account paymentAcct)
        {
            context.payment_accounts.InsertOnSubmit(paymentAcct);
            context.SubmitChanges();
        }

        public void AddUser(user loginAccount)
        {
            context.users.InsertOnSubmit(loginAccount);
            context.SubmitChanges();
        }

        public user GetUserByEmail(string email)
        {
            return context.users.Where(x => x.email_address == email).FirstOrDefault();
        }

        /*public client GetClientByMobileNumber(string mobileNum)
        {
            return context.clients.Where(x => x.mobile_number == mobileNum).FirstOrDefault();
        }*/

        public void AddActivationCodes(client_activation_code cac)
        {
            context.client_activation_codes.InsertOnSubmit(cac);
            context.SubmitChanges();
        }

        public client_activation_code GetClientActivationCode(user u)
        {
            return context.client_activation_codes.Where(x => x.client_id == u.client_id).FirstOrDefault();
        }

        public client_activation_code GetClientActivationCodeByMobileActivationCode(string activation_code)
        {
            return context.client_activation_codes.Where(x => x.mobile_activation_code == activation_code).FirstOrDefault();
        }

        public void UpdateUserActivationStatus(user u)
        {
            context.SubmitChanges();
        }

        public user GetUserByMobileNumber(string user_mobile_num)
        {
            return context.users.Where(x => x.primary_mobile_number == user_mobile_num).FirstOrDefault();
        }

        public void UpdateClientMobileActivationStatus(client_activation_code cac)
        {
            context.SubmitChanges();
        }

        public user GetUserByEmailAddress(string username)
        {
            return context.users.Where(x => x.email_address == username).FirstOrDefault();
        }

        /*public client GetClientByUser(user u)
        {
            return context.clients.Where(x => x.email_address == u.email_address || x.mobile_number == u.mobile_number).FirstOrDefault();
        }*/

        public client_activation_code GetClientActivationCodeByUser(user u)
        {
            return context.client_activation_codes.Where(x => x.client_id == u.client_id).FirstOrDefault();
        }

        public void AddEmail(email e)
        {
            context.emails.InsertOnSubmit(e);
            context.SubmitChanges();
        }

        public void SendActivationEmail(email e)
        {
            context.SendEmail(e.message, e.recipient_email_address, e.subject, e.message_format);
        }

        public client_activation_code GetClientActivationCodeByEmailActivationCode(string emailActivationCode)
        {
            return context.client_activation_codes.Where(x => x.email_activation_code == emailActivationCode).FirstOrDefault();
        }

        public promotion_code GetPromotionCodeByCode(string promoCode)
        {
            return context.promotion_codes.Where(x => x.code == promoCode).FirstOrDefault();
        }

        public void AddPasswordReset(password_reset pr)
        {
            context.password_resets.InsertOnSubmit(pr);
            context.SubmitChanges();
        }

        public password_reset GetPasswordResetInfoByToken(string pt)
        {
            return context.password_resets.Where(x => x.password_reset_token == pt).FirstOrDefault();
        }

        public user GetUserById(int client_id)
        {
            return context.users.Where(x => x.client_id == client_id).FirstOrDefault();
        }

        public void UpdateUserPassword(user u)
        {
            context.SubmitChanges();
        }

        public void AddErrorLog(error_log el)
        {
            context.error_logs.InsertOnSubmit(el);
            context.SubmitChanges();
        }

        public email GetEmailMessage()
        {
            return context.emails.Where(x => x.email_sent == false).FirstOrDefault();
        }

        public List<email> GetSentEmailMessage()
        {
            return new List<email>(context.emails.Where(x => x.email_sent == true).AsEnumerable());
        }

        public void UpdateEmailStatus(email em)
        {
            context.SubmitChanges();
        }

        public void ClearAllSentEmails(email em)
        {
            context.emails.DeleteOnSubmit(em);
        }

        public void DeleteExpiredPasswordResetToken(password_reset pr)
        {
            context.password_resets.DeleteOnSubmit(pr);
            context.SubmitChanges();
        }

        public List<password_reset> GetPasswordResetInfoByUser(user u)
        {
            return new List<password_reset>(context.password_resets.Where(x => x.client_id == u.client_id).AsEnumerable());
        }

        public void UpdateClientEmailActivationStatus(client_activation_code cac)
        {
            context.SubmitChanges();
        }

        public price_matrix GetCostOfJourneyLeg(float startCoords, float endCoords)
        {
            //TODO: Put in logarithm for determining if start and end coords provided for trip within a certain radius of the start and end coords in the database for a specific cost.
            return context.price_matrixes.Where(x => x.start_coord == startCoords.ToString() && x.end_coord== endCoords.ToString()).FirstOrDefault();
        }

        /*public price_matrix GetPriceMatrixByUser(user u)
        {
            return context.price_matrixes.Where(x => x.client_id == u.client_id).FirstOrDefault();
        }*/

        /*public void AddPriceToTransaction(transaction t)
        {
            context.transactions.InsertOnSubmit(t);
            context.SubmitChanges();
        }

        public transaction GetTransactionById(int trans_id)
        {
            return context.transactions.Where(x => x.transaction_id == trans_id).FirstOrDefault();
        }

        public List<gps_coordinate> GetUniqueListOfGPSDevices()
        {
            return new List<gps_coordinate>(context.gps_coordinates.Where(x => x.date_time_stamp.Value.Date == DateTime.Now.Date).Distinct());
        }

        public List<gps_coordinate> ListDriverGPSCoordinatesAtThisTime()
        {
            List<int> list = new List<int>();

            
            DateTime dt = DateTime.Now;
            var gps_coordinate_datetimestamp = ;

            
            return new List<gps_coordinate>(context.gps_coordinates.Where(x => x.dat
        }*/

        public trip_detail GetTripDetailsByUser(user u)
        {
            return context.trip_details.Where(x => x.user_id == u.client_id).FirstOrDefault();
        }

        public void AddNewTripDetails(trip_detail td)
        {
            context.trip_details.InsertOnSubmit(td);
            context.SubmitChanges();
        }

        public void UpdateTripDetails(trip_detail td)
        {
            context.SubmitChanges();
        }

        public role GetRoleByName(string p)
        {
            return context.roles.Where(x => x.name == p).FirstOrDefault();
        }

        public void AddNewClient(client c)
        {
            context.clients.InsertOnSubmit(c);
            context.SubmitChanges();
        }

        public void AddNewUser(user u)
        {
            context.users.InsertOnSubmit(u);
            context.SubmitChanges();
        }

        public client GetClientByFirstName(string p)
        {
            return context.clients.Where(x => x.first_name == p).FirstOrDefault();
        }

        public trip_detail GetTripDetailsById(int trip_id)
        {
            return context.trip_details.FirstOrDefault(x => x.id == trip_id);
        }

        public void DeleteTripDetails(trip_detail td)
        {
            context.trip_details.DeleteOnSubmit(td);
            context.SubmitChanges();
        }

        public void AddTripDetails(trip_detail _td)
        {
            context.trip_details.InsertOnSubmit(_td);
            context.SubmitChanges();
        }

        public trip_detail GetTripDetailsByUserId(int user_id)
        {
            return context.trip_details.Where(x => x.user_id == user_id).FirstOrDefault();
        }

        public List<trip_detail> ListTripDetailsWithoutCost()
        {
            return new List<trip_detail>(context.trip_details.Where(x => x.cost == (decimal)0.0000).AsEnumerable());
        }

        public client GetClientByUserId(int client_id)
        {
            return context.clients.Where(x => x.id == client_id).FirstOrDefault();
        }

        public trip_type GetTripTypeByName(string tripTypeName)
        {
            return context.trip_types.Where(x => x.name == tripTypeName).FirstOrDefault();
        }

        public void UpdateClient(client cresult)
        {
            context.SubmitChanges();
        }

        public client GetClientById(int client_id)
        {
            return context.clients.Where(x => x.id == client_id).FirstOrDefault();
        }
    }
}