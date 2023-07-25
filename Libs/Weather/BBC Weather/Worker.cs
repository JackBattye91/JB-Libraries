using JB.Common;
using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JB.Weather.BBC_Weather {
    internal class Worker {
        public static IReturnCode<IForcast> ExtractForcast(XmlNode? pXmlNode) {
            IReturnCode<IForcast> rc = new ReturnCode<IForcast>();
            IForcast forcast = new Models.Forcast();

            try {
                string? description = pXmlNode?["description"]?.InnerText;
                string[]? geoText = pXmlNode?["georss:point"]?.InnerText?.Split(" ");
                IDictionary<string, string> descriptionDetails = ExtractDescriptionDetails(description);

                if (geoText != null) {

                }

                forcast.Title = pXmlNode?["title"]?.InnerText ?? "";
                forcast.Date = DateTime.Parse(pXmlNode?["dc:date"]?.InnerText ?? "");
                forcast.Description = description;
                forcast.Location = pXmlNode?["description"]?.InnerText;
                forcast.GpsLocation = null;

                if (descriptionDetails.ContainsKey("temperature")) {
                    var temps = ExtractTemperatures(descriptionDetails["temperature"]);

                    forcast.TemeratureCelsius = temps["celsius"];
                    forcast.TemeratureFahrenheit = temps["fahrenheit"];
                }

                if (descriptionDetails.ContainsKey("wind speed")) {
                    forcast.WindSpeedMph = 0;// descriptionDetails["Wind Speed"];
                }

                if (descriptionDetails.ContainsKey("wind direction")) {
                    forcast.WindDirection = descriptionDetails["wind direction"];
                }

                forcast.Humidity = 0;
                forcast.PressureMb = 0;
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

        public static IDictionary<string, string> ExtractDescriptionDetails(string? pDescription) {
            IDictionary<string, string> details = new Dictionary<string, string>();
            string[]? parts = pDescription?.Split(',');

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
            string[]? parts = pDescription?.Split(" ");

            if (parts != null) {
                foreach (string part in parts) {
                    if (part.ToUpper().Contains("C")) {
                        if (float.TryParse(part, out float value)) {
                            temperatures.Add("celsius", value);
                        }
                    }
                    else if (part.ToUpper().Contains("F")) {
                        if (float.TryParse(part, out float value)) {
                            temperatures.Add("fahrenheit", value);
                        }
                    }
                }
            }

            return temperatures;
        }
    }
}
