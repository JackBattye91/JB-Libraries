using JB.Common;
using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace JB.Weather.OpenWeather {
    internal class Wrapper : IWrapper {
        public Task<IReturnCode<IList<IForecast>>> Get3DayForecast(string pAreaCode) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<IForecast>> GetTodaysForecast(string pAreaCode) {
            throw new NotImplementedException();
        }
    }
}
