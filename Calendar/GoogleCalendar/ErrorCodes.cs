using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar.GoogleCalendar {
    public class ErrorCodes {
        // XXX-XXX-XXXX
        // 200 000 0000
        private const int BASE = 2 * JB.Common.ErrorCodes.API_BASE;
        private const int LIB = 0 * JB.Common.ErrorCodes.LIBRARY_BASE;

        public const int UNABLE_TO_INSERT_NEW_ITEM =        BASE + LIB + 1;
        public const int UNABLE_TO_UPDATE_ITEM =            BASE + LIB + 2;
        public const int UNABLE_TO_DELETE_ITEM =            BASE + LIB + 3;
        public const int UNABLE_TO_GET_USER_CREDENTIALS =   BASE + LIB + 4;
        public const int UNABLE_TO_GET_ITEMS =              BASE + LIB + 5;
    }
}
