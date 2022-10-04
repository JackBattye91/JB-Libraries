using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather
{
    internal class ErrorCodes
    {
        public const int SCOPE = 3;

        public const int GET_TODAYS_FORCAST_FAILED = 1;
        public const int GET_3DAY_FORCAST_FAILED = 2;
        public const int EXTRACT_FORCAST_FAILED = 3;
    }
}
