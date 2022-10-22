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
                rc.Errors.Add(new Error(ErrorCodes.GET_DEVICES_FAILED, ex));
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
                        rc.Errors.Add(new Error(ErrorCodes.UNABLE_TO_PARSE_ADDRESS));
                    }
                }

                if (rc.Success) {
                    I2cBus bus = I2cBus.Create(1);
                    device = new Device(pName, bus.CreateDevice(address));
                }
            }
            catch(Exception ex) {
                rc.Errors.Add(new Error(ErrorCodes.ADD_DEVICE_FAILED, ex));
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
                        rc.Errors.Add(new Error(ErrorCodes.INVALID_DEVICE));
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new Error(ErrorCodes.WRITE_DATA_FAILED, ex));
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
                            rc.Errors.Add(new Error(ErrorCodes.NO_DATA_RECIEVED));
                        }
                    }
                    else {
                        rc.Errors.Add(new Error(ErrorCodes.INVALID_DEVICE));
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new Error(ErrorCodes.READ_DATA_FAILED, ex));
            }

            if (rc.Success) {
                rc.Data = data;
            }

            return rc;
        }
    }
}
