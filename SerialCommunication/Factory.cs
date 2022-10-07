using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication {
    public sealed class Factory {
        public static IWrapper CreateSerialCommunicationInstance() {
            return new COM.Wrapper();
        }
    }
}
