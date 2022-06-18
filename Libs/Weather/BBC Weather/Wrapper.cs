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

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                JB.Common.Errors.IReturnCode<string> responseTextRc = await JB.Common.NetworkHelper.GetStringResponse($"https://weather-broker-cdn.api.bbci.co.uk/en/observation/rss/{ pAreaCode }", HttpMethod.Get);
                
                if (responseTextRc.ErrorCode == Common.Errors.ErrorCodes.SUCCESS) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseTextRc.Data ?? "");

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

            if (JB.Common.Errors.ErrorCodes.SUCCESS == rc.ErrorCode) {
                Common.Errors.IReturnCode<string> httpRequestRc = await Common.NetworkHelper.GetStringResponse($"https://weather-broker-cdn.api.bbci.co.uk/en/forecast/rss/3day/{ pAreaCode }", HttpMethod.Get);
                
                if (httpRequestRc.ErrorCode == Common.Errors.ErrorCodes.SUCCESS) {
                    string responseText = httpRequestRc.Data ?? "";

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
