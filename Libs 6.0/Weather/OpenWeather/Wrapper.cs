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
        public async Task<ReturnCode<IList<IForcast>>> Get3DayForcast(string pAreaCode) {
            throw new NotImplementedException();
        }

        public async Task<ReturnCode<IForcast>> GetTodaysForcast(string pAreaCode) {
            throw new NotImplementedException();
        }
    }
}
