using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EspecialidadeService : IEspecialidadeService
    {
        private readonly CuidaPetContext context;

        public EspecialidadeService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova especialidade na base de dados
        /// </summary>
        /// <param name="especialidade">Dados da Especialidade</param>
        /// <returns>ID da Especialidade</returns>
        public uint Create(Especialidade especialidade)
        {
            context.Especialidades.Add(especialidade);
            context.SaveChanges();
            return especialidade.Id;
        }

        /// <summary>
        /// Editar uma especialidade existente na base de dados
        /// </summary>
        /// <param name="especialidade">Dados da Especialidade</param>
        public void Edit(Especialidade especialidade)
        {
            context.Especialidades.Update(especialidade);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletar uma especialidade da base de dados
        /// </summary>
        /// <param name="id">ID da Especialidade</param>
        public void Delete(uint id)
        {
            var especialidade = context.Especialidades.Find(id);
            if (especialidade != null)
            {
                context.Especialidades.Remove(especialidade);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma especialidade na base de dados
        /// </summary>
        /// <param name="id">ID da Especialidade</param>
        /// <returns>Dados da Especialidade</returns>
        public Especialidade? Get(uint id)
        {
            return context.Especialidades.Find(id);
        }

        /// <summary>
        /// Buscar todas as especialidades na base de dados
        /// </summary>
        /// <returns>Lista de Especialidades</returns>
        public IEnumerable<Especialidade> GetAll(int page, int pageSize)
        {
            return context.Especialidades
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<Especialidade> GetByNome(string nome)
        {
            return context.Especialidades
                .Where(e => e.Nome.Contains(nome))
                .AsNoTracking()
                .ToList();
        }
        public int GetCount()
        {
            return context.Especialidades.Count();
        }
    }
}
