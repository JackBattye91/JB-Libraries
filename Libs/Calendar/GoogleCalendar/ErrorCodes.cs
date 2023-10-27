using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.Calendar.GoogleCalendar {
    public class ErrorCodes {
        private const int SCOPE = 1;

        public const int UNABLE_TO_INSERT_NEW_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int UNABLE_TO_UPDATE_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int UNABLE_TO_DELETE_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
        public const int UNABLE_TO_GET_USER_CREDENTIALS = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 4;
        public const int UNABLE_TO_GET_ITEMS = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 5;
    }
}
