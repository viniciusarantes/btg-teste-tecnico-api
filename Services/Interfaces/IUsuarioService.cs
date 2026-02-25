using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;

namespace TesteTecnicoBTG.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetUsuarioListAsync();
        Task<Usuario?> GetUsuarioAsync(string usuarioId);
        Task<Usuario> CreateUsuarioAsync(CreateUsuarioRequest request);
        Task<Usuario?> UpdateUsuarioAsync(string usuarioId, UpdateUsuarioRequest request);
        Task<bool> DeleteUsuarioAsync(string usuarioId);
    }
}
