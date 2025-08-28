using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PetDto
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Sexo { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public uint IdRaca { get; set; }
    }
}
