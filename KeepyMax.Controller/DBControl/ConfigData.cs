using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using KeepyMax.Model;
using KeepyMax.Controller.DBControlI;
using DeviceDAO.Base;
using DeviceDAO;
using KeepyMax.Controller.DataAccess;


namespace KeepyMax.Controller.DBControl
{
    public class ConfigData //: DataAccesBase// ConfigDataI
    {
        public BaseDAO DaoBase { get; set; }
        //public List<GeneralConfig> GetGeneralConfig(int ComTypeId)
        //{
        //    BaseDAO baseDAOSupport = null;
        //    DbDataReader dbReader = null;
        //    List<GeneralConfig> gcl = new List<GeneralConfig>();

        //    try
        //    {
        //        baseDAOSupport = DaoBase;
        //        baseDAOSupport = BaseDAO.Instance;

        //        baseDAOSupport.Conectar();

        //        if (ComTypeId == 0)
        //            baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetAllConfigData"));
        //        else
        //        {
        //            baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetConfigData"));
        //            baseDAOSupport.AsignarParametroEntero("@ComTypeId", ComTypeId);
        //        }

        //        dbReader = baseDAOSupport.EjecutarConsulta();

        //        if (dbReader.HasRows)
        //        {
        //            while (dbReader.Read())
        //            {
        //                GeneralConfig gc = new GeneralConfig();

        //                gc.DeviceIdGuid = Convert.ToString(CommonUtils.validaCampo(dbReader["DeviceIdGuid"], null)).Trim();
        //                gc.MeterId = Convert.ToInt16(CommonUtils.validaCampo(dbReader["MeterId"], CommonUtils.defaultEntero));
        //                gc.NCPU = Convert.ToInt16(CommonUtils.validaCampo(dbReader["NCPU"], CommonUtils.defaultEntero));
        //                gc.IPAddress = Convert.ToString(CommonUtils.validaCampo(dbReader["IPAddress"], null)).Trim();
                        
        //                gcl.Add(gc);
        //            }
        //        }
        //        return gcl;
        //    }
        //    catch (Exception bde)
        //    {
        //        System.Diagnostics.Trace.WriteLine("Error al obtener configuración de medidor: " + bde.ToString() + "\n");
        //        throw new DBException("Error en Acceso a Base de Datos ");
        //    }

        //    finally
        //    {
        //        if (baseDAOSupport != null)
        //        {
        //            try
        //            {
        //                baseDAOSupport.Desconectar();
        //            }
        //            catch (Exception bde)
        //            {
        //                System.Diagnostics.Trace.WriteLine("Error al intentar desconectarse de la BD: " + bde.ToString() + "\n");
        //                throw new DBException("Error al obtener configuración.");
        //            }
        //        }
        //    }
        //}

        //public List<MeterMBParConfig> GetMeterMBParConfigL(int MeterId)
        //{
        //    BaseDAO baseDAOSupport = null;
        //    DbDataReader dbReader = null;
        //    List<MeterMBParConfig> mMBPCl = new List<MeterMBParConfig>();

        //    try
        //    {
        //        baseDAOSupport = DaoBase;
        //        baseDAOSupport = BaseDAO.Instance;

        //        baseDAOSupport.Conectar();

        //        baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetMeterMBParConfig"));
        //        baseDAOSupport.AsignarParametroEntero("@MeterId", MeterId);

        //        dbReader = baseDAOSupport.EjecutarConsulta();

