using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using KeepyMax.Model;
using KeepyMax.Controller.DBControlI;
using DeviceDAO.Base;
using DeviceDAO;

namespace KeepyMax.Controller.DBControl
{
    public class AlertsControl : AlertsControlI
    {
        public BaseDAO DaoBase { get; set; }
        public bool SaveConnectionAlertList(List<ConnectionAlertList> calL)
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                foreach (var cal in calL)
                {
                    baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("InsertConnectionAlertList"));
                    baseDAOSupport.AsignarParametroEntero("@ConnectionTypeId", cal.ConnectionTypeId);
                    baseDAOSupport.AsignarParametroEntero("@MeterId", cal.MeterId);
                    baseDAOSupport.AsignarParametroEntero("@Timestamp", cal.Timestamp);

                    registros = baseDAOSupport.Ejecutar();

                    if (registros > 0)
                        sucess = true;
                }

                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al guardar alertas de conexión: " + bde.ToString() + "\n");
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
                        System.Diagnostics.Trace.WriteLine("Error al intentar alertas de conexión: " + bde.ToString() + "\n");
                    }
                }
            }
        }

        public bool InsertAlertsConfig(List<Alerts> ALertsL)
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                foreach (var alert in ALertsL)
                {
                    baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("InsertAlertsConfig"));
                    baseDAOSupport.AsignarParametroCadena("@AlertIdGuid", alert.AlertIdGuid);
                    baseDAOSupport.AsignarParametroCadena("@SerialMeter", alert.SerialMeter);
                    baseDAOSupport.AsignarParametroEntero("@ParameterId", alert.ParameterId);
                    baseDAOSupport.AsignarParametroEntero("@AlarmTypeId", alert.AlarmTypeId);
                    baseDAOSupport.AsignarParametroDouble("@MinValueAl", alert.MinValueAl);
                    baseDAOSupport.AsignarParametroDouble("@MaxValueAl", alert.MaxValueAl);

                    registros = baseDAOSupport.Ejecutar();

                    if (registros > 0)
                        sucess = true;
                }

                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al insertar una configuración de alarmas: " + bde.ToString() + "\n");
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

        public List<Alerts> ReadRangeAlerts()
        {
            BaseDAO baseDAOSupport = null;
            DbDataReader dbReader = null;
            List<Alerts> alL = new List<Alerts>();

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();

                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("ReadRangeAlerts"));

                dbReader = baseDAOSupport.EjecutarConsulta();

                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        Alerts al = new Alerts();
                        al.AlertIdGuid = Convert.ToString(CommonUtils.validaCampo(dbReader["AlertIdGuid"], null)).Trim();
                        al.ReadValue = Convert.ToSingle(CommonUtils.validaCampo(dbReader["ReadValue"], CommonUtils.defaultFloat));
                        al.Timestamp = Convert.ToInt32(CommonUtils.validaCampo(dbReader["Timestamp"], CommonUtils.defaultEntero));
                        alL.Add(al);
                    }
                }
                return alL;
            }
            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al obtener listado de alertas de rango: " + bde.ToString() + "\n");
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
                        throw new DBException("Error al obtener listado de alertas de rango.");
                    }
                }
            }
        }

        public bool GetRangeAlerts(int Timestamp)
        {
            BaseDAO baseDAOSupport = null;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();

                baseDAOSupport.CrearQuery("call RangeAlertsSP(@Timestamp)");
                baseDAOSupport.AsignarParametroEntero("@Timestamp", Timestamp);

                int registros = baseDAOSupport.Ejecutar();
                bool sucess = false;
                if (registros > 0)
                    sucess = true;

                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al generar alertas de rangos: " + bde.ToString() + "\n");
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

        public List<string> ReadConnectionAlerts(int Timestamp)
        {
            BaseDAO baseDAOSupport = null;
            DbDataReader dbReader = null;
            List<string> SerMetL = new List<string>();

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;

                baseDAOSupport.Conectar();

                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("ReadConnectionAlerts"));
                baseDAOSupport.AsignarParametroEntero("@Timestamp", Timestamp);

                dbReader = baseDAOSupport.EjecutarConsulta();

                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        string SerM = string.Empty;
                        SerM = Convert.ToString(CommonUtils.validaCampo(dbReader["DeviceIdGuid"], null)).Trim();
                        SerMetL.Add(SerM);
                    }
                }
                return SerMetL;
            }
            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al obtener listado de dispositivos no detectados: " + bde.ToString() + "\n");
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
                        throw new DBException("Error al obtener listado de dispositivos no detectados.");
                    }
                }
            }
        }

        public bool DeleteAlertsRange()
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("DeleteAlertsRange"));

                registros = baseDAOSupport.Ejecutar();

                if (registros > 0)
                    sucess = true;
                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al eliminar lista de alertas de rango: " + bde.ToString() + "\n");
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

        public bool DeleteConnectionAlert(int ts)
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("DeleteConnectionAlert"));
                baseDAOSupport.AsignarParametroEntero("@Timestamp", ts);

                registros = baseDAOSupport.Ejecutar();

                if (registros > 0)
                    sucess = true;
                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al eliminar lista de alertas de conexión: " + bde.ToString() + "\n");
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

        public bool DeleteAlertsConfig()
        {
            BaseDAO baseDAOSupport = null;
            int registros = 0;
            bool sucess = false;

            try
            {
                baseDAOSupport = DaoBase;
                baseDAOSupport = BaseDAO.Instance;
                baseDAOSupport.Conectar();
                baseDAOSupport.CrearQuery(ConfigurationManager.AppSettings.Get("DeleteAlertsConfig"));

                registros = baseDAOSupport.Ejecutar();

                if (registros > 0)
                    sucess = true;
                return sucess;
            }

            catch (Exception bde)
            {
                System.Diagnostics.Trace.WriteLine("Error al eliminar configuración de alertas: " + bde.ToString() + "\n");
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
