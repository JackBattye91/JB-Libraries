using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Networking {
    internal class NetworkingError : INetworkError {
        public int Scope { get; } = ErrorCodes.SCOPE;
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; private set; }
        public HttpStatusCode StatusCode { get; set; }

        public NetworkingError(int code, HttpStatusCode status, Exception? ex = null) {
            Scope = ErrorCodes.SCOPE;
            Code = code;
            TimeStamp = DateTime.UtcNow;
            StatusCode = status;
            Exception = ex;
        }
    }
}