        //        if (dbReader.HasRows)
        //        {
        //            while (dbReader.Read())
        //            {
        //                MeterMBParConfig mMBPC = new MeterMBParConfig();
        //                mMBPC.Models.ModelId = Convert.ToInt16(CommonUtils.validaCampo(dbReader["ModelId"], CommonUtils.defaultEntero));
        //                mMBPC.Models.MBFunctionId = Convert.ToInt16(CommonUtils.validaCampo(dbReader["MBFunctionId"], CommonUtils.defaultEntero));
        //                mMBPC.MeasurRange.MeasurType.MeasurTypeId = Convert.ToInt16(CommonUtils.validaCampo(dbReader["MeasurTypeId"], CommonUtils.defaultEntero));
        //                mMBPC.MeasurRange.MeasurType.Calculated = Convert.ToBoolean(CommonUtils.validaCampo(dbReader["Calculated"], CommonUtils.defaultBoolean));
        //                mMBPC.MeasurMeterList.SampleTime = Convert.ToInt16(CommonUtils.validaCampo(dbReader["SampleTime"], CommonUtils.defaultEntero));
        //                mMBPC.MeasurRange.Range = Convert.ToString(CommonUtils.validaCampo(dbReader["Range"], null)).Trim();
        //                mMBPCl.Add(mMBPC);
        //            }
        //        }
        //        return mMBPCl;
        //    }
        //    catch (Exception bde)
        //    {
        //        System.Diagnostics.Trace.WriteLine("Error al obtener configuración de lecturas por medidor: " + bde.ToString() + "\n");
        //        throw new DBException("Error en Acceso a Base de Datos ");
        //    }

        //    finally
        //    {
        //        if (baseDAOSupport != null)
        //        {
        //            try
        //            {
        //                baseDAOSupport.Desconectar();
        //            }
        //            catch (Exception bde)
        //            {
        //                System.Diagnostics.Trace.WriteLine("Error al intentar desconectarse de la BD: " + bde.ToString() + "\n");
        //                throw new DBException("Error al obtener configuración de lecturas por medidor.");
        //            }
        //        }
        //    }
        //}
        //public SerialCom GetSerialCom()
        //{
        //    return new SerialCom
        //    {
        //        ComPort = ConfigurationManager.AppSettings["port"],
        //        Baudrate = ConfigurationManager.AppSettings["Baudrate"],
        //        Parity = ConfigurationManager.AppSettings["Parity"],
        //        Stopbits = ConfigurationManager.AppSettings["Stopbits"]
        //    };

        //    BaseDAO baseDAOSupport = null;
        //    DbDataReader dbReader = null;
        //    SerialCom sc = new SerialCom();

        //    try
        //    {
        //        baseDAOSupport = DaoBase;
        //        baseDAOSupport = BaseDAO.Instance;

        //        baseDAOSupport.Conectar();

        //        baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetSerialCom"));

        //        dbReader = baseDAOSupport.EjecutarConsulta();

        //        if (dbReader.HasRows)
        //        {
        //            if (dbReader.Read())
        //            {
        //                sc.ComPort = Convert.ToString(CommonUtils.validaCampo(dbReader["ComPort"], null)).Trim();
        //                sc.Baudrate = Convert.ToString(CommonUtils.validaCampo(dbReader["Baudrate"], null)).Trim();
        //                sc.Parity = Convert.ToString(CommonUtils.validaCampo(dbReader["Parity"], null)).Trim();
        //                sc.Stopbits = Convert.ToString(CommonUtils.validaCampo(dbReader["Stopbits"], null)).Trim();
        //            }
        //        }
        //        return sc;
        //    }
        //    catch (Exception bde)
        //    {
        //        System.Diagnostics.Trace.WriteLine("Error al obtener configuración serial: " + bde.ToString() + "\n");
        //        throw new DBException("Error en Acceso a Base de Datos ");
        //    }

        //    finally
        //    {
        //        if (baseDAOSupport != null)
        //        {
        //            try
        //            {
        //                baseDAOSupport.Desconectar();
        //            }
        //            catch (Exception bde)
        //            {
        //                System.Diagnostics.Trace.WriteLine("Error al intentar desconectarse de la BD: " + bde.ToString() + "\n");
        //                throw new DBException("Error al obtener configuración serial.");
        //            }
        //        }
        //    }
        //}
        //public List<MBRegisters> GetParametersMetersList(int MeterId, MBRange range)
        //{
        //    List<MBRegisters> mbrl = new List<MBRegisters>();

        //    DbConnection connection = DataAccesBase.DataBase.CreateDefaultConnection();
        //    connection.Open();

        //    try
        //    {
        //        DbCommand command = connection.CreateCommand();
        //        command.CommandText = @"select pms.ParameterId, mbr.MBRegId, mbr.Multiplier, dt.DataTypeId from parametermeterselect pms
        //                            inner join generalconfig gc on gc.MeterId = pms.MeterId
        //                            inner join mbregisters mbr on mbr.ParameterId = pms.ParameterId
        //                            inner join datatype dt on dt.DataTypeId = mbr.DataType
        //                            where gc.MeterId = @MeterId and gc.ModelId = mbr.ModelId
        //                            and mbr.MBRegId between @StartRange and @EndRange;";

