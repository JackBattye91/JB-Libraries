using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase.Cosmos {
    internal class ErrorCodes {
        public const int BASE = 1 * JB.Common.ErrorCodes.API_BASE;
        public const int LIB = 0 * JB.Common.ErrorCodes.LIBRARY_BASE;

        public const int UNABLE_TO_GET_CONTAINER =      BASE + LIB + 1;
        public const int UNABLE_TO_CREATE_DATABASE =    BASE + LIB + 2;
        public const int UNABLE_TO_CREATE_CONTAINER =   BASE + LIB + 3;
    }
}
