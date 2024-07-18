using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.MsSql {
    public sealed class ErrorCodes {
        private const int SCOPE = 6;

        public const int CREATE_CONNECTION_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int UNABLE_TO_OPEN_CONNECTION_TO_SERVER = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int CREATE_DATABASE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
        public const int UNABLE_TO_GET_CONNECTION_STRING = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 4;
        public const int CONNECTED_TO_INCORRECT_DATABASE = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 5;
    }
}
