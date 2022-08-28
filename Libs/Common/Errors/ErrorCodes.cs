using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public class ErrorCodes {
        private static int SCOPE = 0;

        public static Error SUCCESS = new Error(SCOPE, 0);
        public static Error BAD_HTTP_STATUS_CODE = new Error(SCOPE, 1);
    }
}
