using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly CuidaPetContext context;

        public AgendamentoService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo agendamento na base de dados
        /// </summary>
        /// <param name="agendamento">Dados do Agendamento</param>
        /// <returns>ID do Agendamento</returns>
        public uint Create(Agendamento agendamento)
        {
            context.Agendamentos.Add(agendamento);
            context.SaveChanges();
            return agendamento.Id;
        }

        /// <summary>
        /// Editar um agendamento existente na base de dados
        /// </summary>
        /// <param name="agendamento">Dados do Agendamento</param>
        public void Edit(Agendamento agendamento)
        {
            context.Agendamentos.Update(agendamento);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletar um agendamento da base de dados
        /// </summary>
        /// <param name="id">ID do Agendamento</param>
        public void Delete(uint id)
        {
            var agendamento = context.Agendamentos.Find(id);
            if (agendamento != null)
            {
                context.Agendamentos.Remove(agendamento);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um agendamento na base de dados
        /// </summary>
        /// <param name="id">ID do Agendamento</param>
        /// <returns>Dados do Agendamento</returns>
        public Agendamento? Get(uint id)
        {
            return context.Agendamentos.Find(id);
        }

        /// <summary>
        /// Buscar todos os agendamentos na base de dados
        /// </summary>
        /// <returns>Lista de Agendamentos</returns>
        public IEnumerable<Agendamento> GetAll(int page = 1, int pageSize = 10)
        {
            return context.Agendamentos
                .Include(p => p.IdPetNavigation)
                .Include(p => p.IdTutorNavigation)
                .Include(p => p.IdFuncionarioNavigation)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Buscar agendamentos pelo pet
        /// </summary>
        /// <param name="idPet">ID do Pet</param>
        /// <returns>Lista de Agendamentos</returns>
        public IEnumerable<AgendamentoDto> GetByPet(uint idPet)
        {
            return context.Agendamentos
                .Where(a => a.IdPet == idPet)
                .Select(a => new AgendamentoDto
                {
                    Id = a.Id,
                    DataSolicitacao = a.DataSolicitacao,
                    DataConfirmacao = a.DataConfirmacao,
                    Horario = a.Horario,
                    Status = a.Status,
                    IdPet = a.IdPet,
                    IdFuncionario = a.IdFuncionario,
                    IdTutor = a.IdTutor,
                }).ToList();
        }

        /// <summary>
        /// Buscar agendamentos pelo funcionario
        /// </summary>
        /// <param name="idFuncionario">ID do Funcionario</param>
        /// <returns>Lista de Agendamentos</returns>
        public IEnumerable<AgendamentoDto> GetByFuncionario(uint idFuncionario)
        {
            return context.Agendamentos
                .Where(a => a.IdFuncionario == idFuncionario)
                .Select(a => new AgendamentoDto
                {
                    Id = a.Id,
                    DataSolicitacao = a.DataSolicitacao,
                    DataConfirmacao = a.DataConfirmacao,
                    Horario = a.Horario,
                    Status = a.Status,
                    IdPet = a.IdPet,
                    IdFuncionario = a.IdFuncionario,
                    IdTutor = a.IdTutor,
                }).ToList();
        }

        /// <summary>
        /// Buscar agendamentos pelo tutor
        /// </summary>
        /// <param name="idTutor">ID do Tutor</param>
        /// <returns>Lista de Agendamentos</returns>
        public IEnumerable<AgendamentoDto> GetByTutor(uint idTutor)
        {
            return context.Agendamentos
                .Where(a => a.IdTutor == idTutor)
                .Select(a => new AgendamentoDto
                {
                    Id = a.Id,
                    DataSolicitacao = a.DataSolicitacao,
                    DataConfirmacao = a.DataConfirmacao,
                    Horario = a.Horario,
                    Status = a.Status,
                    IdPet = a.IdPet,
                    IdFuncionario = a.IdFuncionario,
                    IdTutor = a.IdTutor,
                }).ToList();
        }

        public int GetCount()
        {
            return context.Agendamentos.Count();
        }
    }
}
