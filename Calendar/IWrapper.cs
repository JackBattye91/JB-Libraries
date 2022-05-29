using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar {
    public interface IWrapper {
        public JB.Common.IReturnCode<bool> GetCalendarEvents();
    }
}
