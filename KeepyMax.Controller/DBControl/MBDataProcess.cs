using System;
using System.Collections.Generic;
using System.Linq;
using KeepyMax.Model;
using Modbus.Device;
using System.Net.Sockets;
using System.Diagnostics;
using Modbus.Utility;
using System.IO.Ports;
using System.Configuration;
using Keepener.Keepy.Configuration;

namespace KeepyMax.Controller.DBControl
{
    public class MBDataProcess
    {
        SerialPort port;
        IModbusSerialMaster master;
        readonly List<GeneralConfig> _meters;
        Keepener.Keepy.Configuration.PortSettings _portSettings;
        readonly List<MBModelsL> _parametersMBL;


        public MBDataProcess(List<GeneralConfig> meters, Keepener.Keepy.Configuration.PortSettings portSettings, List<MBModelsL> parametersMBL)
        {
            this._meters = meters;
            this._portSettings = portSettings;
            this._parametersMBL = parametersMBL;
        }

        private bool TestSerialPort(string PortName)
        {
            //bool PortStatus = false;
            string[] ports = SerialPort.GetPortNames();
            return Array.Exists(ports, item => item == PortName);
        }

        private SerialPort BuildPort()
        {
            SerialPort result = new SerialPort();

            result.PortName = _portSettings.Name;
            result.BaudRate = _portSettings.BaudRate;
            result.DataBits = Convert.ToInt16("8");
            result.Parity = (Parity)Enum.Parse(typeof(Parity), _portSettings.Parity);
            
            result.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            result.ReadTimeout = 1000;
            result.WriteTimeout = 1000;

            return result;
        }

        private void Open()
        {
            bool PortExist = false;
            port = BuildPort();

            PortExist = TestSerialPort(port.PortName);

            if (PortExist)
            {
                if (!port.IsOpen)
                {
                    port.Open();
                }
            }
            else
            {
                string message = "Error opening " + port.PortName + ". Not Exist.";
                Trace.WriteLine(message + "\n");
                throw new Exception(message);

            }

            master = ModbusSerialMaster.CreateRtu(port);

        }


