using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using KeepyMax.Model;
using Keepener.Keepy.Configuration;
using KeepyMax.Controller.DBControl;


namespace GetMBDataLibrary
{
    public partial class Form1 : Form
    {
        List<GeneralConfig> metersSettings;
        PortSettings portSettings;
        List<MBModelsL> parametersMBL;
        string backupFolder;
        string settingsFolder = string.Empty;
        string csvFilePath = string.Empty;
        string[] row = new string[] { };
        List<ModbusMapValues> mbmvL = new List<ModbusMapValues>();
        List<MeterDataReaded> data = new List<MeterDataReaded>();
        string jsonFile = "MeterModbus.json";

        public Form1()
        {
            InitializeComponent();
            cmdProcess.Enabled = false;
        }


        private void WorkingDirectory()
        {
            try
            {
                fbdCSV.Description = "Selecciona directorio de trabajo";
                if (fbdCSV.ShowDialog() == DialogResult.OK)
                {
                    settingsFolder = fbdCSV.SelectedPath + "\\";
                    txtSettingsPath.Text = settingsFolder;

                    System.IO.Directory.CreateDirectory(fbdCSV.SelectedPath + "\\csvFiles");
                    System.IO.Directory.CreateDirectory(fbdCSV.SelectedPath + "\\jsonFiles");
                    System.IO.Directory.CreateDirectory(fbdCSV.SelectedPath + "\\ListeningsBackup");
                }
            }
            catch
            {
                MessageBox.Show("Seleccionar archivo correcto");
            }
        }

        private void LoadData()
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            
            int cntR = 0;
            int cntR1 = 0;
            int cntR2 = 0;

            dgvData.Rows.Clear();

            if (lines.Length > 0 && !string.IsNullOrEmpty(lines[0]))
            {
                dgvData.ColumnCount = 5;

                dgvData.Columns[0].Name = "ParameterID";
                dgvData.Columns[1].Name = "Name";
                dgvData.Columns[2].Name = "MBRegID";
                dgvData.Columns[3].Name = "Multiplier";
                dgvData.Columns[4].Name = "Value";

                foreach (var kwh in lines)
                {
                    ModbusMapValues mbmv = new ModbusMapValues();

                    if (cntR > 0)
                    {
                        var values = kwh.Split(',');

                        if (int.Parse(values[8]) == 1 || values[8] != "")
                        {
                            row = new string[] { values[2], values[3], values[4], values[6], "0" };
                            dgvData.Rows.Add(row);

                            mbmv.ParameterID = int.Parse(values[2]);
                            mbmv.Name = values[3];
                            mbmv.MBRegID = int.Parse(values[4]);
                            mbmv.Multiplier = float.Parse(values[6]);

                            mbmvL.Add(mbmv);

                        }
                    }

                    cntR++;

                }

            }


            //************************************//
            if (lines.Length > 0 && !string.IsNullOrEmpty(lines[0]))
            {
                List<MBModelsL> MBMeterJson = new List<MBModelsL>();
                List<MBModelL> MBModel = new List<MBModelL>();


                //*****************************//
                //**Measurement data type = 1**//
                List<MBRegisters> mbrL1 = new List<MBRegisters>();
                MBModelL mbmL = new MBModelL();
                foreach (var rowMB in lines)
                {
                    if (cntR1 > 0)
                    {
                        var values = rowMB.Split(',');

                        if (int.Parse(values[7]) == 1 && int.Parse(values[8]) == 1)
                        {
                            MBRegisters mbr = new MBRegisters();

                            mbr.ParameterID = int.Parse(values[2]);
                            mbr.MBRegID = int.Parse(values[4]);
                            mbr.DataTypeId = int.Parse(values[5]);
                            mbr.Multiplier = float.Parse(values[6]);

                            mbrL1.Add(mbr);
                        }
                    }
                    
                    cntR1++;

                }

                if (mbrL1.Count > 0)
                {
                    mbmL.MeasureTypeId = 1;
                    mbmL.mbregisters = mbrL1;
                    MBModel.Add(mbmL);
                }

                //*****************************//
                //**Measurement data type = 3**//
                cntR1 = 0;
                List<MBRegisters> mbrL3 = new List<MBRegisters>();
                MBModelL mbmL3 = new MBModelL();
                foreach (var rowMB in lines)
                {
                    if (cntR1 > 0)
                    {
                        var values = rowMB.Split(',');

                        if (int.Parse(values[7]) == 3 && int.Parse(values[8]) == 1)
                        {
                            MBRegisters mbr = new MBRegisters();

                            mbr.ParameterID = int.Parse(values[2]);
                            mbr.MBRegID = int.Parse(values[4]);
                            mbr.DataTypeId = int.Parse(values[5]);
                            mbr.Multiplier = float.Parse(values[6]);

                            mbrL3.Add(mbr);
                        }
                    }

                    cntR1++;

                }

                if (mbrL3.Count > 0)
                {
                    mbmL3.MeasureTypeId = 3;
                    mbmL3.mbregisters = mbrL3;
                    MBModel.Add(mbmL3);
                }

                /////////////////////////////////////////////////////////////

                //*****************************//
                //**Measurement data type = 1**//

                cntR2 = 0;
                List<MBRegisters> mbrL4 = new List<MBRegisters>();
                MBModelL mbmL4 = new MBModelL();
                foreach (var rowMB in lines)
                {
                    if (cntR2 > 0)
                    {
                        var values = rowMB.Split(',');

                        if (int.Parse(values[7]) == 4 && int.Parse(values[8]) == 1)
                        {
                            MBRegisters mbr = new MBRegisters();

                            mbr.ParameterID = int.Parse(values[2]);
                            mbr.MBRegID = int.Parse(values[4]);
                            mbr.DataTypeId = int.Parse(values[5]);
                            mbr.Multiplier = float.Parse(values[6]);

                            mbrL4.Add(mbr);
                        }
                    }

                    cntR2++;

                }

                if (mbrL4.Count > 0)
                {
                    mbmL4.MeasureTypeId = 4;
                    mbmL4.mbregisters = mbrL4;
                    MBModel.Add(mbmL4);
                }


                ////////////////////////////////////////////////////////////

                if (MBModel.Count > 0)
                {
                    MBModelsL mbModL = new MBModelsL();
                   
                    string RowName = lines[1];
                    var Nam = RowName.Split(',');

                    mbModL.mbList = MBModel;
                    mbModL.model = Nam[1];

                    MBMeterJson.Add(mbModL);

                    string filename = settingsFolder + "jsonFiles\\" + jsonFile + ".json";
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(MBMeterJson);
                    System.IO.File.Delete(filename);
                    System.IO.File.AppendAllText(filename, json);
                }
            }
        }

