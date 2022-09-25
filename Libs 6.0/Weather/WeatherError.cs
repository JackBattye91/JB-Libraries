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
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public WeatherError(int code, HttpStatusCode statusCode, Exception? ex) {
            Code = code;
            StatusCode = statusCode;
            Exception = ex;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
