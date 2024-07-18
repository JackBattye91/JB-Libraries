using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using JB.SerialCommunication.Interfaces;

namespace JB.SerialCommunication {
    public interface IWrapper {
        IReturnCode<IList<Interfaces.IDevice>> GetDevices();
        IReturnCode<Interfaces.IDevice> AddDevice(string pName, string pAddress);
        IReturnCode<bool> WriteData(byte[] pData, IDevice pDevice);
        IReturnCode<byte[]> ReadData(IDevice pDevice);
    }
}
