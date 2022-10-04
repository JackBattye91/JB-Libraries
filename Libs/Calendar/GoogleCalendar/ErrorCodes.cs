using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.Calendar.GoogleCalendar {
    public class ErrorCodes {
        public const int SCOPE = 1;

        public const int UNABLE_TO_INSERT_NEW_ITEM = 1;
        public const int UNABLE_TO_UPDATE_ITEM = 2;
        public const int UNABLE_TO_DELETE_ITEM = 3;
        public const int UNABLE_TO_GET_USER_CREDENTIALS = 4;
        public const int UNABLE_TO_GET_ITEMS = 5;
    }
}
