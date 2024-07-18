using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SerialCommunication.Interfaces {
    public interface IDevice {
        string Address { get; set; }
        string Name { get; set; }
    }
}
