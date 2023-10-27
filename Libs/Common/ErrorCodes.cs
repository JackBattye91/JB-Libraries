using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public sealed class ErrorCodes {
        private const int SCOPE = 0;
        public const int SCOPE_OFFSET = 10000;

        public const int BAD_STATUS_CODE_RETURNED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int TOKEN_SIGNATURE_DO_NOT_MATCH = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int SIGNING_TOKEN_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
        public const int VALIDATE_TOKEN_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 4;
        public const int CONVERT_TO_BASE_64_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 5;
        public const int CONVERT_FROM_BASE_64_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 6;
    }
}
