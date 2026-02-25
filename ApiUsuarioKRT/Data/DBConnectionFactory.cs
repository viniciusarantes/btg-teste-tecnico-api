using ApiUsuarioKRT.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace ApiUsuarioKRT.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SqliteConnection");
        }

        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
    }
}
