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

        /// <summary>
        /// Criar uma nova espécie na base de dados
        /// </summary>
        /// <param name="especie">Dados da espécie</param>
        /// <returns>ID da espécie</returns>
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

        /// <summary>
        /// Editar uma espécie existente na base de dados
        /// </summary>
        /// <param name="especie">Dados da espécie</param>
        public void Edit(EspecieDto especie)
        {
            Especie? entity = context.Especies.Find(especie.Id);
            if (entity == null) return;
            entity.Nome = especie.Nome;
            context.SaveChanges();
        }

        /// <summary>
        /// Deletar uma espécie da base de dados
        /// </summary>
        /// <param name="id">ID da espécie</param>
        public void Delete(uint id)
        {
            Especie? entity = context.Especies.Find(id);
            if (entity == null) return;
            context.Especies.Remove(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Retorna uma espécie na base de dados
        /// </summary>
        /// <param name="id">ID da espécie</param>
        /// <returns>Dados da espécie</returns>
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

        /// <summary>
        /// Buscar todas as espécies na base de dados
        /// </summary>
        /// <returns>Lista de espécies</returns>
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

        /// <summary>
        /// Buscar espécies pelo nome na base de dados
        /// </summary>
        /// <param name="nome">Nome da Espécie</param>
        /// <returns>Lista de Espécies</returns>
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
