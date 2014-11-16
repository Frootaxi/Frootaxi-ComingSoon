using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DAL;

namespace EngineClass
{
    public class Accounts
    {
        static DataProvider dp = DataProvider.GetInstance();
        static SMSMessageHandler smh = new SMSMessageHandler();
        price_matrix pr = new price_matrix();

        static user u = new user();
        trip_detail td = new trip_detail();
        List<string> listOfCoordinates = new List<string>();
        List<price_matrix> listOfCosts = new List<price_matrix>();

        public bool CheckCustomerCredit(string username)
        {
            return false;
        }

        public void TimeOut(int trip_det_id)
        {
            Thread.Sleep(3000);
            td = dp.GetTripDetailsById(trip_det_id);
        }

        public void CalculateTripCost(trip_detail td)
        {
            /*SplitCoordinates(td.trip_coordinates);
            foreach (string s in listOfCoordinates)
            {
                string[] _coords = s.Split(',');
                float startCoords = float.Parse(_coords[0]);
                float endCoords = float.Parse(_coords[1]);

                listOfCosts.Add(GetCost(startCoords, endCoords));
            }

            td.cost = AggregateCosts(listOfCosts);
            dp.UpdateTripDetails(td);*/
        }

        /*private decimal AggregateCosts(List<price_matrix> list)
        {
            decimal cost = 0;

            foreach (price_matrix pm in list)
            {
                if(DateTime.Now.

                switch()
                {
                    case 1:

                }


                cost += pm.off_peak_cost;
            }

            return cost;
        }*/

        private void SplitCoordinates(string s)
        {
            char[] delimiterChars = { '(', ')' };
            string[] _coordinates = s.Split(delimiterChars);

            for (int i = 0; i < _coordinates.Length; i++)
                listOfCoordinates.Add(_coordinates[i]);
        }

        /*public price_matrix GetCost(List<float> startCoords, List<float> endCoords)
        {
            pr = dp.GetCostOfJourney(startCoords, endCoords);
            return pr;

        }*/

        public price_matrix GetCost(float startCoords, float endCoords)
        {
            pr = dp.GetCostOfJourneyLeg(startCoords, endCoords);
            return pr;

        }
    }
}
