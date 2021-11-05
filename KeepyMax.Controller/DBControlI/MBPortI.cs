using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepyMax.Model;
using System.IO.Ports;

namespace KeepyMax.Controller.DBControlI
{
    public interface MBPortI
    {
        MBStatusM OpenSerialPort();
        MBStatusM CloseSerialPort(SerialPort ComPort);
    }
}
