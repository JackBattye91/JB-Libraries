using JB.SerialCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace JB.SerialCommunication.COM.Models {
    internal class Device : IDevice {
        public string Address { get; set; }
        public string Name { get; set; }
        public SerialPort? COMPort { get; set; }

        public Device() {
            Address = Name = string.Empty;
        }
        public Device(string pName, SerialPort pDevice) {
            COMPort = pDevice;
            Address = COMPort.PortName;
            Name = pName;
        }
    }
}
