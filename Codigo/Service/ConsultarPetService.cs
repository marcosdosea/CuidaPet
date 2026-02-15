using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ConsultarPetService : IConsultarPetService
    {
        private readonly CuidaPetContext context;

        public ConsultarPetService(CuidaPetContext context)
        {
            this.context = context;
        }

        public ConsultarPetDto? ObterDadosPetParaConsulta(uint idPet, uint idAgendamento)
        {
            var agendamento = context.Agendamentos
                .Include(a => a.IdPetNavigation)
                    .ThenInclude(p => p.IdRacaNavigation)
                .Include(a => a.IdPetNavigation)
                    .ThenInclude(p => p.Vacinacaos)
                        .ThenInclude(v => v.IdVacinaNavigation)
                .Include(a => a.IdPetNavigation)
                    .ThenInclude(p => p.Petdoencas)
                        .ThenInclude(pd => pd.IdDoencaNavigation)
                .Include(a => a.IdTutorNavigation)
                    .ThenInclude(t => t.IdUsuarioNavigation)
                .Include(a => a.IdFuncionarioNavigation)
                .FirstOrDefault(a => a.Id == idAgendamento && a.IdPet == idPet && a.Status == "A");

            if (agendamento == null)
                return null;

            var pet = agendamento.IdPetNavigation;
            var tutor = agendamento.IdTutorNavigation;

            var consultarPetDto = new ConsultarPetDto
            {
                IdPet = pet.Id,
                NomePet = pet.Nome,
                Raca = pet.IdRacaNavigation.Nome,
                Sexo = pet.Sexo == "M" ? "Macho" : "F�mea",
                Idade = pet.DataNascimento.HasValue 
                    ? CalcularIdade(pet.DataNascimento.Value) 
                    : null,
                NomeTutor = tutor.IdUsuarioNavigation?.UserName ?? "",
                Vacinas = pet.Vacinacaos
                    .Select(v => v.IdVacinaNavigation.Nome)
                    .Distinct()
                    .ToList(),
                Doencas = pet.Petdoencas
                    .Select(pd => pd.IdDoencaNavigation.Nome)
                    .Distinct()
                    .ToList(),
                IdAgendamento = agendamento.Id,
                IdFuncionario = agendamento.IdFuncionario,
                IdTutor = agendamento.IdTutor
            };

            return consultarPetDto;
        }

        public uint FinalizarConsulta(ConsultarPetDto consultarPetDto)
        {
            // Verificar se o agendamento existe e est� aprovado
            var agendamento = context.Agendamentos
                .FirstOrDefault(a => a.Id == consultarPetDto.IdAgendamento && a.Status == "A");

            if (agendamento == null)
                throw new Exception("Agendamento n�o encontrado ou n�o est� aprovado.");

            // Criar registro de consulta
            var consulta = new Consulta
            {
                DataConsulta = DateTime.Now,
                Anotacoes = consultarPetDto.Observacao,
                IdTutor = consultarPetDto.IdTutor,
                IdPet = consultarPetDto.IdPet,
                IdFuncionario = consultarPetDto.IdFuncionario,
                IdAgendamento = consultarPetDto.IdAgendamento
            };

            context.Consulta.Add(consulta);

            // Atualizar status do agendamento para "Realizado"
            agendamento.Status = "R";
            context.Agendamentos.Update(agendamento);

            context.SaveChanges();

            return consulta.Id;
        }

        public IEnumerable<AgendamentoConsultaDto> ObterAgendamentosAprovados(uint idFuncionario)
        {
            var agendamentos = context.Agendamentos
                .Include(a => a.IdPetNavigation)
                .Include(a => a.IdTutorNavigation)
                    .ThenInclude(t => t.IdUsuarioNavigation)
                .Where(a => a.IdFuncionario == idFuncionario && a.Status == "A")
                .OrderBy(a => a.Horario)
                .AsNoTracking()
                .Select((a, index) => new AgendamentoConsultaDto
                {
                    IdAgendamento = a.Id,
                    Numero = index + 1,
                    NomeTutor = a.IdTutorNavigation.IdUsuarioNavigation.UserName ?? "",
                    NomePet = a.IdPetNavigation.Nome,
                    Horario = a.Horario,
                    IdPet = a.IdPet
                })
                .ToList();

            // Ajustar numera��o sequencial
            for (int i = 0; i < agendamentos.Count(); i++)
            {
                agendamentos[i].Numero = i + 1;
            }

            return agendamentos;
        }

        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }
    }
}