        public List<MeterDataReaded> ReadMeters(int Timestamp)
        {
            List<MeterDataReaded> CombinedData = new List<MeterDataReaded>();

            if (_meters != null && _meters.Count > 0)
            {
                if (_meters.Count(x => x.ComTypeId == 0) > 0)
                    CombinedData = ReadMBData_RTU(Timestamp, CombinedData);
                if (_meters.Count(x => x.ComTypeId == 1) > 0)
                    CombinedData = ReadMBData_IP(Timestamp, CombinedData);
            }

            return CombinedData;
        }

        
        //Read MB data via serial port
        /**this method should be privete**/
        private List<MeterDataReaded> ReadMBData_RTU(int Timestamp, List<MeterDataReaded> mbdr)
        {
            Open();
            
            try
            {
                ushort[] registers = { };
                ushort ContTry = 0;

                //Get data for each meter and data type
                foreach (GeneralConfig meter in _meters)
                {
                    if (meter.ComTypeId == 0)
                    {
                        List<MBDataReadedValues> mbdrCL = new List<MBDataReadedValues>();

                        Guid listeningId = Guid.NewGuid();

                        MeterDataReaded mdr = new MeterDataReaded
                        {
                            GeneralConfig = new GeneralConfig
                            {
                                DeviceIdGuid = meter.DeviceIdGuid
                            },
                            ListeningGuid = listeningId,
                            Timestamp = Timestamp,
                            DataSendedStatus = 0
                        };

                        foreach (var mbMeter in meter.MeterMBParConfigList)
                        {
                            //if is time to read and save data according to range group
                            int ST_ON = (DateTime.Now.Minute % mbMeter.MeasurMeterList.SampleTime);

                            if (ST_ON == 0)
                            {
                                try
                                {
                                    //get range list
                                    List<MBRange> RangeMeter = new List<MBRange>();
                                    RangeMeter = GetReadRange(mbMeter.MeasurRange.Range);

                                    foreach (var range in RangeMeter)
                                    {
                                        //size of MB registers to read
                                        ushort MBSize = (ushort)(range.EndRange - range.StartRange + 1);

                                        while (ContTry < 2 && (registers.Length != MBSize))
                                        {
                                            if (mbMeter.Models.MBFunctionId == 3)
                                                registers = master.ReadHoldingRegisters((byte)meter.NCPU, (ushort)range.StartRange, MBSize);
                                            if (mbMeter.Models.MBFunctionId == 4)
                                                registers = master.ReadInputRegisters((byte)meter.NCPU, (ushort)range.StartRange, MBSize);

                                            ContTry++;
                                        }

                                        ContTry = 0;

                                        if (registers.Length == MBSize)
                                            mbdrCL = GetParametersMeter(registers, range, mbdrCL, meter.NCPU, listeningId.ToString(), meter.ModelName, mbMeter.MeasurRange.MeasurType.MeasurTypeId);
                                        else
                                            Trace.WriteLine("Range error: " + meter.NCPU + ", " + range.StartRange + "-" + range.EndRange + ". Hora:" + DateTime.Now.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Error: Meter is disconnected
                                    Trace.WriteLine("No meter." + meter.NCPU + " :" + DateTime.Now.ToString() + Environment.NewLine);
                                    Trace.WriteLine(ex.ToString().Substring(0, 120));
                                    //Trace.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_portSettings));
                                    break;
                                }
                            }
                        }

                        mdr.MBDataReadedValuesL = mbdrCL.OrderBy(x => x.MeterParameters.ParameterId).ToList();

                        mbdr.Add(mdr);
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error en lecturas RTU.");
                Trace.WriteLine(ex.ToString().Substring(0,120));
            }
            finally
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
            }

            return mbdr;
        }
        
        private List<MeterDataReaded> ReadMBData_IP(int Timestamp, List<MeterDataReaded> mbdr)
        {
            TcpClient client = new TcpClient();
            ModbusIpMaster masterIP = null;

            try
            {
                ushort[] registers = { };
                ushort ContTry = 0;

                //Get data for each meter and data type
                foreach (GeneralConfig meter in _meters)
                {
                    if (meter.ComTypeId == 1 && meter.IPAddress != "")
                    {
                        client = new TcpClient(meter.IPAddress, 502);
                        masterIP = ModbusIpMaster.CreateIp(client);

                        List<MBDataReadedValues> mbdrCL = new List<MBDataReadedValues>();

                        Guid listeningId = Guid.NewGuid();

                        MeterDataReaded mdr = new MeterDataReaded
                        {
                            GeneralConfig = new GeneralConfig
                            {
                                DeviceIdGuid = meter.DeviceIdGuid
                            },
                            ListeningGuid = listeningId,
                            Timestamp = Timestamp,
                            DataSendedStatus = 0
                        };

                        foreach (var mbMeter in meter.MeterMBParConfigList)
                        {
                            //if is time to read and save data according to range group
                            int ST_ON = (DateTime.Now.Minute % mbMeter.MeasurMeterList.SampleTime);

                            if (ST_ON == 0)
                            {
                                try
                                {
                                    //get range list
                                    List<MBRange> RangeMeter = new List<MBRange>();
                                    RangeMeter = GetReadRange(mbMeter.MeasurRange.Range);

                                    foreach (var range in RangeMeter)
                                    {
                                        //size of MB registers to read
                                        ushort MBSize = (ushort)(range.EndRange - range.StartRange + 1);

                                        while (ContTry < 3 && (registers.Length != MBSize))
                                        {
                                            if (mbMeter.Models.MBFunctionId == 3)
                                                registers = masterIP.ReadHoldingRegisters((byte)meter.NCPU, (ushort)range.StartRange, MBSize);
                                            if (mbMeter.Models.MBFunctionId == 4)
                                                registers = masterIP.ReadInputRegisters((byte)meter.NCPU, (ushort)range.StartRange, MBSize);

                                            ContTry++;

                                        }

                                        ContTry = 0;

                                        if (registers.Length == MBSize)
                                            mbdrCL = GetParametersMeter(registers, range, mbdrCL, meter.NCPU, listeningId.ToString(), meter.ModelName, mbMeter.MeasurRange.MeasurType.MeasurTypeId);
                                        else
                                            //read range again
                                            Trace.WriteLine("Range error: " + meter.NCPU + ", " + range.StartRange + "-" + range.EndRange + ". Hora:" + DateTime.Now.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Error: Meter is disconnected
                                    Trace.WriteLine("Error reading IP meter: " + meter.NCPU + " :" + DateTime.Now.ToString() + Environment.NewLine);
                                    break;
                                }
                            }
                        }

                        mdr.MBDataReadedValuesL = mbdrCL.OrderBy(x => x.MeterParameters.ParameterId).ToList();

                        mbdr.Add(mdr);

                        masterIP.Dispose();
                        client.Close();

                        System.Threading.Thread.Sleep(150);

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error en lecturas IP: " + ex.ToString().Substring(0,120));
            }
            finally
            {
                masterIP.Dispose();

                if(client.Connected)
                    client.Close();
            }

            return mbdr;
        }
        
        private List<MBRange> GetReadRange(string ranges)
        {
            List<MBRange> range = new List<MBRange>();

            string[] rangeL = ranges.Split(';');

            foreach (string rangosD in rangeL)
            {
                MBRange RangUnit = new MBRange();
                string[] rgo = rangosD.Split(',');
                if (rgo.Length == 2)
                {
                    RangUnit.StartRange = int.Parse(rgo[0]);
                    RangUnit.EndRange = int.Parse(rgo[1]);
                }
                range.Add(RangUnit);
            }

            return range;
        }

        private List<MBDataReadedValues> GetParametersMeter(ushort[] registers, MBRange range, List<MBDataReadedValues> mbdrC, int MeterId, string ListeningGuid, string modelName, int measurTypeId)
        {
            int MBIndex = 0;

            var mbml = _parametersMBL.Find(x => x.model == modelName).mbList;

            var mpl = mbml.Find(x => x.MeasureTypeId == measurTypeId).mbregisters;

            while (MBIndex < registers.Length)
            {
                MBDataReadedValues mbdrv = new MBDataReadedValues();

                var mbp = mpl.Find(x => x.MBRegID == range.StartRange + MBIndex);

                if (mbp != null)
                {
                    switch (mbp.DataTypeId)
                    {
                        case 1://ushort
                            mbdrv.ListeningValue = registers[MBIndex] * mbp.Multiplier;
                            MBIndex++;
                            break;
                        case 2://float
                            mbdrv.ListeningValue = ModbusUtility.GetSingle(registers[MBIndex], registers[MBIndex + 1]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;
                        case 3://uint
                            mbdrv.ListeningValue = ModbusUtility.GetUInt32(registers[MBIndex], registers[MBIndex + 1]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;
                        case 4://int
                            mbdrv.ListeningValue = ModbusUtility.GetInt32(registers[MBIndex], registers[MBIndex + 1]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;
                        case 5://ulong
                            mbdrv.ListeningValue = ModbusUtility.GetLong(registers[MBIndex], registers[MBIndex + 1], registers[MBIndex + 2], registers[MBIndex + 3]) * mbp.Multiplier;
                            MBIndex += 4;
                            break;
                        case 6://long
                            mbdrv.ListeningValue = ModbusUtility.GetLong(registers[MBIndex], registers[MBIndex + 1], registers[MBIndex + 2], registers[MBIndex + 3]) * mbp.Multiplier;
                            MBIndex += 4;
                            break;
                        case 7://Mod10 (For PM800 Schneider)
                            mbdrv.ListeningValue = (UInt64)(registers[MBIndex + 3] * 10E11 + registers[MBIndex + 2] * 10E7 + registers[MBIndex + 1] * 10E3 + registers[MBIndex]);
                            MBIndex += 4;
                            break;
                        case 8://Agreagate GWh + KWh
                            mbdrv.ListeningValue = (Int64)(registers[MBIndex + 1] / 1e6 + registers[MBIndex] / 1e3);
                            MBIndex += 2;
                            break;
                        case 9://Double
                            mbdrv.ListeningValue = (float)ModbusUtility.GetDouble(registers[MBIndex], registers[MBIndex + 1], registers[MBIndex + 2], registers[MBIndex + 3]) * mbp.Multiplier;
                            MBIndex += 4;
                            break;
                        case 10://Double: special for PAC3200
                            double Val1 = ModbusUtility.GetDouble(registers[MBIndex], registers[MBIndex + 1], registers[MBIndex + 2], registers[MBIndex + 3]) * mbp.Multiplier;
                            double Val2 = ModbusUtility.GetDouble(registers[MBIndex + 4], registers[MBIndex + 5], registers[MBIndex + 6], registers[MBIndex + 7]) * mbp.Multiplier;
                            mbdrv.ListeningValue = (float)(Val1 + Val2);
                            MBIndex += 8;
                            break;
                        case 11://int inverted (SATEC PM130E)
                            mbdrv.ListeningValue = ModbusUtility.GetInt32(registers[MBIndex + 1], registers[MBIndex]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;
                        case 12:
                            mbdrv.ListeningValue = (UInt64)(registers[MBIndex + 1] * 10000 + registers[MBIndex]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;
                        case 13://uint
                            mbdrv.ListeningValue = ModbusUtility.GetUInt32(registers[MBIndex + 1], registers[MBIndex]) * mbp.Multiplier;
                            MBIndex += 2;
                            break;

                        default:
                            MBIndex++;
                            break;
                    }
                    mbdrv.ListeningGuid = ListeningGuid;
                    mbdrv.MeasurType.MeasurTypeId = measurTypeId;
                    mbdrv.MeterParameters.ParameterId = mbp.ParameterID;
                    mbdrC.Add(mbdrv);
                }
                else
                    MBIndex++;
            }

            return mbdrC;

        }
    }
}
