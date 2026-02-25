using Microsoft.AspNetCore.Mvc;
using ApiUsuarioKRT.Models;
using ApiUsuarioKRT.ModelView.Request;
using ApiUsuarioKRT.ModelView.Response;
using ApiUsuarioKRT.Services.Interfaces;

namespace ApiUsuarioKRT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService)
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
        public async Task<ActionResult<UsuarioResponse>> GetUsuarioDetail(string id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { Detail = "Nenhum usuário encontrado." });
                }
                return Ok(usuario);
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
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao criar usuário. {ex.Message}" });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UsuarioResponse>> UpdateUsuario(string id, [FromBody] UpdateUsuarioRequest request)
        {
            try
            {
                var usuario = await _usuarioService.UpdateUsuarioAsync(id, request);
                if (usuario == null)
                {
                    return NotFound(new { Detail = "Nenhum usuário encontrado." });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar usuário. {ex.Message}. {ex.StackTrace}");
                return StatusCode(500, new { Detail = $"Erro ao editar usuário. {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(string id)
        {
            try
            {
                var success = await _usuarioService.DeleteUsuarioAsync(id);
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
