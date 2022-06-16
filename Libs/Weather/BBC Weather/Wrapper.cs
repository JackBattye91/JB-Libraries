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
        public async Task<JB.Common.Errors.IReturnCode<Interfaces.IForcast>> GetTodaysForcast(string pAreaCode) {
            JB.Common.Errors.IReturnCode<Interfaces.IForcast> rc = new JB.Common.Errors.ReturnCode<Interfaces.IForcast>();
            Interfaces.IForcast? forcast = null;
            HttpClient? client = null;

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://weather-broker-cdn.api.bbci.co.uk/en/observation/rss/{ pAreaCode }");
                client = new HttpClient();

                HttpResponseMessage response = await client.SendAsync(httpRequest);
                string responseText = await response.Content.ReadAsStringAsync();
                if (HttpStatusCode.OK == response.StatusCode) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseText);

                    XmlElement? rssElement = xmlDocument["rss"];
                    XmlElement? channelElement = rssElement?["channel"];
                    XmlElement? titleElement = channelElement?["title"];

                    XmlNode? item = channelElement?["item"];
                    forcast = Worker.ExtractForcast(item);
                }
            }

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = forcast;
            }

            return rc;
        }

        public async Task<JB.Common.Errors.IReturnCode<IList<Interfaces.IForcast>>> Get3DayForcast(string pAreaCode) {
            JB.Common.Errors.IReturnCode<IList<Interfaces.IForcast>> rc = new JB.Common.Errors.ReturnCode<IList<Interfaces.IForcast>>();
            IList<Interfaces.IForcast> forcasts = new List<Interfaces.IForcast>();
            HttpClient? client = null;

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://weather-broker-cdn.api.bbci.co.uk/en/forecast/rss/3day/{ pAreaCode }");
                client = new HttpClient();

                HttpResponseMessage response = await client.SendAsync(httpRequest);
                if (HttpStatusCode.OK != response.StatusCode) {
                    rc.ErrorCode = 3;
                    JB.Common.Errors.ErrorWorker.AddError(rc, rc.ErrorCode);
                }

                if (HttpStatusCode.OK == response.StatusCode) {
                    string responseText = await response.Content.ReadAsStringAsync();

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
            }

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = forcasts;
            }

            return rc;
        }
    }
}
