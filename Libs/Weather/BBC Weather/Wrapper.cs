using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;

namespace JB.Weather.BBC_Weather {
    internal class Wrapper : IWrapper {
        public async Task<JB.Common.ReturnCode<Interfaces.IForcast>> GetTodaysForcast(string pAreaCode) {
            JB.Common.ReturnCode<Interfaces.IForcast> rc = new();
            Interfaces.IForcast? forcast = null;
            string responseText = string.Empty;

            if (rc.Success) {
                var getHttpResponseRc = await JB.Common.Networking.Worker.GetStringResponse($"https://weather-broker-cdn.api.bbci.co.uk/en/observation/rss/{pAreaCode}", HttpMethod.Get, null, null, null);

                if (getHttpResponseRc.Success) {
                    responseText = getHttpResponseRc.Data ?? string.Empty;
                }
                else {
                    JB.Common.ErrorWorker.CopyErrors(getHttpResponseRc, rc);
                }
            }

            if (rc.Success) {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseText);

                XmlElement? rssElement = xmlDocument["rss"];
                XmlElement? channelElement = rssElement?["channel"];
                XmlElement? titleElement = channelElement?["title"];

                XmlNode? item = channelElement?["item"];
                forcast = Worker.ExtractForcast(item);
            }

            if (rc.Success) {
                rc.Data = forcast;
            }

            return rc;
        }

        public async Task<JB.Common.ReturnCode<IList<Interfaces.IForcast>>> Get3DayForcast(string pAreaCode) {
            JB.Common.ReturnCode<IList<Interfaces.IForcast>> rc = new();
            IList<Interfaces.IForcast> forcasts = new List<Interfaces.IForcast>();
            string responseText = string.Empty;

            if (rc.Success) {
                var getHttpResponseRc = await JB.Common.Networking.Worker.GetStringResponse($"https://weather-broker-cdn.api.bbci.co.uk/en/forecast/rss/3day/{pAreaCode}", HttpMethod.Get, null, null, null);

                if (getHttpResponseRc.Success) {
                    responseText = getHttpResponseRc.Data ?? string.Empty;
                }
                else {
                    JB.Common.ErrorWorker.CopyErrors(getHttpResponseRc, rc);
                }
            }

            if (rc.Success) {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseText);

                XmlElement? rssElement = xmlDocument["rss"];
                XmlElement? channelElement = rssElement?["channel"];
                XmlElement? titleElement = channelElement?["title"];
                XmlNodeList? itemNodes = channelElement?.GetElementsByTagName("item");

                for (int n = 0; n < itemNodes?.Count; n++) {
                    forcasts.Add(Worker.ExtractForcast(itemNodes[n]));
                }
            }

            if (rc.Success) {
                rc.Data = forcasts;
            }

            return rc;
        }
    }
}
