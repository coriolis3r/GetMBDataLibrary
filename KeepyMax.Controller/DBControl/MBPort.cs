using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepyMax.Model;
using System.IO.Ports;
using System.Diagnostics;
using KeepyMax.Controller.DBControlI;
using Modbus.Device;
using System.Configuration;

namespace KeepyMax.Controller.DBControl
{
    public class MBPort : MBPortI
    {
        MBConfig MBPortP;

        //public MBConfig StartMBPort()
        //{
        //    //SerialCom SerCom = new SerialCom();

        //    MBPortP = new MBConfig();
        //    SerialPort sp = new SerialPort();

        //    MBPortP.SerialPortN = sp;

        //    try
        //    {
        //        ConfigData cd = new ConfigData();
        //        //SerCom = cd.GetSerialCom();

        //        if (!MBPortP.SerialPortN.IsOpen)
        //        {
        //            //Assign desired settings to the serial port:
        //            MBPortP.SerialPortN.PortName = ConfigurationManager.AppSettings["port"];
        //            MBPortP.SerialPortN.BaudRate = Convert.ToInt32(ConfigurationManager.AppSettings["Baudrate"]);
        //            MBPortP.SerialPortN.DataBits = Convert.ToInt16("8");
        //            MBPortP.SerialPortN.Parity = (Parity)Enum.Parse(typeof(Parity), ConfigurationManager.AppSettings["Parity"]);
        //            MBPortP.SerialPortN.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");

        //            //These timeouts are default and cannot be editted through the class at this point:
        //            MBPortP.SerialPortN.ReadTimeout = 1000;
        //            MBPortP.SerialPortN.WriteTimeout = 1000;

        //            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(MBPortP.SerialPortN);

        //            MBPortP.MasterMB = master;

        //            MBPortP.MBStatusM.MBStatusName = MBPortP.SerialPortN.PortName + " created successfully";
        //            MBPortP.MBStatusM.MBStatus = true;
        //        }

        //        else
        //        {
        //            MBPortP.MBStatusM.MBStatusName = MBPortP.SerialPortN.PortName + " already created";
        //            MBPortP.MBStatusM.MBStatus = false;
        //        }

        //        Trace.WriteLine(MBPortP.MBStatusM.MBStatusName + "\n");

        //        return MBPortP;

        //    }
        //    catch (Exception err)
        //    {
        //        MBPortP.MBStatusM.MBStatusName = "Error creating serial port: " + MBPortP.SerialPortN.PortName + ": " + err.Message;
        //        MBPortP.MBStatusM.MBStatus = false;
        //        Trace.WriteLine(MBPortP.MBStatusM.MBStatusName + "\n");
        //        return MBPortP;
        //    }
        //}

        public MBStatusM OpenSerialPort()
        {
            MBStatusM mbs = new MBStatusM();

            bool PortExist = false;

            try
            {
                PortExist = TestSerialPort(MBPortP.SerialPortN.PortName.ToString());

                if (PortExist)
                {
                    if (!MBPortP.SerialPortN.IsOpen)
                    {
                        MBPortP.SerialPortN.Open();
                        mbs.MBStatus = true;
                        mbs.MBStatusName = MBPortP.SerialPortN.ToString() + " opened successfully";
                    }
                    else
                    {
                        mbs.MBStatus = false;
                        mbs.MBStatusName = MBPortP.SerialPortN.ToString() + " already opened";
                    }
                }
                else if (!PortExist)
                {
                    mbs.MBStatusName = "Error opening " + MBPortP.SerialPortN.PortName.ToString() + ". Not Exist.";
                    mbs.MBStatus = false;
                    Trace.WriteLine(mbs.MBStatusName + "\n");
                }
                return mbs;
            }

            catch (Exception err)
            {
                mbs.MBStatusName = "Error opening " + MBPortP.SerialPortN.PortName.ToString() + ": " + err.Message;
                mbs.MBStatus = false;
                Trace.WriteLine(mbs.MBStatusName + "\n");
                return mbs;
            }
        }

        public MBStatusM CloseSerialPort(SerialPort ComPort)
        {
            MBStatusM MBSt = new MBStatusM();

            try
            {
                if (ComPort.IsOpen)
                {
                    ComPort.Close();
                    MBSt.MBStatus = true;
                    MBSt.MBStatusName = ComPort.PortName.ToString() + " closed successfully";
                }
                else
                {
                    MBSt.MBStatus = false;
                    MBSt.MBStatusName = ComPort.PortName.ToString() + " already closed";
                }
                return MBSt;
            }

            catch (Exception err)
            {
                MBSt.MBStatusName = "Error closing " + ComPort.PortName.ToString() + ": " + err.Message;
                MBSt.MBStatus = false;
                Trace.WriteLine(MBSt.MBStatusName + "\n");
                return MBSt;
            }
        }

        private bool TestSerialPort(string PortName)
        {
            //bool PortStatus = false;
            string[] ports = SerialPort.GetPortNames();

            return Array.Exists(ports, item => item == PortName);

        }
    }
}
