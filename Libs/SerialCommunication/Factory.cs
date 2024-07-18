using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SerialCommunication {
    public sealed class Factory {
        public static IWrapper CreateDefaultSerialCommunicationInstance() {
            return new COM.Wrapper();
        }
        public static IWrapper CreateI2CSerialCommunicationInstance() {
            return new I2C.Wrapper();
        }
        public static IWrapper CreateCOMSerialCommunicationInstance() {
            return new COM.Wrapper();
        }
    }
}
