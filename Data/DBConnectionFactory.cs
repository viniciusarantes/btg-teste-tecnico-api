using Microsoft.Data.Sqlite;
using System.Data;

namespace TesteTecnicoBTG.Data
{
    public class DBConnectionFactory : IDisposable
    {
        public IDbConnection Connection { get; }

        public DBConnectionFactory(IConfiguration config)
        {
            Connection = new SqliteConnection(config.GetConnectionString("SqliteConnection"));
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
