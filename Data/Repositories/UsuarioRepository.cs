using Dapper;
using TesteTecnicoBTG.Data.Interfaces;
using TesteTecnicoBTG.Models;

namespace TesteTecnicoBTG.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly DBConnectionFactory _dbConnection;

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            usuario.Id = Guid.NewGuid();

            var sql = @"
                INSERT INTO usuario (id, nomeTitular, cpf, statusConta, isDeleted) 
                VALUES (@Id, @NomeTitular, @Cpf, @StatusConta, 0)";

            await _dbConnection.Connection.ExecuteAsync(sql, new 
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
            var sql = "UPDATE usuario SET isDeleted = 1 WHERE id = @UsuarioId";
            var rowsAffected = await _dbConnection.Connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUsuarioAsync(Guid usuarioId)
        {
            var sql = "DELETE usuario WHERE id = @UsuarioId";
            var rowsAffected = await _dbConnection.Connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rowsAffected > 0;
        }

        public async Task<Usuario?> GetUsuarioAsync(Guid usuarioId)
        {
            var sql = "SELECT id, nomeTitular, cpf, statusConta WHERE id = @UsuarioId";
            var usuarioList = await _dbConnection.Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { UsuarioId = usuarioId });

            return usuarioList;
        }

        public async Task<List<Usuario>> GetUsuarioListAsync()
        {
            var sql = "SELECT id, nomeTitular, cpf, statusConta";
            var usuarioList = await _dbConnection.Connection.QueryAsync<Usuario>(sql);

            return usuarioList.ToList();
        }

        public async Task<Usuario?> UpdateUsuarioAsync(Usuario usuario)
        {
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

            var rowsAffected = await _dbConnection.Connection.ExecuteAsync(sql, new
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
