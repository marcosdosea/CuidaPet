using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class RacaService : IRacaService
    {
        private readonly CuidaPetContext context;

        public RacaService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova raça na base de dados
        /// </summary>
        /// <param name="raca">Dados da Raça</param>
        /// <returns>ID da Raça</returns>
        public uint Create(Raca raca)
        {
            context.Racas.Add(raca);
            context.SaveChanges();
            return raca.Id;
        }

        /// <summary>
        /// Editar uma raça existente na base de dados
        /// </summary>
        /// <param name="raca">Dados da Raça</param>
        public void Edit(Raca raca)
        {
            context.Racas.Update(raca);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletar uma raça da base de dados
        /// </summary>
        /// <param name="id">ID da Raça</param>
        public void Delete(uint id)
        {
            var raca = context.Racas.Find(id);
            if (raca != null)
            {
                context.Racas.Remove(raca);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma raça na base de dados
        /// </summary>
        /// <param name="id">ID da Raça</param>
        /// <returns>Dados da Raça</returns>
        public Raca? Get(uint id)
        {
            return context.Racas.Find(id);
        }

        /// <summary>
        /// Buscar todas as raças na base de dados
        /// </summary>
        /// <returns>Lista de Raças</returns>
        public IEnumerable<Raca> GetAll(int page, int pageSize)
        {
            return context.Racas
                .Include(r => r.IdEspecieNavigation)
                .OrderBy(r => r.Id)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int getCount()
        {
            return context.Racas.Count();
        }
    }
}

