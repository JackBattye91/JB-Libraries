using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.BBC_Weather {
    internal class ErrorCodes
    {
        private const int SCOPE = 3;

        public const int GET_TODAYS_FORCAST_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int GET_3DAY_FORCAST_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int EXTRACT_FORCAST_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
    }
}
