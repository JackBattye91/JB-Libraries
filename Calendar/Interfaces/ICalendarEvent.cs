﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar.Interfaces {
    public interface ICalendarEvent {
        DateTime? Start { get; set; }
        DateTime? Finish { get; set; }
    }
}
