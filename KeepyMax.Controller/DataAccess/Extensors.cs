using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;


namespace KeepyMax.Controller.DataAccess
{
    public static class Extensors
    {

        public static void AddWithValue(this DbParameterCollection collection, string name, System.Data.DbType type, object value, System.Data.ParameterDirection direction)
        {
            DbParameter parameter = DataAccesBase.DataBase.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value;
            parameter.Direction = direction;
            collection.Add(parameter);
        }


        public static void AddWithValue(this DbParameterCollection collection, string name, System.Data.DbType type, object value)
        {
            DbParameter parameter = DataAccesBase.DataBase.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value;
            parameter.Direction = System.Data.ParameterDirection.Input;
            collection.Add(parameter);

        }

        public static DbConnection CreateDefaultConnection(this DbProviderFactory factory)
        {
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings[DataAccesBase.DefaultConnetionStringName].ConnectionString;
            return connection;
        }


    }
}
