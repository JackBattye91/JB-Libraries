using JB.Common;
using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace JB.Weather.BBC_Weather {
    internal class Worker {
        public static IReturnCode<IForecast> ExtractForecast(XmlNode? pXmlNode) {
            IReturnCode<IForecast> rc = new ReturnCode<IForecast>();
            IForecast forcast = new Models.Forecast();

            try {
                string? title = pXmlNode?[Consts.ItemTag.Title]?.InnerText;
                string? description = pXmlNode?[Consts.ItemTag.Description]?.InnerText;
                string? geoText = pXmlNode?[Consts.ItemTag.GeoPos]?.InnerText;
                IDictionary<string, string> titleDetails = ExtractDetails(title);
                IDictionary<string, string> descriptionDetails = ExtractDetails(description);

                forcast.Day = titleDetails.Keys.ElementAt(0).PascalCase();
                forcast.Date = DateTime.Parse(pXmlNode?[Consts.ItemTag.Date]?.InnerText ?? "");
                forcast.Description = titleDetails.Values.ElementAt(0);
                forcast.GpsLocation = ExtractGpsCoordinate(geoText);

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.MaxTemp)) {
                    var temps = ExtractTemperatures(descriptionDetails[Consts.ItemDescriptionDetail.MaxTemp]);

                    forcast.MaximumTemperatureCelsius = temps["celsius"];
                    forcast.MaximumTemperatureFahrenheit = temps["fahrenheit"];
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.MinTemp)) {
                    var temps = ExtractTemperatures(descriptionDetails[Consts.ItemDescriptionDetail.MinTemp]);

                    forcast.MinimumTemperatureCelsius = temps["celsius"];
                    forcast.MinimumTemperatureFahrenheit = temps["fahrenheit"];
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.WindSpeed)) {
                    forcast.WindSpeedMph = ExtractWindSpeedMph(descriptionDetails[Consts.ItemDescriptionDetail.WindSpeed]);
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.WindDirection)) {
                    forcast.WindDirection = descriptionDetails[Consts.ItemDescriptionDetail.WindDirection];
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.Humidity)) {
                    forcast.Humidity = ExtractHumidity(descriptionDetails[Consts.ItemDescriptionDetail.Humidity]);
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.Pressure)) {
                    forcast.PressureMb = ExtractPressure(descriptionDetails[Consts.ItemDescriptionDetail.Pressure]);
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.Pollution)) {
                    forcast.Pollution = descriptionDetails[Consts.ItemDescriptionDetail.Pollution];
                }

                if (descriptionDetails.ContainsKey(Consts.ItemDescriptionDetail.UvRisk)) {
                    if (int.TryParse(descriptionDetails[Consts.ItemDescriptionDetail.UvRisk], out int uvRisk)) {
                        forcast.UvRisk = uvRisk;
                    }
                }

            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.EXTRACT_FORCAST_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, System.Net.HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = forcast;
            }

            return rc;
        }

        public static Interfaces.IGpsCoordinate? ExtractGpsCoordinate(string? geoText) {
            Interfaces.IGpsCoordinate? coordinate = null;
            string[]? parts = geoText?.Split(" ");

            if (parts != null && parts.Length == 2) {
                if (float.TryParse(parts[0], out float log) == false) {
                    return null;
                }
                if (float.TryParse(parts[1], out float lat) == false) {
                    return null;
                }

                coordinate = new Models.GpsCoordinate(log, lat);
            }

            return coordinate;
        }

        public static IDictionary<string, string> ExtractDetails(string? pDetailsText) {
            IDictionary<string, string> details = new Dictionary<string, string>();
            string[]? parts = pDetailsText?.Split(',');

            if (parts != null) {
                foreach (string part in parts) {
                    string[] keyValues = part.Split(':');
                    details.Add(keyValues[0].ToLower().Trim(), keyValues[1].Trim());
                }
            }

            return details;
        }
        public static IDictionary<string, float> ExtractTemperatures(string? pDescription) {
            IDictionary<string, float> temperatures = new Dictionary<string, float>();
            Regex regex = new Regex("(?(?=[0-9]+°C)(?'cel'[0-9]+)(°C)|'')[ ]*\\((?(?=[0-9]+°F)(?'fah'[0-9]+)(°F)|'')\\)");

            Match tempsMatch = regex.Match(pDescription ?? "");

            Group? celciusGroup = tempsMatch.Groups.ContainsKey("cel") ? tempsMatch.Groups["cel"] : null;
            Group? fahrenheitGroup = tempsMatch.Groups.ContainsKey("fah") ? tempsMatch.Groups["fah"] : null;

            if (celciusGroup != null) {
                temperatures.Add("celsius", float.Parse(celciusGroup.Value));
            }

            if (fahrenheitGroup != null) {
                temperatures.Add("fahrenheit", float.Parse(fahrenheitGroup.Value));
            }

            return temperatures;
        }
        public static int? ExtractPressure(string? pPressure) {
            int? pressure = null;
            Regex regex = new Regex("(?(?=[0-9]+[MmBb])(?'pressure'[0-9]+)([MmBb]{2})|'')");

            Match pressureMatch = regex.Match(pPressure ?? "");
            Group? pressureGroup = pressureMatch.Groups.ContainsKey("pressure") ? pressureMatch.Groups["pressure"] : null;

            if (pressureGroup != null) {
                if (int.TryParse(pressureGroup.Value, out int pres)) {
                    pressure = pres;
                }
            }

            return pressure;
        }
        public static int? ExtractHumidity(string? pHumidity) {
            int? humidity = null;
            Regex regex = new Regex("(?(?=[0-9]+[\\%])(?'humidity'[0-9]+)([\\%])|'')");

            Match humidityMatch = regex.Match(pHumidity ?? "");
            Group? humidityGroup = humidityMatch.Groups.ContainsKey("humidity") ? humidityMatch.Groups["humidity"] : null;

            if (humidityGroup != null) {
                if (int.TryParse(humidityGroup.Value, out int hum)) {
                    humidity = hum;
                }
            }

            return humidity;
        }
        public static int? ExtractWindSpeedMph(string? pWindSpeed) {
            int? windSpeedMph = null;
            Regex regex = new Regex("(?(?=[0-9]+(mph))(?'windspeed'[0-9]+)(mph)|'')");

            Match windSpeedMatch = regex.Match(pWindSpeed ?? "");
            Group? windSpeedGroup = windSpeedMatch.Groups.ContainsKey("windspeed") ? windSpeedMatch.Groups["windspeed"] : null;

            if (windSpeedGroup != null) {
                if (int.TryParse(windSpeedGroup.Value, out int speed)) {
                    windSpeedMph = speed;
                }
            }

            return windSpeedMph;
        }
    }
}
