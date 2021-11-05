using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace KeepyMax.Controller.DataAccess
{
    public class DataAccesBase
    {
        public const string DefaultConnetionStringName = "MySQL";

        private static DbProviderFactory factory = null;

        readonly string connectionStringName;
        public static DbProviderFactory DataBase
        {
            get
            {
                if(factory == null)
                {
                    factory = CreateFactory(DefaultConnetionStringName);
                }
                return factory;
            }
        }

        public static DbProviderFactory CreateFactory(string connectionStringName)
        {
            DbProviderFactory f;
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            f = DbProviderFactories.GetFactory(connectionString.ProviderName);
            
            return f;
        }

       

        
    }
}
