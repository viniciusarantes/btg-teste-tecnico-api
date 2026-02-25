using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;

namespace TesteTecnicoBTG.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetUsuarioListAsync();
        Task<Usuario?> GetUsuarioAsync(Guid usuarioId);
        Task<Usuario> CreateUsuarioAsync(CreateUsuarioRequest request);
        Task<Usuario?> UpdateUsuarioAsync(Guid usuarioId, UpdateUsuarioRequest request);
        Task<bool> DeleteUsuarioAsync(Guid usuarioId);
    }
}
