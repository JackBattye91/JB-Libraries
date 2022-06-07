using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Tests.Models {
    internal class CalendarEvent : JB.Calendar.Interfaces.ICalendarEvent {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }

        public CalendarEvent() {
            Id = String.Empty;
            Description = String.Empty;
        }
    }
}
