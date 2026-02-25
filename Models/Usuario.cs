namespace TesteTecnicoBTG.Models
{
    public enum StatusConta
    {
        Inativo = 0,
        Ativo = 1,
    }

    public class Usuario
    {
        public string Id { get; set; }
        public string? NomeTitular { get; set; }
        public string? Cpf { get; set; }
        public StatusConta StatusConta { get; set; } = StatusConta.Ativo;
        public int isDeleted { get; set; }
    }
}
