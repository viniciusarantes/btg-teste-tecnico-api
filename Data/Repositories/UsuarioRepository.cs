using Dapper;
using TesteTecnicoBTG.Data.Interfaces;
using TesteTecnicoBTG.Models;

namespace TesteTecnicoBTG.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly DBConnectionFactory _dbConnectionFactory;

        public UsuarioRepository(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            usuario.Id = Guid.NewGuid();

            var sql = @"
                INSERT INTO usuario (id, nomeTitular, cpf, statusConta, isDeleted) 
                VALUES (@Id, @NomeTitular, @Cpf, @StatusConta, 0)";

            await connection.ExecuteAsync(sql, new 
            { 
                Id = usuario.Id,
                NomeTitular = usuario.NomeTitular,
                Cpf = usuario.Cpf,
                StatusConta = usuario.StatusConta
            });
            return usuario;
        }

        public async Task<bool> SoftDeleteUsuarioAsync(Guid usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "UPDATE usuario SET isDeleted = 1 WHERE id = @UsuarioId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUsuarioAsync(Guid usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "DELETE usuario WHERE id = @UsuarioId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<Usuario?> GetUsuarioAsync(Guid usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT id, nomeTitular, cpf, statusConta WHERE id = @UsuarioId";
            var usuarioList = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { UsuarioId = usuarioId });

            return usuarioList;
        }

        public async Task<List<Usuario>> GetUsuarioListAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT id, nomeTitular, cpf, statusConta";
            var usuarioList = await connection.QueryAsync<Usuario>(sql);

            return usuarioList.ToList();
        }

        public async Task<Usuario?> UpdateUsuarioAsync(Usuario usuario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var usuarioDb = await GetUsuarioAsync(usuario.Id);
            if (usuarioDb == null) return null;

            usuarioDb.NomeTitular = usuario.NomeTitular ?? usuarioDb.NomeTitular;
            usuarioDb.Cpf = usuario.Cpf ?? usuarioDb.Cpf;
            usuarioDb.StatusConta = usuario.StatusConta;

            var sql = @"
                UPDATE usuario SET 
                    nomeTitular = @NomeTitular, 
                    cpf = @Cpf, 
                    statusConta = @StatusConta 
                WHERE id = @UsuarioId";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                NomeTitular = usuarioDb.NomeTitular,
                Cpf = usuarioDb.Cpf,
                StatusConta = usuarioDb.StatusConta
            });

            if (rowsAffected == 0) throw new Exception("Nenhum registro foi atualizado");
            return usuarioDb;
        }
    }
}
