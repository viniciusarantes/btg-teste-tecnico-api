using Microsoft.AspNetCore.Mvc;
using TesteTecnicoBTG.Models;
using TesteTecnicoBTG.ModelView.Request;
using TesteTecnicoBTG.ModelView.Response;
using TesteTecnicoBTG.Services.Interfaces;

namespace TesteTecnicoBTG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly Logger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(Logger<UsuarioController> logger, IUsuarioService usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }


        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponse>>> GetUsuarioList()
        {
            try
            {
                var usuarioList = await _usuarioService.GetUsuarioListAsync();
                return Ok(usuarioList);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Erro ao consultar lista de usuários. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new {Detail = $"Erro ao consultar lista de usuários. {ex.Message}"});
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> GetUsuarioDetail(Guid usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound(new { Detail = "Nenhum usuário encontrado." });
                }
                return Ok(new UsuarioResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao consultar usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao consultar usuário. {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> CreateUsuario([FromBody] CreateUsuarioRequest request)
        {
            try
            {
                var usuario = await _usuarioService.CreateUsuarioAsync(request);
                return Ok(new UsuarioResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao criar usuário. {ex.Message}" });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UsuarioResponse>> UpdateUsuario(Guid usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.UpdateUsuarioAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound(new { Detail = "Nenhum usuário encontrado." });
                }
                return Ok(new UsuarioResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao editar usuário. {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(Guid usuarioId)
        {
            try
            {
                var success = await _usuarioService.DeleteUsuarioAsync(usuarioId);
                if (!success)
                {
                    return NotFound(new { Detail = "Usuário não existe ou já removido." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao remover usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao remover usuário. {ex.Message}" });
            }
        }
    }
}
