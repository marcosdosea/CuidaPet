namespace Core.DTO
{
    public class NotificacaoDto
    {
        public uint Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descricao { get; set; }
        public DateTime DataEnvio { get; set; }
        public uint IdPessoa { get; set; }
        public bool Lida { get; set; }
    }
}