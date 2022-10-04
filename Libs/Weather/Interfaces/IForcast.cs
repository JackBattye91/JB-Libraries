using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.Interfaces {
    public interface IForcast {
        string Title { get; set; }
        DateTime Date { get; set; }
        string? Description { get; set; }
        string? Location { get; set; }
        IGpsCoordinate? GpsLocation { get; set; }
        float? TemeratureCelsius { get; set; }
        float? TemeratureFahrenheit { get; set; }
        float? MinimumTemeratureCelsius { get; set; }
        float? MinimumTemeratureFahrenheit { get; set; }
        float? MaximumTemeratureCelsius { get; set; }
        float? MaximumTemeratureFahrenheit { get; set; }
        float? WindSpeedMph { get; set; }
        string? WindDirection { get; set; }
        float? Humidity { get; set; }
        float? PressureMb { get; set; }
    }
}
