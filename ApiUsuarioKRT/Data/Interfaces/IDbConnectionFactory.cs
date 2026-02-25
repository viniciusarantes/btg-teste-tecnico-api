using System.Data;

namespace ApiUsuarioKRT.Data.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
