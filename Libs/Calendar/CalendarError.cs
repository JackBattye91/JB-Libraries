using JB.Calendar.GoogleCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    internal class CalendarError : JB.Common.IError {
        public int Scope { get; } = ErrorCodes.SCOPE;
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public CalendarError(int code, Exception? ex = null) {
            Code = code;
            Exception = ex;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
