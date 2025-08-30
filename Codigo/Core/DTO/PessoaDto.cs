namespace Core.DTO
{
    public class PessoaDto
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string? Telefone { get; set; }
        public string Tipo { get; set; } = "T";
        public string Status { get; set; } = null!;
        public string Logradouro { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
