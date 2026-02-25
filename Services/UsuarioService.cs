using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;
using TesteTecnicoBTG.Services.Interfaces;

namespace TesteTecnicoBTG.Services
{
    public class UsuarioService : IUsuarioService
    {
        public Task<Usuario> CreateUsuarioAsync(CreateUsuarioRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUsuarioAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> GetUsuarioAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Usuario>> GetUsuarioListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsuarioAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
