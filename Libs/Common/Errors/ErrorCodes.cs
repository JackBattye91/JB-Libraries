using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Errors {
    public class ErrorCodes {
        public static int SCOPE = 1;

        public static Error.Code SUCCESS = new Error.Code(SCOPE, 0);
        public static Error.Code BAD_HTTP_STATUS_CODE = new Error.Code(SCOPE, 1);
    }
}
