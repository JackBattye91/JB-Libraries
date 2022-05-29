using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar.GoogleCalendar.Models {
    internal class CalendarEvent : Interfaces.ICalendarEvent {
        public string Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }

        public CalendarEvent() {
            Id = String.Empty;
        }
    }
}
