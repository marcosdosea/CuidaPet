using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly CuidaPetContext context;

        public NotificacaoService(CuidaPetContext context)
        {
            this.context = context;
        }

        #region Métodos CRUD

        public uint Create(Notificacao notificacao)
        {
            context.Notificacaos.Add(notificacao);
            context.SaveChanges();
            return notificacao.Id;
        }

        public void Edit(Notificacao notificacao)
        {
            var entity = context.Notificacaos.Find(notificacao.Id);
            if (entity == null) return;

            entity.Titulo = notificacao.Titulo;
            entity.Descricao = notificacao.Descricao;
            entity.DataEnvio = notificacao.DataEnvio;

            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            var entity = context.Notificacaos.Find(id);
            if (entity == null) return;

            // Remove relacionamentos pessoa-notificação primeiro
            var pessoaNotificacoes = context.Pessoanotificacaos
                .Where(pn => pn.IdNotificacao == id)
                .ToList();

            context.Pessoanotificacaos.RemoveRange(pessoaNotificacoes);

            // Remove a notificação
            context.Notificacaos.Remove(entity);
            context.SaveChanges();
        }

        public Notificacao? Get(uint id)
        {
            return context.Notificacaos
                .AsNoTracking()
                .FirstOrDefault(n => n.Id == id);
        }

        public IEnumerable<Notificacao> GetAll(int page, int pageSize)
        {
            return context.Notificacaos
                .AsNoTracking()
                .OrderByDescending(n => n.DataEnvio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetCount()
        {
            return context.Notificacaos.Count();
        }

        #endregion

        #region Métodos específicos do domínio

        public void EnviarNotificacao(string titulo, string mensagem, uint idPessoa)
        {
            var notificacao = new Notificacao
            {
                Titulo = titulo,
                Descricao = mensagem,
                DataEnvio = DateTime.Now
            };

            context.Notificacaos.Add(notificacao);
            context.SaveChanges();

            var pessoaNotificacao = new Pessoanotificacao
            {
                IdNotificacao = notificacao.Id,
                IdPessoa = idPessoa,
                StatusLida = 0
            };

            context.Pessoanotificacaos.Add(pessoaNotificacao);
            context.SaveChanges();
        }

        public List<Notificacao> ObterNotificacoesPorPessoa(uint idPessoa)
        {
            return context.Pessoanotificacaos
                .AsNoTracking()
                .Include(pn => pn.IdNotificacaoNavigation)
                .Where(pn => pn.IdPessoa == idPessoa)
                .OrderByDescending(pn => pn.IdNotificacaoNavigation.DataEnvio)
                .Select(pn => pn.IdNotificacaoNavigation)
                .ToList();
        }

        public List<NotificacaoDto> ObterNotificacoesComStatusPorPessoa(uint idPessoa)
        {
            return context.Pessoanotificacaos
                .AsNoTracking()
                .Include(pn => pn.IdNotificacaoNavigation)
                .Where(pn => pn.IdPessoa == idPessoa)
                .OrderByDescending(pn => pn.IdNotificacaoNavigation.DataEnvio)
                .Select(pn => new NotificacaoDto
                {
                    Id = pn.IdNotificacaoNavigation.Id,
                    Titulo = pn.IdNotificacaoNavigation.Titulo,
                    Descricao = pn.IdNotificacaoNavigation.Descricao,
                    DataEnvio = pn.IdNotificacaoNavigation.DataEnvio,
                    IdPessoa = pn.IdPessoa,
                    Lida = pn.StatusLida == 1
                })
                .ToList();
        }

        public int ObterContagemNaoLidas(uint idPessoa)
        {
            return context.Pessoanotificacaos
                .AsNoTracking()
                .Count(pn => pn.IdPessoa == idPessoa && pn.StatusLida == 0);
        }

        public void MarcarComoLida(uint idNotificacao, uint idPessoa)
        {
            var pessoaNotificacao = context.Pessoanotificacaos
                .FirstOrDefault(pn => pn.IdNotificacao == idNotificacao && pn.IdPessoa == idPessoa);

            if (pessoaNotificacao != null)
            {
                pessoaNotificacao.StatusLida = 1;
                context.SaveChanges();
            }
        }

        #endregion

        #region Métodos auxiliares para casos de uso específicos

        public void NotificarAprovacaoPedido(uint idPedido)
        {
            var pedido = context.Pedidos
                .Include(p => p.IdTutorNavigation)
                .FirstOrDefault(p => p.Id == idPedido);

            if (pedido != null)
            {
                var titulo = "Pedido Aprovado";
                var mensagem = $"Seu pedido #{pedido.Id} foi aprovado e está sendo processado.";

                EnviarNotificacao(titulo, mensagem, pedido.IdTutor);
            }
        }

        public void NotificarAgendamento(uint idAgendamento)
        {
            var agendamento = context.Agendamentos
                .Include(a => a.IdTutorNavigation)
                .Include(a => a.IdPetNavigation)
                .FirstOrDefault(a => a.Id == idAgendamento);

            if (agendamento != null)
            {
                string titulo;
                string mensagem;

                switch (agendamento.Status)
                {
                    case "A":
                        titulo = "Agendamento Confirmado";
                        mensagem = $"Seu agendamento para {agendamento.IdPetNavigation.Nome} foi confirmado para {agendamento.DataConfirmacao?.ToString("dd/MM/yyyy")} às {agendamento.Horario}.";
                        break;
                    case "C":
                        titulo = "Agendamento Cancelado";
                        mensagem = $"Seu agendamento para {agendamento.IdPetNavigation.Nome} foi cancelado.";
                        break;
                    case "R":
                        titulo = "Consulta Realizada";
                        mensagem = $"A consulta do {agendamento.IdPetNavigation.Nome} foi realizada com sucesso.";
                        break;
                    default:
                        return;
                }

                EnviarNotificacao(titulo, mensagem, agendamento.IdTutor);
            }
        }

        public void NotificarRenovacaoVacina(uint idPet)
        {
            var pet = context.Pets
                .Include(p => p.Vacinacaos)
                    .ThenInclude(v => v.IdVacinaNavigation)
                .Include(p => p.IdRacaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .FirstOrDefault(p => p.Id == idPet);

            if (pet == null) return;

            var tutor = context.Pessoapets
                .Include(pp => pp.IdPessoaNavigation)
                .Where(pp => pp.IdPet == idPet)
                .Select(pp => pp.IdPessoaNavigation)
                .FirstOrDefault();

            if (tutor == null) return;

            var dataAtual = DateTime.Now;

            var vacinasParaRenovar = context.Vacinas
                .Where(v => v.IdEspecie == pet.IdRacaNavigation.IdEspecie)
                .ToList();

            foreach (var vacina in vacinasParaRenovar)
            {
                var ultimaVacinacao = pet.Vacinacaos
                    .Where(v => v.IdVacina == vacina.Id)
                    .OrderByDescending(v => v.DataVacina)
                    .FirstOrDefault();

                if (ultimaVacinacao != null)
                {
                    var periodoEmDias = vacina.PeriodoEmDias ?? 0;
                    if (periodoEmDias == 0)
                        continue;

                    var proximaData = ultimaVacinacao.DataVacina.AddDays(periodoEmDias);
                    var diasParaVencer = (proximaData - dataAtual).Days;

                    if (diasParaVencer <= 30 && diasParaVencer >= 0)
                    {
                        var titulo = "Renovação de Vacina";
                        var mensagem = $"A vacina {vacina.Nome} do {pet.Nome} vence em {diasParaVencer} dias ({proximaData:dd/MM/yyyy}). Agende uma consulta para renovação.";

                        EnviarNotificacao(titulo, mensagem, tutor.Id);
                    }
                }
            }
        }

        public void VerificarVacinasVencendo()
        {
            var pets = context.Pets.ToList();

            foreach (var pet in pets)
            {
                NotificarRenovacaoVacina(pet.Id);
            }
        }

        #endregion
    }
}