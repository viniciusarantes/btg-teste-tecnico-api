using ApiUsuarioKRT.Models;
using ApiUsuarioKRT.ModelView.Request;

namespace ApiUsuarioKRT.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetUsuarioListAsync();
        Task<Usuario?> GetUsuarioAsync(string usuarioId);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(string usuarioId);
        Task<bool> SoftDeleteUsuarioAsync(string usuarioId);
    }
}
