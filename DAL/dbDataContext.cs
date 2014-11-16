using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Properties;

namespace DAL
{
    public partial class dbDataContext
    {
        /*public dbDataContext() :
            base(ConfigurationManager.ConnectionStrings[0].ConnectionString.ToString())
        {
            OnCreated();
        }

        public dbDataContext() :
            base(global::DAL.Properties.Settings.Default.frootaxi_dbConnectionString, mappingSource)
        {
            OnCreated();
        }*/
    }
}
