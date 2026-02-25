using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace ApiUsuarioKRT.Data
{
    public class DBConnectionFactory
    {
        private readonly string _connectionString;

        public DBConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SqliteConnection");
        }

        public DbConnection CreateConnection() => new SqliteConnection(_connectionString);
    }
}
