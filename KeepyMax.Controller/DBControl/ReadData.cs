using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using KeepyMax.Controller.DBControlI;
using KeepyMax.Model;
using DeviceDAO.Base;
using DeviceDAO;

namespace KeepyMax.Controller.DBControl
{
    public class ReadData : ReadDataI
    {
        public BaseDAO DaoBase { get; set; }
        public List<MeterDataReaded> GetListeningMeters(int TimeStart)
        {
            BaseDAO baseDAOSupport = null;
            DbDataReader dbReader = null;
            List<MeterDataReaded> mdrL = new List<MeterDataReaded>();

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();


                if (TimeStart > 0)
                {
                    baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetListeningMeters"));
                    baseDAOSupport.AsignarParametroEntero("@TimeStart", TimeStart);
                }
                else
                    baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetAllListeningMeters"));

                dbReader = baseDAOSupport.EjecutarConsulta();

                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        MeterDataReaded mdr = new MeterDataReaded();
                        mdr.GeneralConfig.DeviceIdGuid = Convert.ToString(CommonUtils.validaCampo(dbReader["DeviceIdGuid"], null)).Trim();
                        //mdr.ListeningGuid = Convert.ToString(CommonUtils.validaCampo(dbReader["ListeningGuid"], null)).Trim();
                        mdr.Timestamp = Convert.ToInt32(CommonUtils.validaCampo(dbReader["Timestamp"], CommonUtils.defaultEntero));
                        mdrL.Add(mdr);
                    }
                }
                return mdrL;
            }
            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al obtener meters listeningsID: " + bde.ToString() + "\n");
                throw new DBException("Error en Acceso a Base de Datos ");
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
                        throw new DBException("Error al obtener meters listeningsID.");
                    }
                }
            }
        }

        public List<MBDataReadedValues> GetListeningValues(string ListeningGuid)
        {
            BaseDAO baseDAOSupport = null;
            DbDataReader dbReader = null;
            List<MBDataReadedValues> mbdrvL = new List<MBDataReadedValues>();

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();
                
                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("GetListeningValues"));
                baseDAOSupport.AsignarParametroCadena("@ListeningGuid", ListeningGuid);
                
                dbReader = baseDAOSupport.EjecutarConsulta();

                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        MBDataReadedValues mbdrv = new MBDataReadedValues();
                        mbdrv.MeterParameters.ParameterId = Convert.ToInt16(CommonUtils.validaCampo(dbReader["ParameterId"], CommonUtils.defaultEntero)); ;
                        mbdrv.ListeningValue = Convert.ToSingle(CommonUtils.validaCampo(dbReader["ListeningValue"], CommonUtils.defaultFloat));
                        mbdrvL.Add(mbdrv);
                    }
                }
                return mbdrvL;
            }
            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al obtener listening values: " + bde.ToString() + "\n");
                throw new DBException("Error en Acceso a Base de Datos ");
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
                        throw new DBException("Error al obtener listening values.");
                    }
                }
            }
        }

        public bool DeleteSendedData_ListeningGuid(string ListeningGuid)
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("DeleteDataSendedLG"));
                baseDAOSupport.AsignarParametroCadena("@ListeningGuid", ListeningGuid);

                registros = baseDAOSupport.Ejecutar();

                if (registros > 0)
                    sucess = true;
                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al eliminar datos obtenidos x ListeningGuid: " + bde.ToString() + "\n");
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

        public bool GetPowerDemand(int Timestamp)
        {
            BaseDAO baseDAOSupport = null;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();

                baseDAOSupport.CrearQuery("call GetPowDemandSP(@Timest)");
                baseDAOSupport.AsignarParametroEntero("@Timest", Timestamp);

                int registros = baseDAOSupport.Ejecutar();
                bool sucess = false;
                if (registros > 0)
                    sucess = true;
                
                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al obtener datos de KWh 15 minutos: " + bde.ToString() + "\n");
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
    }
}
