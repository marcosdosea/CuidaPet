namespace Core;

public partial class Vacina
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public ushort? PeriodoEmDias { get; set; }

    public uint IdDoenca { get; set; }

    public uint IdEspecie { get; set; }

    public virtual Doenca IdDoencaNavigation { get; set; } = null!;

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Vacinacao> Vacinacaos { get; set; } = new List<Vacinacao>();
}
