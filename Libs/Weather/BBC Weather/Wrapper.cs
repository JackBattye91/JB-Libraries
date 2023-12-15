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
        public async Task<IReturnCode<IForcast>> GetTodaysForcast(string pAreaCode) {
            IReturnCode<IForcast> rc = new ReturnCode<IForcast>();
            Interfaces.IForcast? forcast = null;
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
                    IReturnCode<IForcast> extractForcastRc = Worker.ExtractForcast(forcastNode);
                    if (extractForcastRc.Success) {
                        forcast = extractForcastRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(extractForcastRc, rc);
                    }
                }
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_TODAYS_FORCAST_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = forcast;
            }

            return rc;
        }

        public async Task<IReturnCode<IList<Interfaces.IForcast>>> Get3DayForcast(string pAreaCode) {
            IReturnCode<IList<IForcast>> rc = new ReturnCode<IList<IForcast>>();
            IList<IForcast> forcasts = new List<Interfaces.IForcast>();
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

                    XmlElement? rssElement = xmlDocument["rss"];
                    XmlElement? channelElement = rssElement?["channel"];
                    XmlElement? titleElement = channelElement?["title"];
                    XmlNodeList? itemNodes = channelElement?.GetElementsByTagName("item");

                    for (int n = 0; n < itemNodes?.Count; n++) {
                        IReturnCode<IForcast> extractForcastRc = Worker.ExtractForcast(itemNodes[n]);
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
                rc.ErrorCode = ErrorCodes.GET_3DAY_FORCAST_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = forcasts;
            }

            return rc;
        }
    }
}
