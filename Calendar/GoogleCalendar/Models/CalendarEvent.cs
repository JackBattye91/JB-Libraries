using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar.GoogleCalendar.Models {
    internal class CalendarEvent : Interfaces.ICalendarEvent {
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
    }
}
