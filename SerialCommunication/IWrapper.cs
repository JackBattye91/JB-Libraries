using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication {
    public interface IWrapper {
        IList<Interfaces.IDevice> GetDevices();
    }
}
