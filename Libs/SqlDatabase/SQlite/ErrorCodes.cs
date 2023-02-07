using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.SQlite {
    internal class ErrorCodes {
        internal const int SCOPE = 5;

        public const int PARSE_DATA_FAILED = 1;
        public const int CREATE_TABLE_FAILED = 2;
        public const int RUN_QUERY_FAILED = 3;
        public const int RUN_STORE_PROCEDURE_FAILED = 4;
        public const int GET_DATA_FAILED = 5;
        public const int UNABLE_TO_OPEN_DATA_BASE = 6;
    }
}
