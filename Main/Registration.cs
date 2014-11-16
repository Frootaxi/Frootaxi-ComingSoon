using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace EngineClass
{
    public class Registration
    {
        DataProvider dp = DataProvider.GetInstance();

        client_activation_code cac = new client_activation_code();
        user sessionUser = new user(); 
        string mobileActivationCode = "";
        Guid emailActivationCode = new Guid();
        static Random rdm = new Random();

        public void ResendMobileActivationCode(user u)
        {
            GenerateMobileActivationCode(u);

            cac = dp.GetClientActivationCode(u);
            if (cac != null)
            {
                cac.mobile_activation_code = mobileActivationCode;
                dp.UpdateClientMobileActivationStatus(cac);
            }
        }
        
        public void GenerateMobileActivationCode(user u)
        {
            sessionUser = u;
            mobileActivationCode = rdm.Next(1000,9999).ToString();
        }

        public void GenerateEmailActivationGuid(user u)
        {
            sessionUser = u;
            emailActivationCode = Guid.NewGuid();
        }

        public void AddActivationCodes()
        {
            cac.client_id = sessionUser.client_id;
            cac.mobile_activation_code = mobileActivationCode;
            cac.email_activation_code = emailActivationCode.ToString();
            cac.activated_email = 0;
            cac.activated_mobile = 0;
            cac.date_time_stamp = DateTime.Now;
            
            dp.AddActivationCodes(cac);
        }
    }
}
