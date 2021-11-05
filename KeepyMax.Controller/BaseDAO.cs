using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Diagnostics;

namespace DeviceDAO.Base
{
	/// <summary>
	/// Representa la base de datos en el sistema.
	/// Ofrece los métodos de acceso a la misma.
	/// </summary>
	public class BaseDAO
	{

		private static BaseDAO instance;

		private BaseDAO() { Configurar();}

		public static BaseDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BaseDAO();
				}

				return instance;
			}
		}

		private DbConnection conexion = null;
		private DbCommand comando = null;
		private DbTransaction transaccion = null;
		private string cadenaConexion;

		private DbProviderFactory factory = null;

		void conexion_StateChange(object sender, StateChangeEventArgs e)
		{
			if (e.CurrentState == ConnectionState.Closed)
				((DbConnection)sender).Dispose();
		}

		/// <summary>
		/// Configura el acceso a la base de datos para su utilización.
		/// </summary>
		/// <exception cref="BaseDatosException">Si existe un error al cargar la configuración.</exception>
		private void Configurar() 
		{
			try
			{
				string proveedor = ConfigurationManager.AppSettings.Get("PROVEEDOR_ADONET");
				this.cadenaConexion = ConfigurationManager.AppSettings.Get("CADENA_CONEXION");
				//EventLog.WriteEntry("RANKINGWEB@DEBUG", "Proveedor: " + proveedor + " Cadena: " + cadenaConexion , EventLogEntryType.Information);

				//BaseDatos.factory = DbProviderFactories.GetFactory(proveedor);
				this.factory = DbProviderFactories.GetFactory(proveedor);
			} catch (ConfigurationException ex) {
				throw new DBException("Error al cargar la configuración del acceso a datos.", ex);
			}
		}

		/// <summary>
		/// Permite desconectarse de la base de datos.
		/// </summary>
		public void Desconectar()
		{
			if( this.conexion.State.Equals(ConnectionState.Open) )
			{
				this.conexion.Close();
				instance = null;
			}
		}

		/// <summary>
		/// Se concecta con la base de datos.
		/// </summary>
		/// <exception cref="BaseDatosException">Si existe un error al conectarse.</exception>
		public void Conectar()
		{
			if (this.conexion != null /*&& !this.conexion.State.Equals(ConnectionState.Closed)*/) {
				//throw new DBException("La conexión ya se encuentra abierta.");
				//this.conexion.Close();
				this.conexion = null;
			}
			try {
				if (this.conexion == null)
				{
					this.conexion = factory.CreateConnection();
					this.conexion.ConnectionString = cadenaConexion;
					conexion.StateChange += new StateChangeEventHandler(conexion_StateChange);
				}
				this.conexion.Open();
			} catch (DataException ex) {
				throw new DBException("Error al conectarse a la base de datos.", ex);
			}
		}

		/// <summary>
		/// Crea un sp SQL.
		/// Ejemplo:
		/// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
		/// Guarda el comando para el seteo de parámetros y la posterior ejecución.
		/// </summary>
		/// <param name="sentenciaSQL">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>
		public void CrearSP(string spSQL)
		{
			this.comando = factory.CreateCommand();
			this.comando.Connection = this.conexion;
			this.comando.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TIMEOUTDB")); 
			this.comando.CommandText = spSQL;
			this.comando.CommandType = CommandType.StoredProcedure;
			if (this.transaccion != null) {
				this.comando.Transaction = this.transaccion;
			}
		}

		/// <summary>
		/// Crea un query SQL.
		/// Ejemplo:
		/// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
		/// Guarda el comando para el seteo de parámetros y la posterior ejecución.
		/// </summary>
		/// <param name="sentenciaSQL">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>
		public void CrearQuery(string spSQL)
		{
			this.comando = factory.CreateCommand();
			this.comando.Connection = this.conexion;
			this.comando.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TIMEOUTDB"));
			this.comando.CommandText = spSQL;
			this.comando.CommandType = CommandType.Text;
			if (this.transaccion != null)
			{
				this.comando.Transaction = this.transaccion;
			}
		}

		/// <summary>
		/// Setea un parámetro como nulo del comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro cuyo valor será nulo.</param>
		public void AsignarParametroNulo(string nombre)
		{
			DbParameter parString = factory.CreateParameter();
			parString.ParameterName = nombre;
			//parString.DbType = System.Data.DbType.Object;
			parString.Value = DBNull.Value;
			parString.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parString); 
		}

		/// <summary>
		/// Agrega un parametro de salida suponiendo que es cadena
		/// tal que se pueda parsear a lo que se necesite
		/// </summary>
		/// <param name="nombre"></param>
		/// <param name="valor"></param>
		public void AsignarParametroSalida(string nombre)
		{
			DbParameter parOutputString = this.factory.CreateParameter();
			parOutputString.ParameterName = nombre;
			parOutputString.DbType = System.Data.DbType.String;
			parOutputString.Size = 100;
			parOutputString.Direction = System.Data.ParameterDirection.Output;
			this.comando.Parameters.Add(parOutputString);
		}

		/// <summary>
		/// Asigna un parámetro de tipo cadena al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroCadena(string nombre, string valor) {
			DbParameter parString = factory.CreateParameter();
			parString.ParameterName = nombre;
			parString.DbType = System.Data.DbType.String;
			if(string.IsNullOrEmpty(valor))
				parString.Value = DBNull.Value;
			else
				parString.Value = valor;
			parString.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parString); 
		}

		/// <summary>
		/// Asigna un parámetro de tipo entero al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroEntero(string nombre, Int32 valor) 
		{
			DbParameter parInteger = factory.CreateParameter();
			parInteger.ParameterName = nombre;
			parInteger.DbType = System.Data.DbType.Int32;
			parInteger.Value = valor;
			parInteger.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parInteger); 
		}


		public void AsignarParametroObjeto(string nombre, object valor)
		{
			DbParameter parInteger = factory.CreateParameter();
			parInteger.ParameterName = nombre;
			parInteger.Value = valor;
			parInteger.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parInteger);
		}
		/// <summary>
		/// Asigna un parámetro de tipo entero al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroLong(string nombre, Int64 valor)
		{
			DbParameter parInteger = factory.CreateParameter();
			parInteger.ParameterName = nombre;
			parInteger.DbType = System.Data.DbType.Int64;
			parInteger.Value = valor;
			parInteger.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parInteger);
		}

		/// <summary>
		/// Asigna un parámetro de tipo fecha al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroFecha(string nombre, DateTime valor)
		{
			DbParameter parString = factory.CreateParameter();
			parString.ParameterName = nombre;
			parString.DbType = System.Data.DbType.DateTime2;
			parString.Value = valor;
			parString.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parString); 
		}

		/// <summary>
		/// Asigna un parámetro de tipo decimal al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroDecimal(string nombre, Decimal  valor)
		{
			DbParameter parDec = factory.CreateParameter();
			parDec.ParameterName = nombre;
			parDec.DbType = System.Data.DbType.Decimal ;
			parDec.Value = valor;
			parDec.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parDec);
		}

		/// <summary>
		/// Asigna un parámetro de tipo decimal al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroDouble(string nombre, Double valor)
		{
			DbParameter parDec = factory.CreateParameter();
			parDec.ParameterName = nombre;
			parDec.DbType = System.Data.DbType.Double;
			parDec.Value = valor;
			parDec.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parDec);
		}

		/// <summary>
		/// Asigna un parámetro de tipo boolean al comando creado.
		/// </summary>
		/// <param name="nombre">El nombre del parámetro.</param>
		/// <param name="valor">El valor del parámetro.</param>
		public void AsignarParametroBoolean(string nombre, Boolean valor)
		{
			DbParameter parDec = factory.CreateParameter();
			parDec.ParameterName = nombre;
			parDec.DbType = System.Data.DbType.Boolean;
			parDec.Value = valor;
			parDec.Direction = System.Data.ParameterDirection.Input;
			this.comando.Parameters.Add(parDec);
		}

		/// <summary>
		/// Obtiene el valor del parametro de salida del sp
		/// </summary>
		/// <param name="nombre"></param>
		/// <returns></returns>
		public object ObtenerValorParametroSalida(string nombre)
		{
			object valorOut = null;

			if (this.comando != null && 
				this.comando.Parameters != null &&
				this.comando.Parameters.Contains(nombre))
			{ 
				valorOut = this.comando.Parameters[nombre].Value;
			}

			return valorOut;
		}

		/// <summary>
		/// Ejecuta el comando creado y retorna el resultado de la consulta.
		/// </summary>
		/// <returns>El resultado de la consulta.</returns>
		/// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
		public DbDataReader EjecutarConsulta()
		{
			return this.comando.ExecuteReader();
		}

		/// <summary>
		/// Ejecuta el comando creado y retorna un escalar.
		/// </summary>
		/// <returns>El escalar que es el resultado del comando.</returns>
		/// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
		public Int64 EjecutarEscalar()
		{
			Int64 escalar = 0;
			try {
				escalar = Int64.Parse(this.comando.ExecuteScalar().ToString());
			} catch (InvalidCastException ex) {
				throw new DBException("Error al ejecutar un escalar.", ex);
			}
			return escalar;
		}

		/// <summary>
		/// Ejecuta el comando creado.
		/// </summary>
		public int Ejecutar()
		{
			return this.comando.ExecuteNonQuery();
		}

		/// <summary>
		/// Ejecuta el comando creado y retorna el resultado de la consulta.
		/// </summary>
		/// <returns>El resultado de la consulta.</returns>
		/// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
		public DbDataReader EjecutarRegresaIdentity()
		{
			return this.comando.ExecuteReader();
		}


		/// <summary>
		/// Comienza una transacción en base a la conexion abierta.
		/// Todo lo que se ejecute luego de esta ionvocación estará 
		/// dentro de una tranasacción.
		/// </summary>
		public void ComenzarTransaccion()
		{
			if( this.transaccion == null )
			{
				this.transaccion = this.conexion.BeginTransaction();
			}
		}

		/// <summary>
		/// Cancela la ejecución de una transacción.
		/// Todo lo ejecutado entre ésta invocación y su 
		/// correspondiente <c>ComenzarTransaccion</c> será perdido.
		/// </summary>
		public void CancelarTransaccion()
		{
			if( this.transaccion != null )
			{
				this.transaccion.Rollback();
				this.transaccion = null;
			}
		}

		/// <summary>
		/// Confirma todo los comandos ejecutados entre el <c>ComanzarTransaccion</c>
		/// y ésta invocación.
		/// </summary>
		public void ConfirmarTransaccion()
		{
			if( this.transaccion != null )
			{
				this.transaccion.Commit();
				this.transaccion = null;
			}
		}

	}
}
