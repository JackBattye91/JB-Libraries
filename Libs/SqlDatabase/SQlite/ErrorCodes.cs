using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.SQlite {
    internal class ErrorCodes {
        private const int SCOPE = 5;

        public const int PARSE_DATA_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int CREATE_TABLE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int RUN_QUERY_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
        public const int RUN_STORE_PROCEDURE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 4;
        public const int GET_DATA_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 5;
        public const int UNABLE_TO_OPEN_DATA_BASE = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 6;
        public const int CREATE_DATABASE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 7;
        public const int POPULATE_OBJECT_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 8;
        public const int INSERT_DATA_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 9;
        public const int GET_OBJECT_VALUES_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 10;
        public const int DELETE_TABLE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 11;
    }
}
