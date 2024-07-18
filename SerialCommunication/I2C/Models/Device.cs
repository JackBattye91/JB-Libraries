using JB.SerialCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SerialCommunication.I2C.Models {
    internal class Device : IDevice {
        public string Address { get; set; }
        public string Name { get; set; }
        public I2cDevice? I2CDevice { get; set; }

        public Device() {
            Address = Name = string.Empty;
        }
        public Device(string pName, I2cDevice pDevice) {
            I2CDevice = pDevice;
            Address = I2CDevice.ConnectionSettings.DeviceAddress.ToString();
            Name = pName;
        }
    }
}
