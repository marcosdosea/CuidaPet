using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class DoencaService : IDoencaService
    {
        private readonly CuidaPetContext context;

        public DoencaService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Cria uma nova doença na base de dados
        /// </summary>
        /// <param name="doenca">Dados da Doença</param>
        /// <returns>ID da Doença</returns>
        public uint Create(Doenca doenca)
        {
            context.Doencas.Add(doenca);
            context.SaveChanges();
            return doenca.Id;
        }

        /// <summary>
        /// Edita uma doença existente na base de dados
        /// </summary>
        /// <param name="doenca">Dados da Doença</param>
        public void Edit(Doenca doenca)
        {
            Doenca? doencaExistente = context.Doencas.Find(doenca.Id);
            if (doencaExistente != null)
            {
                doencaExistente.Nome = doenca.Nome;
                doencaExistente.IdEspecie = doenca.IdEspecie;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta uma doença da base de dados
        /// </summary>
        /// <param name="id">ID da Doença</param>
        public void Delete(uint id)
        {
            var doenca = context.Doencas.Find(id);
            if (doenca != null)
            {
                context.Doencas.Remove(doenca);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca uma doença na base de dados
        /// </summary>
        /// <param name="id">ID da Doença</param>
        /// <returns>Dados da Doenças</returns>
        public Doenca? Get(uint id)
        {
            return context.Doencas.Find(id);
        }

        /// <summary>
        /// Buscar todos as doenças na base de dados
        /// </summary>
        /// <returns>Lista de Doenças</returns>
        public IEnumerable<Doenca> GetAll(int page, int pageSize)
        {
            return context.Doencas
                .Include(d => d.IdEspecieNavigation)
                .OrderBy(d => d.Id)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Buscar doenças pelo nome
        /// </summary>
        /// <param name="nome">Nome da Doença</param>
        /// <returns>Lista de Doenças</returns>
        public IEnumerable<Doenca> GetByNome(string nome)
        {
            return context.Doencas
                .AsNoTracking()
                .Where(d => d.Nome.Contains(nome))
                .Select(d => new Doenca
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    IdEspecie = d.IdEspecie
                })
                .ToList();
        }

        public int GetCount()
        {
            return context.Doencas.Count();
        }
    }
}
