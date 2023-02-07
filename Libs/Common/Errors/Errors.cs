using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public interface IError {
        int Scope { get; }
        int Code { get; set; }
        Exception? Exception { get; set; }
        DateTime TimeStamp { get; }
    }
    public interface INetworkError : IError {
        HttpStatusCode StatusCode { get; set; }
    }

    public class Error : IError {
        public int Scope { get; protected set; }
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public Error() {
            Scope = 0;
            Code = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
        }

        public Error(int scope, int code, Exception? exception = null) {
            Scope = scope;
            Code = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
        }
    }
    public class NetworkError : INetworkError {
        public int Scope { get; protected set; }
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StatusCode { get; set; }

        public NetworkError() {
            Scope = 0;
            Code = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
            StatusCode = HttpStatusCode.OK;
        }

        public NetworkError(int scope, int code, HttpStatusCode? statusCode = null, Exception? exception = null) {
            Scope = scope;
            Code = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
            StatusCode = statusCode ?? HttpStatusCode.OK;
        }
    }
}
