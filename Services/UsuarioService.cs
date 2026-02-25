using TesteTecnicoBTG.Data.Interfaces;
using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;
using TesteTecnicoBTG.Services.Interfaces;

namespace TesteTecnicoBTG.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> CreateUsuarioAsync(CreateUsuarioRequest request)
        {
            var usuario = new Usuario()
            {
                NomeTitular = request.NomeTitular,
                Cpf = request.Cpf,
                StatusConta = StatusConta.Ativo
            };
            var newUsuario = await _usuarioRepository.CreateUsuarioAsync(usuario);
            return newUsuario;
        }

        public async Task<bool> DeleteUsuarioAsync(Guid userId)
        {
            // var success = await _usuarioRepository.DeleteUsuarioAsync(userId);
            var success = await _usuarioRepository.SoftDeleteUsuarioAsync(userId);
            return success;
        }

        public async Task<Usuario?> GetUsuarioAsync(Guid userId)
        {
            var usuario = await _usuarioRepository.GetUsuarioAsync(userId);
            return usuario;
        }

        public async Task<List<Usuario>> GetUsuarioListAsync()
        {
            var usuarioList = await _usuarioRepository.GetUsuarioListAsync();
            return usuarioList;
        }

        public async Task<Usuario?> UpdateUsuarioAsync(Guid userId, UpdateUsuarioRequest request)
        {
            var usuario = new Usuario()
            {
                Id = userId,
                NomeTitular = request.NomeTitular,
                Cpf = request.Cpf,
                StatusConta = request.StatusConta,
            };
            var updated = await _usuarioRepository.UpdateUsuarioAsync(usuario);
            return updated;
        }
    }
}
