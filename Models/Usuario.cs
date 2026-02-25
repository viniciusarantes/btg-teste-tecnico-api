namespace TesteTecnicoBTG.Models
{
    public enum StatusConta
    {
        Inativo = 0,
        Ativo = 1,
    }

    public class Usuario
    {
        public Guid Id { get; set; }
        public string? NomeTitular { get; set; }
        public string? Cpf { get; set; }
        public StatusConta StatusConta { get; set; } = StatusConta.Ativo;
    }
}
