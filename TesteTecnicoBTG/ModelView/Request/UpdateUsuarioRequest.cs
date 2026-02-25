using ApiUsuarioKRT.Models;

namespace ApiUsuarioKRT.ModelView.Request
{
    public class UpdateUsuarioRequest
    {
        public string? NomeTitular { get; set; }
        public string? Cpf { get; set; }
        public StatusConta? StatusConta { get; set; }
    }
}