        //        command.Parameters.AddWithValue("@MeterId", System.Data.DbType.Int32, MeterId);
        //        command.Parameters.AddWithValue("@StartRange", System.Data.DbType.Int32, range.StartRange);
        //        command.Parameters.AddWithValue("@EndRange", System.Data.DbType.Int32, range.EndRange);

        //        DbDataReader dbReader = command.ExecuteReader();

        //        if (dbReader.HasRows)
        //        {
        //            while (dbReader.Read())
        //            {
        //                MBRegisters mbr = new MBRegisters();
        //                mbr.MeterParameters.ParameterId = (int)dbReader["ParameterId"];
        //                mbr.MBRegId = (int)dbReader["MBRegId"];
        //                mbr.Multiplier = (float)dbReader["Multiplier"];
        //                mbr.DataType.DataTypeId = (int)dbReader["DataTypeId"];
        //                mbrl.Add(mbr);
        //            }
        //        }
                
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return mbrl;
        //}

#region Save MB data to DB
        //public bool SaveMeterListening(MeterDataReaded mdr)
        //{
        //    BaseDAO baseDAOSupport = null;
        //    int registros = 0;
        //    bool sucess = false;

        //    try
        //    {
        //        baseDAOSupport = DaoBase;
        //        baseDAOSupport = BaseDAO.Instance;
        //        baseDAOSupport.Conectar();

        //        baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("InsertMeterListening"));
        //        baseDAOSupport.AsignarParametroEntero("@MeterId", mdr.GeneralConfig.MeterId);
        //        baseDAOSupport.AsignarParametroCadena("@DeviceIdGuid", mdr.GeneralConfig.DeviceIdGuid);
        //        baseDAOSupport.AsignarParametroCadena("@ListeningGuid", mdr.ListeningGuid);
        //        baseDAOSupport.AsignarParametroEntero("@Timestamp", mdr.Timestamp);
        //        baseDAOSupport.AsignarParametroEntero("@DataSendedStatus", mdr.DataSendedStatus);

        //        registros = baseDAOSupport.Ejecutar();

        //        if (registros > 0)
        //            sucess = true;

        //        return sucess;
        //    }

        //    catch (Exception bde)
        //    {
        //        System.Diagnostics.Trace.WriteLine("Error al guardar lista de medidores leídos: " + bde.ToString() + "\n");
        //        return false;
        //    }

        //    finally
        //    {
        //        if (baseDAOSupport != null)
        //        {
        //            try
        //            {
        //                baseDAOSupport.Desconectar();
        //            }
        //            catch (Exception bde)
        //            {
        //                System.Diagnostics.Trace.WriteLine("Error al intentar desconectarse de la BD: " + bde.ToString() + "\n");
        //            }
        //        }
        //    }
        //}
        public bool SaveListeningValues(List<MBDataReadedValues> mbdrv)
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                foreach (var mbv in mbdrv)
                {
                    baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("InsertListeningValues"));
                    baseDAOSupport.AsignarParametroCadena("@ListeningGuid", mbv.ListeningGuid);
                    baseDAOSupport.AsignarParametroEntero("@MeasurTypeId", mbv.MeasurType.MeasurTypeId);
                    baseDAOSupport.AsignarParametroEntero("@ParameterId", mbv.MeterParameters.ParameterId);
                    baseDAOSupport.AsignarParametroDouble("@ListeningValue", mbv.ListeningValue);

                    registros = baseDAOSupport.Ejecutar();

                    if (registros > 0)
                        sucess = true;
                }

                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al guardar datos obtenidos: " + bde.ToString() + "\n");
                return false;
            }

            finally
            {
                if (baseDAOSupport != null)
                {
                    try
                    {
                        baseDAOSupport.Desconectar();
                    }
                    catch (Exception bde)
                    {
                        System.Diagnostics.Trace.WriteLine("Error al intentar desconectarse de la BD: " + bde.ToString() + "\n");
                    }
                }
            }
        }
#endregion
    }
}
