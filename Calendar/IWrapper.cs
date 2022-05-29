using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    public interface IWrapper {
        public Task<JB.Common.IReturnCode<IList<Interfaces.ICalendarEvent>>> GetEvents();
        public Task<JB.Common.IReturnCode<bool>> AddEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
        public Task<JB.Common.IReturnCode<bool>> UpdateEvent(Interfaces.ICalendarEvent pEvent, string pCalendarId);
    }
}