        private void ReadMBData()
        {
            try
            {
                int Timestamp = ConvertToUnixTimestamp(DateTime.Now) - DateTime.Now.Second;
                MBDataProcess mbdp = new MBDataProcess(this.metersSettings, portSettings, parametersMBL);

                data = mbdp.ReadMeters(Timestamp);

                if (data.Count > 0)
                {
                    SendDataToService(data);
                    RefillData();
                    
                }
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
            }
        }

        private void RefillData()
        {
            try
            {
                dgvData.Rows.Clear();
                dgvData.Refresh();

                if (mbmvL.Count > 0)
                {
                    foreach (var dLV in data)
                    {
                        foreach (var vmbD in dLV.MBDataReadedValuesL)
                        {
                            foreach (var mbR in mbmvL)
                            {
                                if (mbR.ParameterID == vmbD.MeterParameters.ParameterId)
                                {
                                    row = new string[] { mbR.ParameterID.ToString(), mbR.Name, mbR.MBRegID.ToString(), mbR.Multiplier.ToString(), vmbD.ListeningValue.ToString() };
                                    dgvData.Rows.Add(row);
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void SendDataToService(List<MeterDataReaded> ml)
        {
            if (ml.Count < 1)
            {
                return;
            }
            try
            {
                if (ml != null && ml.Count > 0)
                {
                    List<MeterListening> listenings = new List<MeterListening>();

                    foreach (var mlL in ml)
                    {
                        if (mlL.MBDataReadedValuesL.Count > 0)
                        {
                            MeterListening listening = new MeterListening
                            {
                                ListeningDate = UnixTimeStampToDateTime(mlL.Timestamp),
                                ListeningID = mlL.ListeningGuid,
                                MeterSerial = mlL.GeneralConfig.DeviceIdGuid,
                                ListeningValue = new List<ListeningValue>()
                            };

                            //Obtener lista de parámetros guardados
                            var lv = mlL.MBDataReadedValuesL;

                            foreach (var lvL in lv)
                            {
                                if (lvL.MeasurType.MeasurTypeId == 1 || lvL.MeasurType.MeasurTypeId == 3)
                                {
                                    listening.ListeningValue.Add(new ListeningValue
                                    {
                                        Index = (short)lvL.MeterParameters.ParameterId,
                                        Value = lvL.ListeningValue
                                    });
                                }
                            }

                            listenings.Add(listening);
                        }
                    }

                    if (listenings.Count > 0)
                    {
                        try
                        {
                            SaveListeningsFile(listenings);
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        
        private void SaveListeningsFile(List<MeterListening> listenings)
        {
            string filename = string.Format("{0}{1:ddMMyyyy_HHmmss.ffffff}.json", backupFolder, DateTime.Now);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(listenings);
            System.IO.File.AppendAllText(filename, json);
        }
        
        private void cmdProcess_Click(object sender, EventArgs e)
        {
            LoadInfoData();

            dgvData.Rows.Clear();
            dgvData.Refresh();

            if (rbJsontoCSV.Checked == true)
            {
                if (txtSettingsPath.Text != string.Empty)
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = txtSettingsPath.Text + "\\csvFiles";
                        openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Get the path of specified file
                            csvFilePath = openFileDialog.FileName;

                            jsonFile = System.IO.Path.GetFileNameWithoutExtension(csvFilePath);

                            LoadData();

                        }
                    }
                }
            }

            if (rbMB.Checked)
            {
                ReadMBData();
            }

        }

        private static int ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (int)Math.Floor(diff.TotalSeconds);
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        private void cmdDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                //select working directory
                WorkingDirectory();
                LoadInfoData();
            }
            catch
            {
                MessageBox.Show("Error: seleccionar ruta correcta que contenga los archivos de configuración JSON");
            }
        }

        private void LoadInfoData()
        {
            try
            {
                if (settingsFolder != string.Empty)
                {
                    backupFolder = settingsFolder + "ListeningsBackup\\";
                    metersSettings = Json.Read<List<GeneralConfig>>(settingsFolder + "GetGeneralConfig.json");
                    portSettings = Json.Read<PortSettings>(settingsFolder + "SerialPort.json");
                    parametersMBL = Json.Read<List<MBModelsL>>(settingsFolder + "MeterModbus.json");

                    cmdProcess.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Formato de datos incorrecto.");
            }
        }

    }
}
