using System.Security.Cryptography;
using System.Text;
using JB.Common.Networking.JWT;
using System.IO;
using System.Net;
using JB.Common;
using Newtonsoft.Json;

namespace JB {
    class Program {
        static async Task Main(string[] args) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            JB.Weather.IWrapper weatherWrapper = JB.Weather.Factory.CreateWeatherWrapper();

            try {
                if (rc.Success) {
                    IReturnCode<IList<JB.Weather.Interfaces.IForecast>> get3DayForecastRc = await weatherWrapper.Get3DayForecast("8260059");

                    if (get3DayForecastRc.Failed) {
                        ErrorWorker.CopyErrors(get3DayForecastRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = 7;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Failed) {
                foreach(var error in rc.Errors) {
                    Console.WriteLine($"{error.ErrorCode} - {error.Exception?.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}
