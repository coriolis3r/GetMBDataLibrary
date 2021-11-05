using System;
using System.IO.Ports;
using Modbus.Device;

namespace KeepyMax.Model
{
    public class MBConfig
    {
        public MBConfig()
        {
            this.MBStatusM = new MBStatusM();
        }
        public SerialPort SerialPortN { get; set; }
        public IModbusSerialMaster MasterMB { get; set; }
        public MBStatusM MBStatusM { get; set; }

    }
}
