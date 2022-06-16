using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    public interface IWrapper {
        public Task<JB.Common.Errors.IReturnCode<IList<Interfaces.ICalendarEvent>>> GetEvents();
        public Task<JB.Common.Errors.IReturnCode<bool>> AddEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
        public Task<JB.Common.Errors.IReturnCode<bool>> UpdateEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
        public Task<JB.Common.Errors.IReturnCode<bool>> CancelEvent(string pEventId, string pCalendarId);
    }
}
