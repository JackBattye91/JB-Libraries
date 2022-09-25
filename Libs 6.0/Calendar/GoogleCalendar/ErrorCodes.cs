using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.Calendar.GoogleCalendar {
    public class ErrorCodes {
        // XXX-XXX-XXXX
        // 200 000 0000
        private const int SCOPE = 1;

        public static Error UNABLE_TO_INSERT_NEW_ITEM =         new Error(SCOPE, 1);
        public static Error UNABLE_TO_UPDATE_ITEM =             new Error(SCOPE, 2);
        public static Error UNABLE_TO_DELETE_ITEM =             new Error(SCOPE, 3);
        public static Error UNABLE_TO_GET_USER_CREDENTIALS =    new Error(SCOPE, 4);
        public static Error UNABLE_TO_GET_ITEMS =               new Error(SCOPE, 5);
    }
}
