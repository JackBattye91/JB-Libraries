using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    public class Factory {
        public static IWrapper CreateCalendarWrapper() {
            return new GoogleCalendar.Wrapper();
        }
    }
}
