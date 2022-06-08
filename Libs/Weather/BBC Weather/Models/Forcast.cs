using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.BBC_Weather.Models {
    internal class Forcast : Interfaces.IForcast {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public Interfaces.IGpsCoordinate? GpsLocation { get; set; }
        public float? TemeratureCelsius { get; set; }
        public float? TemeratureFahrenheit { get; set; }
        public float? WindSpeedMph { get; set; }
        public string? WindDirection { get; set; }
        public float? Humidity { get; set; }
        public float? PressureMb { get; set; }

        public Forcast() {
            Title = string.Empty;
            Date = new DateTime();
        }
    }
}
