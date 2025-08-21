using System.ComponentModel.DataAnnotations;

namespace Core;

public partial class Especie
{
    public uint Id { get; set; }

    [Required(ErrorMessage = "O nome da espécie é obrigatório.")]
    public string Nome { get; set; } = null!;

    public virtual ICollection<Doenca> Doencas { get; set; } = new List<Doenca>();

    public virtual ICollection<Raca> Racas { get; set; } = new List<Raca>();

    public virtual ICollection<Vacina> Vacinas { get; set; } = new List<Vacina>();
}
