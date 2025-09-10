using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class VacinacaoService : IVacinacaoService
    {
        private readonly CuidaPetContext context;

        public VacinacaoService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Cria um novo registro de vacinação na base de dados
        /// </summary>
        /// <param name="vacinacao">Dados da vacinação</param>
        /// <returns>ID da vacinação</returns>
        public uint Create(Vacinacao vacinacao)
        {
            context.Vacinacaos.Add(vacinacao);
            context.SaveChanges();
            return vacinacao.Id;
        }

        /// <summary>
        /// Edita um registro de vacinação existente na base de dados
        /// </summary>
        /// <param name="vacinacao">Dados da vacinação</param>
        public void Edit(Vacinacao vacinacao)
        {
            var existingVacinacao = context.Vacinacaos.Find(vacinacao.Id);
            if (existingVacinacao != null)
            {
                context.Entry(existingVacinacao).CurrentValues.SetValues(vacinacao);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta um registro de vacinação da base de dados
        /// </summary>
        /// <param name="id">ID da vacinação</param>
        public void Delete(uint id)
        {
            var vacinacao = context.Vacinacaos.Find(id);
            if (vacinacao != null)
            {
                context.Vacinacaos.Remove(vacinacao);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um registro de vacinação na base de dados
        /// </summary>
        /// <param name="id">ID da vacinação</param>
        /// <returns>Dados da vacinação</returns>
        public Vacinacao? Get(uint id)
        {
            return context.Vacinacaos
                .Include(v => v.IdVacinaNavigation)
                .Include(v => v.IdPetNavigation)
                .Include(v => v.IdFuncionarioNavigation)
                .Include(v => v.IdTutorNavigation)
                .FirstOrDefault(v => v.Id == id);
        }

        /// <summary>
        /// Busca todos os registros de vacinação na base de dados
        /// </summary>
        /// <returns>Lista de vacinações</returns>
        public IEnumerable<Vacinacao> GetAll(int page, int pageSize)
        {
            return context.Vacinacaos
                .AsNoTracking()
                .OrderBy(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// Busca vacinações por ID do pet
        /// </summary>
        /// <param name="idPet">ID do pet</param>
        /// <returns>Lista de vacinações</returns>
        public IEnumerable<VacinacaoDto> GetByPet(uint idPet)
        {
            return context.Vacinacaos
                .AsNoTracking()
                .Where(v => v.IdPet == idPet)
                .Select(v => new VacinacaoDto
                {
                    Id = v.Id,
                    DataVacina = v.DataVacina,
                    Lote = v.Lote,
                    IdVacina = v.IdVacina,
                    IdPet = v.IdPet,
                    IdFuncionario = v.IdFuncionario,
                    IdTutor = v.IdTutor
                }).ToList();
        }

        /// <summary>
        /// Busca vacinações por ID da vacina
        /// </summary>
        /// <param name="idVacina">ID da vacina</param>
        /// <returns>Lista de vacinações</returns>
        public IEnumerable<VacinacaoDto> GetByVacina(uint idVacina)
        {
            return context.Vacinacaos
                .AsNoTracking()
                .Where(v => v.IdVacina == idVacina)
                .Select(v => new VacinacaoDto
                {
                    Id = v.Id,
                    DataVacina = v.DataVacina,
                    Lote = v.Lote,
                    IdVacina = v.IdVacina,
                    IdPet = v.IdPet,
                    IdFuncionario = v.IdFuncionario,
                    IdTutor = v.IdTutor
                }).ToList();
        }

        /// <summary>
        /// Busca vacinações por ID do funcionário
        /// </summary>
        /// <param name="idFuncionario">ID do funcionário</param>
        /// <returns>Lista de vacinações</returns>
        public IEnumerable<VacinacaoDto> GetByFuncionario(uint idFuncionario)
        {
            return context.Vacinacaos
                .AsNoTracking()
                .Where(v => v.IdFuncionario == idFuncionario)
                .Select(v => new VacinacaoDto
                {
                    Id = v.Id,
                    DataVacina = v.DataVacina,
                    Lote = v.Lote,
                    IdVacina = v.IdVacina,
                    IdPet = v.IdPet,
                    IdFuncionario = v.IdFuncionario,
                    IdTutor = v.IdTutor
                }).ToList();
        }

        /// <summary>
        /// Retorna a quantidade total de vacinações
        /// </summary>
        public int GetCount()
        {
            return context.Vacinacaos.Count();
        }
    }
}