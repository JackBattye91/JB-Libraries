using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace JB.Calendar.GoogleCalendar {
    internal class Worker {
        internal static Event Convert(Interfaces.ICalendarEvent pEvent) {
            Event newEvent = new Event() {
                Summary = pEvent.Description,
                Start = new EventDateTime() { DateTime = pEvent.Start },
                End = new EventDateTime() { DateTime = pEvent.Finish }
            };

            return newEvent;
        }
        internal static Interfaces.ICalendarEvent Convert(Event pEvent) {
            Interfaces.ICalendarEvent newEvent = new Models.CalendarEvent() {
                Description = pEvent.Summary,
                Start = pEvent.Start.DateTime,
                Finish = pEvent.End.DateTime
            };

            return newEvent;
        }
    }
}
