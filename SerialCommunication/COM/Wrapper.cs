using JB.Common;
using JB.SerialCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;

namespace JB.SerialCommunication.COM {
    internal class Wrapper : IWrapper {
        IReturnCode<IList<IDevice>> IWrapper.GetDevices() {
            IReturnCode<IList<IDevice>> rc = new ReturnCode<IList<IDevice>>();
            IList<IDevice> devicesList = new List<IDevice>();

            try {
                string[] portNames = SerialPort.GetPortNames();

                foreach (string name in portNames) {
                    IReturnCode<IDevice> addDeviceRc = AddDevice(name, name);

                    if (addDeviceRc.Success && addDeviceRc.Data != null) {
                        devicesList.Add(addDeviceRc.Data);
                    }
                    if (addDeviceRc.Failed) {
                        ErrorWorker.CopyErrors(addDeviceRc, rc);
                    }
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
            IReturnCode<IDevice> rc = new ReturnCode<IDevice>();
            IDevice device = new Models.Device();
            
            try {
                if (rc.Success) {
                    device = new Models.Device() {
                        Name = pName,
                        Address = pAddress,
                        COMPort = new SerialPort(pAddress, 115200, Parity.None)
                    };

                    ((Models.Device)device).COMPort?.Open();

                    if (((Models.Device)device).COMPort?.IsOpen == false) {
                        rc.Errors.Add(new Error(ErrorCodes.UNABLE_TO_OPEN_PORT));
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new Error(ErrorCodes.ADD_DEVICE_FAILED, ex));
            }

            if (rc.Success) {
                rc.Data = device;
            }

            return rc;
        }

        public IReturnCode<byte[]> ReadData(IDevice pDevice) {
            IReturnCode<byte[]> rc = new ReturnCode<byte[]>();
            byte[] data = new byte[1];

            try {
                if (rc.Success) {
                    if (pDevice is Models.Device) {
                        SerialPort? port = ((Models.Device)pDevice).COMPort;

                        if (port?.IsOpen == false) {
                            port.Open();
                        }

                        string comData = port?.ReadExisting() ?? string.Empty;
                        data = Encoding.UTF8.GetBytes(comData);
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

        public IReturnCode<bool> WriteData(byte[] pData, IDevice pDevice) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try {
                if (rc.Success) {
                    if (pDevice is Models.Device) {
                        SerialPort? port = ((Models.Device)pDevice).COMPort;

                        if (port?.IsOpen == false) {
                            port.Open();
                        }

                        port?.Write(pData, 0, pData.Length);
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new Error(ErrorCodes.WRITE_DATA_FAILED, ex));
            }

            return rc;
        }
    }
}
