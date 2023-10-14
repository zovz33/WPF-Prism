using Microsoft.Data.SqlClient;
using System;
using System.Configuration;

namespace WPFPrism.Infrastructure.Base
{
    public abstract class ServiceBase
    {
        private readonly string _connectionString;
        public ServiceBase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
