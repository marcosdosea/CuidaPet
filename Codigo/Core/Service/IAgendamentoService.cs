using Core.DTO;

namespace Core.Service
{
    public interface IAgendamentoService
    {
        uint Create(Agendamento agendamento);
        void Edit(Agendamento agendamento);
        void Delete(uint id);
        Agendamento? Get(uint id);
        IEnumerable<Agendamento> GetAll(int page, int pageSize);
        IEnumerable<AgendamentoDto> GetByPet(uint idPet);
        IEnumerable<AgendamentoDto> GetByTutor(uint idTutor);
        IEnumerable<AgendamentoDto> GetByFuncionario(uint idFuncionario);
        int GetCount();
    }
}
