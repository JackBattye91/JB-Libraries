using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public sealed class ErrorCodes {
        public const int SCOPE = 0;
        public const int SCOPE_OFFSET = 10000;

        public const int BAD_STATUS_CODE_RETURNED = 1;
        public const int TOKEN_SIGNATURE_DO_NOT_MATCH = 2;
        public const int SIGNING_TOKEN_FAILED = 3;
        public const int VALIDATE_TOKEN_FAILED = 4;
        public const int CONVERT_TO_BASE_64_FAILED = 5;
        public const int CONVERT_FROM_BASE_64_FAILED = 6;
    }
}
