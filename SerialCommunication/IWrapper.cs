using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using SerialCommunication.Interfaces;

namespace SerialCommunication {
    public interface IWrapper {
        IReturnCode<IList<Interfaces.IDevice>> GetDevices();
        IReturnCode<Interfaces.IDevice> AddDevice(string pAddress);
        IReturnCode<bool> WriteData(byte[] pData, IDevice pDevice);
        IReturnCode<byte[]> ReadData(IDevice pDevice);
    }
}
