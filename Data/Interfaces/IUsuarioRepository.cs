using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;

namespace TesteTecnicoBTG.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetUsuarioListAsync();
        Task<Usuario?> GetUsuarioAsync(string usuarioId);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task<Usuario?> UpdateUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(string usuarioId);
        Task<bool> SoftDeleteUsuarioAsync(string usuarioId);
    }
}
