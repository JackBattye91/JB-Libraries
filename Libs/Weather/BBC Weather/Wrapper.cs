using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;
using JB.Common;

namespace JB.Weather.BBC_Weather
{
    internal class Wrapper : IWrapper {
        public async Task<IReturnCode<IForecast>> GetTodaysForecast(string pAreaCode) {
            IReturnCode<IForecast> rc = new ReturnCode<IForecast>();
            Interfaces.IForecast? forcast = null;
            string responseText = string.Empty;
            XmlNode? forcastNode = null;

            try {
                if (rc.Success) {
                    var getHttpResponseRc = await JB.Common.Networking.Worker.GetStringResponse($"{Consts.Endpoints.BBC_RSS_TODAY_ENDPOINT}{pAreaCode}", HttpMethod.Get, null, null, null);

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

                    forcastNode = channelElement?["item"];
                }

                if (rc.Success) {
                    IReturnCode<IForecast> extractForecastRc = Worker.ExtractForecast(forcastNode);
                    if (extractForecastRc.Success) {
                        forcast = extractForecastRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(extractForecastRc, rc);
                    }
                }
            }
            catch(Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.GET_TODAYS_FORCAST_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = forcast;
            }

            return rc;
        }

        public async Task<IReturnCode<IList<Interfaces.IForecast>>> Get3DayForecast(string pAreaCode) {
            IReturnCode<IList<IForecast>> rc = new ReturnCode<IList<IForecast>>();
            IList<IForecast> forcasts = new List<Interfaces.IForecast>();
            string responseText = string.Empty;

            try {
                if (rc.Success) {
                    var getHttpResponseRc = await JB.Common.Networking.Worker.GetStringResponse($"{Consts.Endpoints.BBC_RSS_3DAY_ENDPOINT}{pAreaCode}", HttpMethod.Get, null, null, null);

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

                    XmlElement? rssElement = xmlDocument[Consts.XmlTag.RSS];
                    XmlElement? channelElement = rssElement?[Consts.XmlTag.Channel];
                    XmlElement? titleElement = channelElement?[Consts.XmlTag.Title];
                    XmlNodeList? itemNodes = channelElement?.GetElementsByTagName(Consts.XmlTag.Item);

                    for (int n = 0; n < itemNodes?.Count; n++) {
                        IReturnCode<IForecast> extractForcastRc = Worker.ExtractForecast(itemNodes[n]);
                        if (extractForcastRc.Success) {
                            if (extractForcastRc.Data != null) {
                                forcasts.Add(extractForcastRc.Data);
                            }
                        }
                        else {
                            ErrorWorker.CopyErrors(extractForcastRc, rc);
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.GET_3DAY_FORCAST_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = forcasts;
            }

            return rc;
        }
    }
}
