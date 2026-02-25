using Dapper;
using ApiUsuarioKRT.Data.Interfaces;
using ApiUsuarioKRT.Models;

namespace ApiUsuarioKRT.Data.Repositories
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
            
            usuario.Id = Guid.NewGuid().ToString();

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

        public async Task<bool> SoftDeleteUsuarioAsync(string usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "UPDATE usuario SET isDeleted = 1 WHERE id = @UsuarioId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUsuarioAsync(string usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "DELETE FROM usuario WHERE id = @UsuarioId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<Usuario?> GetUsuarioAsync(string usuarioId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT id, nomeTitular, cpf, statusConta FROM usuario WHERE id = @UsuarioId AND isDeleted = 0";
            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { UsuarioId = usuarioId });

            return usuario;
        }

        public async Task<List<Usuario>> GetUsuarioListAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT id, nomeTitular, cpf, statusConta FROM usuario";
            var usuarioList = await connection.QueryAsync<Usuario>(sql);

            return usuarioList.ToList();
        }

        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var sql = @"
                UPDATE usuario SET 
                    nomeTitular = @NomeTitular, 
                    cpf = @Cpf, 
                    statusConta = @StatusConta 
                WHERE id = @UsuarioId";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                UsuarioId = usuario.Id,
                NomeTitular = usuario.NomeTitular,
                Cpf = usuario.Cpf,
                StatusConta = usuario.StatusConta
            });

            return rowsAffected > 0;
        }
    }
}
