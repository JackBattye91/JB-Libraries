using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SerialCommunication.I2C {
    public class ErrorCodes {
        public const int SCOPE = 5;

        public const int GET_DEVICES_FAILED = 1;
        public const int ADD_DEVICE_FAILED = 2;
        public const int READ_DATA_FAILED = 3;
        public const int WRITE_DATA_FAILED = 4;
        public const int NO_DATA_RECIEVED = 5;
        public const int UNABLE_TO_PARSE_ADDRESS = 7;
        public const int INVALID_DEVICE = 8;
    }
}
