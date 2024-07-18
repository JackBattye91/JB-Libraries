using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.Interfaces {
    public interface IForecast {
        string? Day { get; set; }
        DateTime? Date { get; set; }
        string? Description { get; set; }
        IGpsCoordinate? GpsLocation { get; set; }
        float? MinimumTemperatureCelsius { get; set; }
        float? MinimumTemperatureFahrenheit { get; set; }
        float? MaximumTemperatureCelsius { get; set; }
        float? MaximumTemperatureFahrenheit { get; set; }
        float? WindSpeedMph { get; set; }
        string? WindDirection { get; set; }
        float? Humidity { get; set; }
        float? PressureMb { get; set; }
        string? Pollution { get; set; }
        int? UvRisk { get; set; }
    }
}
