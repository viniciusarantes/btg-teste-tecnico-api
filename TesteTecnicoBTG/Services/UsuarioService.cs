using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private readonly string _cachePreffix = "usuario_";
        private readonly string _usuarioListCacheKey = "usuario_list";
        private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24));

        public UsuarioService(IUsuarioRepository usuarioRepository, IMemoryCache memoryCache)
        {
            _usuarioRepository = usuarioRepository;
            _memoryCache = memoryCache;
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

            _memoryCache.Remove(_usuarioListCacheKey);

            return newUsuario.ToResponse();
        }

        public async Task<bool> DeleteUsuarioAsync(string usuarioId)
        {
            var success = await _usuarioRepository.DeleteUsuarioAsync(usuarioId);
            //var success = await _usuarioRepository.SoftDeleteUsuarioAsync(usuarioId);
            if (success)
            {
                _memoryCache.Remove($"usuario_{usuarioId}");
                _memoryCache.Remove(_usuarioListCacheKey);
            }
            return success;
        }

        public async Task<UsuarioResponse?> GetUsuarioAsync(string usuarioId)
        {
            string key = $"{_cachePreffix}{usuarioId}";
            if (!_memoryCache.TryGetValue(key, out UsuarioResponse? response))
            {
                var usuario = await _usuarioRepository.GetUsuarioAsync(usuarioId);
                response = usuario?.ToResponse();

                _memoryCache.Set(key, response, _cacheOptions);
            }

            return response;
        }

        public async Task<List<UsuarioResponse>?> GetUsuarioListAsync()
        {
            if (!_memoryCache.TryGetValue(_usuarioListCacheKey, out List<UsuarioResponse>? listResponse))
            {
                var usuarioList = await _usuarioRepository.GetUsuarioListAsync();
                listResponse = usuarioList.Select(u => u.ToResponse()).ToList();

                _memoryCache.Set(_usuarioListCacheKey, listResponse, _cacheOptions);
            }

            return listResponse;
        }

        public async Task<UsuarioResponse?> UpdateUsuarioAsync(string usuarioId, UpdateUsuarioRequest request)
        {

            var usuarioDb = await _usuarioRepository.GetUsuarioAsync(usuarioId);
            if (usuarioDb == null) return null;

            usuarioDb.NomeTitular = request.NomeTitular ?? usuarioDb.NomeTitular;
            usuarioDb.Cpf = request.Cpf ?? usuarioDb.Cpf;
            if (request.StatusConta.HasValue)
                usuarioDb.StatusConta = request.StatusConta.Value;

            var success = await _usuarioRepository.UpdateUsuarioAsync(usuarioDb);

            if (success)
            {
                _memoryCache.Remove($"usuario_{usuarioId}");
                _memoryCache.Remove(_usuarioListCacheKey);
            }
            else
            {
                throw new Exception("Nenhum registro foi atualizado.");
            }
            return usuarioDb?.ToResponse();
        }
    }
}
