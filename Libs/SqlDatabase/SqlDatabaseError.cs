using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JB.SqlDatabase {
    internal class SqlDatabaseError : IError {
        public int Scope { get; } = SQlite.ErrorCodes.SCOPE;
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; set; }

        public SqlDatabaseError(int code, Exception? exception = null) {
            ErrorCode = code;
            Exception = exception;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
