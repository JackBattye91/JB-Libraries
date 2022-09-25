using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase {
    internal class NoSqlDatabaseError : JB.Common.INetworkError {
        public int Scope { get; } = ErrorCodes.SCOPE;
        public int Code { get; set; }
        public Exception? Exception { get; set; } 
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StatusCode { get; set; }

        public NoSqlDatabaseError(int code, HttpStatusCode statusCode, Exception? ex = null) {
            Code = code;
            Exception = ex;
            StatusCode = statusCode;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
