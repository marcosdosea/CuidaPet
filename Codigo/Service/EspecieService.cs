using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EspecieService : IEspecieService
    {
        private readonly CuidaPetContext context;

        public EspecieService(CuidaPetContext context)
        {
            this.context = context;
        }

        public uint Create(EspecieDto especie)
        {
            Especie entity = new()
            {
                Nome = especie.Nome
            };
            context.Especies.Add(entity);
            context.SaveChanges();
            return entity.Id;
        }

        public void Edit(EspecieDto especie)
        {
            Especie? entity = context.Especies.Find(especie.Id);
            if (entity == null) return;
            entity.Nome = especie.Nome;
            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            Especie? entity = context.Especies.Find(id);
            if (entity == null) return;
            context.Especies.Remove(entity);
            context.SaveChanges();
        }

        public EspecieDto? Get(uint id)
        {
            return context.Especies
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EspecieDto
                {
                    Id = e.Id,
                    Nome = e.Nome
                })
                .FirstOrDefault();
        }

        public IEnumerable<EspecieDto> GetAll()
        {
            return context.Especies
                .AsNoTracking()
                .Select(e => new EspecieDto
                {
                    Id = e.Id,
                    Nome = e.Nome
                })
                .ToList();
        }

        public IEnumerable<EspecieDto> GetByNome(string nome)
        {
            return context.Especies
                .AsNoTracking()
                .Where(e => e.Nome.Contains(nome))
                .Select(e => new EspecieDto
                {
                    Id = e.Id,
                    Nome = e.Nome
                })
                .ToList();
        }
    }
}
