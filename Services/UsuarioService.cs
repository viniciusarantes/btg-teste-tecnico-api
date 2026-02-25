using TesteTecnicoBTG.Data.Interfaces;
using TesteTecnicoBTG.Mapper;
using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;
using TesteTecnicoBTG.ModelView.Response;
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

        public async Task<UsuarioResponse> CreateUsuarioAsync(CreateUsuarioRequest request)
        {
            var usuario = new Usuario()
            {
                NomeTitular = request.NomeTitular,
                Cpf = request.Cpf,
                StatusConta = StatusConta.Ativo
            };
            var newUsuario = await _usuarioRepository.CreateUsuarioAsync(usuario);
            return newUsuario.ToResponse();
        }

        public async Task<bool> DeleteUsuarioAsync(string userId)
        {
            var success = await _usuarioRepository.DeleteUsuarioAsync(userId);
            //var success = await _usuarioRepository.SoftDeleteUsuarioAsync(userId);
            return success;
        }

        public async Task<UsuarioResponse?> GetUsuarioAsync(string userId)
        {
            var usuario = await _usuarioRepository.GetUsuarioAsync(userId);
            return usuario?.ToResponse();
        }

        public async Task<List<UsuarioResponse>> GetUsuarioListAsync()
        {
            var usuarioList = await _usuarioRepository.GetUsuarioListAsync();
            return usuarioList.Select(u => u.ToResponse()).ToList();
        }

        public async Task<UsuarioResponse?> UpdateUsuarioAsync(string userId, UpdateUsuarioRequest request)
        {

            var usuarioDb = await _usuarioRepository.GetUsuarioAsync(userId);
            if (usuarioDb == null) return null;

            usuarioDb.NomeTitular = request.NomeTitular ?? usuarioDb.NomeTitular;
            usuarioDb.Cpf = request.Cpf ?? usuarioDb.Cpf;
            if (request.StatusConta.HasValue)
                usuarioDb.StatusConta = request.StatusConta.Value;

            var sucesso = await _usuarioRepository.UpdateUsuarioAsync(usuarioDb);
            
            if (!sucesso) throw new Exception("Nenhum registro foi atualizado.");
            return usuarioDb?.ToResponse();
        }
    }
}
