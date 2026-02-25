using Microsoft.OpenApi.Extensions;
using ApiUsuarioKRT.Models;
using ApiUsuarioKRT.ModelView.Response;

namespace ApiUsuarioKRT.Mapper
{
    public static class UsuarioMapper
    {
        public static UsuarioResponse ToResponse(this Usuario usuario)
        {
            return new UsuarioResponse
            {
                Id = usuario.Id,
                NomeTitular = usuario.NomeTitular,
                Cpf = usuario.Cpf,
                StatusConta = Enum.GetName(typeof(StatusConta), usuario.StatusConta)
            };
        }
    }
}
