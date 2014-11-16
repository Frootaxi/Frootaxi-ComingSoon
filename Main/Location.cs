using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace EngineClass
{
    public class Location
    {
        DataProvider dp = DataProvider.GetInstance();
        List<user> driver = new List<user>();
        public static double min = double.MaxValue;

        public List<user> LocateNearestDriver(trip_detail td)
        {
            List<string> list = SplitCoordinates(td.trip_coordinates);
            List<string> coords = SplitLatLng(list[0]);

            double lat = double.Parse(coords[0].ToString());
            double lng = double.Parse(coords[1].ToString());

            //driver = LocateDrivers(lat, lng);
            return driver;
        }

        public List<string> SplitLatLng(string p)
        {
            List<string> latlng = new List<string>();

            char[] delimiterChars = { '(', ')', ',' };
            string[] _coordinates = p.Split(delimiterChars);

            for (int i = 0; i < _coordinates.Length; i++)
                latlng.Add(_coordinates[i]);

            return latlng;
        }

        public List<string> SplitCoordinates(string s)
        {
            List<string> listOfCoordinates = new List<string>();

            char[] delimiterChars = { '(', ')' };
            string[] _coordinates = s.Split(delimiterChars);

            for (int i = 0; i < _coordinates.Length; i++)
            {
                if(_coordinates[i] != "" && _coordinates[i] != ",")
                    listOfCoordinates.Add(_coordinates[i]);
            }

            return listOfCoordinates;
        }

        /*public List<user> LocateDrivers(double latitude, double longitude)
        {
            double[] distances = new double[ad_shops.Count];
            int i = 0;
            foreach (ad_shop s in ad_shops)
            {
                distances[i] = CalcDistance(longitude, latitude, s.longitude.Value, s.latitude.Value);
                i++;
            }


            int pos = 0;
            for (i = 0; i < distances.Length; i++)
            {
                if (min > distances[i])
                {
                    min = distances[i];
                    pos = i;
                }
            }

            return ad_shops[pos];
        }

        static double CalcDistance(double long1, double lat1, double long2, double lat2)
        {
            float earthRadius = 6371;
            float long1Rad, lat1Rad, long2Rad, lat2Rad;

            long1Rad = (float)(long1 * Math.PI / 180);
            long2Rad = (float)(long2 * Math.PI / 180);
            lat1Rad = (float)(lat1 * Math.PI / 180);
            lat2Rad = (float)(lat2 * Math.PI / 180);

            return Math.Acos(
                    Math.Cos(lat1Rad) * Math.Cos(long1Rad) * Math.Cos(lat2Rad) * Math.Cos(long2Rad) +
                    Math.Cos(lat1Rad) * Math.Sin(long1Rad) * Math.Cos(lat2Rad) * Math.Sin(long2Rad) +
                    Math.Sin(lat1Rad) * Math.Sin(lat2Rad)
                ) * earthRadius;
        }*/
    }

    /// <summary>
    /// The distance type to return the results in.
    /// </summary>
    public enum DistanceType { Miles, Kilometers };
 
    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public struct Position
    {
        public double Latitude;
        public double Longitude;
    }
 
    public class Haversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name=”pos1″></param>
        /// <param name=”pos2″></param>
        /// <param name=”type”></param>
        /// <returns></returns>
        /*public double Distance(Position pos1, Position pos2,DistanceType type)
        {
           double R = (type == DistanceType.Miles) ? 3960 : 6371;
 
            double dLat = this.toRadian(pos2.Latitude – pos1.Latitude);
            double dLon = this.toRadian(pos2.Longitude – pos1.Longitude);
 
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(pos1.Latitude)) *Math.Cos(this.toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
 
            return d;
        }*/
 
        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
