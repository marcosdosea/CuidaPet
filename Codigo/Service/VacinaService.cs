using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class VacinaService : IVacinaService
    {
        private readonly CuidaPetContext context;

        public VacinaService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova vacina na base de dados
        /// </summary>
        /// <param name="vacina">Dados da vacina</param>
        /// <returns>ID da vacina</returns>
        public uint Create(Vacina vacina)
        {
            if (vacina.PeriodoEmDias < 0)
                throw new ServiceException("O período em dias não pode ser negativo.");

            context.Vacinas.Add(vacina);
            context.SaveChanges();
            return vacina.Id;
        }

        /// <summary>
        /// Editar uma vacina existente na base de dados
        /// </summary>
        /// <param name="vacina">Dados da vacina</param>
        public void Edit(Vacina vacina)
        {
            if (vacina.PeriodoEmDias < 0)
                throw new ServiceException("O período em dias não pode ser negativo.");

            var existingVacina = context.Vacinas.Find(vacina.Id);
            if (existingVacina != null)
            {
                context.Vacinas.Update(vacina);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletar uma Vacina da base de dados
        /// </summary>
        /// <param name="id">ID da Vacina</param>
        public void Delete(uint id)
        {
            var vacina = context.Vacinas.Find(id);
            if (vacina != null)
            {
                context.Vacinas.Remove(vacina);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar uma vacina na base de dados
        /// </summary>
        /// <param name="id">ID da vacina</param>
        /// <returns>Dados da vacina</returns>
        public Vacina? Get(uint id)
        {
            var vacina = context.Vacinas.Find(id);
            if (vacina != null)
            {
                return vacina;
            }

            return null;
        }

        /// <summary>
        /// Buscar todas as Vacinas na base de dados
        /// </summary>
        /// <returns>Lista de Vacinas</returns>
        public IEnumerable<Vacina> GetAll(int page, int pageSize)
        {
            return context.Vacinas
            .AsNoTracking()
            .OrderBy(v => v.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        }

        /// <summary>
        /// Buscar vacinas pelo nome
        /// </summary>
        /// <param name="nome">Nome da Vacina</param>
        /// <returns>Lista de vacinas</returns>
        public IEnumerable<VacinaDto> GetByNome(string nome)
        {
            return context.Vacinas
                .Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .Select(p => new VacinaDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PeriodoEmDias = p.PeriodoEmDias,
                    Doenca = p.IdDoencaNavigation,
                    Especie = p.IdEspecieNavigation
                }).ToList();
        }

        /// <summary>
        /// Buscar vacinas pela doença
        /// </summary>
        /// <param name="idDoenca">ID da doença</param>
        /// <returns>Lista de vacinas</returns>
        public IEnumerable<VacinaDto> GetByDoenca(uint idDoenca)
        {
            return context.Vacinas
                .Where(p => p.IdDoenca == idDoenca)
                .Select(p => new VacinaDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PeriodoEmDias = p.PeriodoEmDias,
                    Doenca = p.IdDoencaNavigation,
                    Especie = p.IdEspecieNavigation
                }).ToList();
        }

        /// <summary>
        /// Buscar vacinas pela espécie
        /// </summary>
        /// <param name="idEspecie">ID da espécie</param>
        /// <returns>Lista de vacinas</returns>
        public IEnumerable<VacinaDto> GetByEspecie(uint idEspecie)
        {
            return context.Vacinas
                .Where(p => p.IdEspecie == idEspecie)
                .Select(p => new VacinaDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PeriodoEmDias = p.PeriodoEmDias,
                    Doenca = p.IdDoencaNavigation,
                    Especie = p.IdEspecieNavigation
                }).ToList();
        }

        public int getCount()
        {
            return context.Vacinas.Count();
        }
    }
}
