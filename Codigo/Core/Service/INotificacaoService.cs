namespace Core.Service
{
    public interface INotificacaoService
    {
        // Métodos CRUD com entidade
        uint Create(Notificacao notificacao);
        void Edit(Notificacao notificacao);
        void Delete(uint id);
        Notificacao? Get(uint id);
        IEnumerable<Notificacao> GetAll(int page, int pageSize);
        int GetCount();

        // Métodos específicos do domínio
        void EnviarNotificacao(string titulo, string mensagem, uint idPessoa);
        List<Notificacao> ObterNotificacoesPorPessoa(uint idPessoa);
        void MarcarComoLida(uint idNotificacao, uint idPessoa);

        // Casos de uso específicos
        void NotificarAprovacaoPedido(uint idPedido);
        void NotificarAgendamento(uint idAgendamento);
        void NotificarRenovacaoVacina(uint idPet);
        void VerificarVacinasVencendo();
    }
}
