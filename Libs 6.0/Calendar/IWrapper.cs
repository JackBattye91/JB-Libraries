using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    public interface IWrapper {
        public Task<JB.Common.ReturnCode<IList<Interfaces.ICalendarEvent>>> GetEvents();
        public Task<JB.Common.ReturnCode<bool>> AddEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
        public Task<JB.Common.ReturnCode<bool>> UpdateEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
        public Task<JB.Common.ReturnCode<bool>> CancelEvent(string pEventId, string pCalendarId);
    }
}
