using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase.Cosmos {
    internal class ErrorCodes {
        private const int BASE = 1 * JB.Common.ErrorCodes.API_BASE;
        private const int LIB = 0 * JB.Common.ErrorCodes.LIBRARY_BASE;

        public const int UNABLE_TO_GET_CONTAINER =      BASE + LIB + 1;
        public const int UNABLE_TO_CREATE_DATABASE =    BASE + LIB + 2;
        public const int UNABLE_TO_CREATE_CONTAINER =   BASE + LIB + 3;
        public const int UNABLE_TO_CREATE_ITEM  =       BASE + LIB + 4;
        public const int UNABLE_TO_GET_ITEMS  =         BASE + LIB + 5;
        public const int UNABLE_TO_UPDATE_ITEM  =       BASE + LIB + 6;
        public const int UNABLE_TO_DELETE_ITEM  =       BASE + LIB + 7;
        public const int UNABLE_TO_GET_DATABASE =       BASE + LIB + 8;
        public const int NO_CONTAINER_RETURNED =        BASE + LIB + 9;
    }
}
