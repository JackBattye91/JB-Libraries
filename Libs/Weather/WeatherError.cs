using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather {
    internal class WeatherError : JB.Common.INetworkError {
        public HttpStatusCode StatusCode { get; set; }
        public int Scope { get; } = ErrorCodes.SCOPE;
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public WeatherError(int pErrorCode, HttpStatusCode pStatusCode, Exception? pEx) {
            ErrorCode = pErrorCode;
            StatusCode = pStatusCode;
            Exception = pEx;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
