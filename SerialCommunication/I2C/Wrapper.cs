using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.I2c;
using JB.SerialCommunication.I2C.Models;
using JB.Common;
using JB.SerialCommunication.Interfaces;

namespace JB.SerialCommunication.I2C {
    internal class Wrapper : IWrapper {
        public IReturnCode<IList<IDevice>> GetDevices() {
            IReturnCode<IList<IDevice>> rc = new ReturnCode<IList<IDevice>>();
            IList<IDevice> devicesList = new List<IDevice>();

            try {
                I2cBus bus = I2cBus.Create(1);

                for(int d = 0; d < 127; d++) {
                    I2cDevice device = bus.CreateDevice(d);
                    device.WriteByte(0);
                }

            }
            catch (Exception ex) {
                rc.AddError(new Error(ex));
            }

            if (rc.Success) {
                rc.Data = devicesList;
            }

            return rc;
        }

        public IReturnCode<IDevice> AddDevice(string pName, string pAddress) {
            JB.Common.IReturnCode<IDevice> rc = new JB.Common.ReturnCode<IDevice>();
            IDevice? device = null;
            int address = 0;

            try {
                if (rc.Success) {
                    if (false == int.TryParse(pAddress, out address)) {
                        rc.AddError(new Error(new JB.Common.Errors.JBException("Unable to parse I2C address")));
                    }
                }

                if (rc.Success) {
                    I2cBus bus = I2cBus.Create(1);
                    device = new Device(pName, bus.CreateDevice(address));
                }
            }
            catch(Exception ex) {
                rc.AddError(new Error(ex));
            }

            if (rc.Success) {
                rc.Data = device;
            }

            return rc;
        }

        public IReturnCode<bool> WriteData(byte[] pData, IDevice pDevice) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try {
                if (rc.Success) {
                    if (pDevice is Device) {
                        ((Device)pDevice).I2CDevice?.Write(pData);
                    }
                    else {
                        rc.AddError(new Error(new JB.Common.Errors.JBException("Incorrect Device type")));
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new Error(ex));
            }

            return rc;
        }

        public IReturnCode<byte[]> ReadData(IDevice pDevice) {
            IReturnCode<byte[]> rc = new ReturnCode<byte[]>();
            byte[]? data = null;

            try {
                if (rc.Success) {
                    if (pDevice is Device) {
                        ((Device)pDevice).I2CDevice?.Read(data);

                        if (data?.Length == 0) {
                            rc.AddError(new Error(new JB.Common.Errors.JBException("Unable to read data")));
                        }
                    }
                    else {
                        rc.AddError(new Error(new JB.Common.Errors.JBException("Incorrent Device type")));
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new Error(ex));
            }

            if (rc.Success) {
                rc.Data = data;
            }

            return rc;
        }
    }
}
