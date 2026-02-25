using TesteTecnicoBTG.ModelView.Request;
using TesteTecnicoBTG.ModelView.Response;

namespace TesteTecnicoBTG.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioResponse>?> GetUsuarioListAsync();
        Task<UsuarioResponse?> GetUsuarioAsync(string usuarioId);
        Task<UsuarioResponse> CreateUsuarioAsync(CreateUsuarioRequest request);
        Task<UsuarioResponse?> UpdateUsuarioAsync(string usuarioId, UpdateUsuarioRequest request);
        Task<bool> DeleteUsuarioAsync(string usuarioId);
    }
}
