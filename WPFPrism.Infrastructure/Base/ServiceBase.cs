using Microsoft.Data.SqlClient;
using System;


namespace WPFPrism.Infrastructure.Base
{
    public abstract class ServiceBase
    {
        private readonly string _connectionString;
        public ServiceBase()
        {
            _connectionString = "Data Source = LENOVO\\SQLEXPRESS01; Initial Catalog = WPFPrismServiceDb; User Id = 1; Password = 1; TrustServerCertificate = True";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
