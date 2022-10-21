using JB.Common;
using SerialCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication.COM {
    internal class Wrapper : IWrapper {
        public IReturnCode<IDevice> AddDevice(string pAddress) {
            throw new NotImplementedException();
        }

        public IReturnCode<byte[]> ReadData(IDevice pDevice) {
            throw new NotImplementedException();
        }

        public IReturnCode<bool> WriteData(byte[] pData, IDevice pDevice) {
            throw new NotImplementedException();
        }

        IReturnCode<IList<IDevice>> IWrapper.GetDevices() {
            throw new NotImplementedException();
        }
    }
}
