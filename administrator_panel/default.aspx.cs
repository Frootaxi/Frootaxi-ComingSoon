using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EngineClass;
using DAL;

namespace administrator_panel
{
    public partial class _default : System.Web.UI.Page
    {
        DataProvider dp = DataProvider.GetInstance();
        ErrorMessageGenerator emg = new ErrorMessageGenerator();
        Validator v = new Validator();

        List<trip_detail> list = new List<trip_detail>();
        trip_detail td = new trip_detail();
        user u = new user();
        bool result = false;
        int trip_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void btnAddPrice_Click(object sender, EventArgs e)
        {
            if (txtTransactionId.Text == "")
            {
                emg.showErrorMessage("Please enter a transaction id", lblErrorMessage, upErrorMessage, false);
                return;
            }

            if (txtTripPrice.Text == "")
            {
                emg.showErrorMessage("Please enter a price", lblErrorMessage, upErrorMessage, false);
                return;
            }


            if (txtTransactionId.Text != "" && txtTripPrice.Text != "")
            {
                result = v.Validate(txtTransactionId.Text, validationType.number);
                if (!result)
                    emg.showErrorMessage("Please enter a valid transaction id", lblErrorMessage, upErrorMessage, false);

                result = v.Validate(txtTripPrice.Text, validationType.money);

                if (!result)
                    emg.showErrorMessage("Please check your price entry", lblErrorMessage, upErrorMessage, false);
                else
                {
                    trip_id = int.Parse(txtTransactionId.Text);
                    td = dp.GetTripDetailsById(trip_id);

                    trip_detail _td = new trip_detail();
                    _td.cost = int.Parse(txtTripPrice.Text);
                    _td.user_id = td.user_id;
                    _td.trip_coordinates = td.trip_coordinates;
                    _td.datetimestamp = td.datetimestamp;

                    dp.DeleteTripDetails(td);

                    dp.AddTripDetails(_td);
                    ClearEntries();

                    emg.showErrorMessage("Cost successfully added for trip with id " + td.id, lblErrorMessage, upErrorMessage, false);
                    emg.loadScript("hideErrorMsg();", upLatestRequests);
                }
            }
        }

        private void ClearEntries()
        {
            txtTransactionId.Text = "";
            txtTripPrice.Text = "";
        }

        private void LoadCount()
        {
            lblTotalRequestCount.Text = dp.ListTripDetailsWithoutCost().Count.ToString();
        }
    }
}