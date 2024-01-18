using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.BBC_Weather.Models {
    internal class GpsCoordinate : Interfaces.IGpsCoordinate {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public GpsCoordinate() {
            Latitude = 0;
            Longitude = 0;
        }
        public GpsCoordinate(float pLatitude, float pLongitude) {
            Latitude = pLatitude;
            Longitude = pLongitude;
        }
    }
}
