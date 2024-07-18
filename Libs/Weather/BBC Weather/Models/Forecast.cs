using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.BBC_Weather.Models {
    internal class Forecast : Interfaces.IForecast {
        public string? Day { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public IGpsCoordinate? GpsLocation { get; set; }
        public float? MinimumTemperatureCelsius { get; set; }
        public float? MinimumTemperatureFahrenheit { get; set; }
        public float? MaximumTemperatureCelsius { get; set; }
        public float? MaximumTemperatureFahrenheit { get; set; }
        public float? WindSpeedMph { get; set; }
        public string? WindDirection { get; set; }
        public float? Humidity { get; set; }
        public float? PressureMb { get; set; }
        public string? Pollution { get; set; }
        public int? UvRisk { get; set; }
    }
}
