using SerialCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication.COM {
    internal class Wrapper : IWrapper {
        public IList<IDevice> GetDevices() {
            throw new NotImplementedException();
        }
    }
}
