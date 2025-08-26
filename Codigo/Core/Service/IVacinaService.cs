using Core.DTO;

namespace Core.Service
{
    public interface IVacinaService
    {
        uint Create(Vacina vacina);
        void Edit(Vacina vacina);
        void Delete(uint id);
        Vacina? Get(uint id);
        IEnumerable<Vacina> GetAll(int page, int pageSize);
        IEnumerable<VacinaDto> GetByNome(string nome);
        IEnumerable<VacinaDto> GetByDoenca(uint idDoenca);
        IEnumerable<VacinaDto> GetByEspecie(uint idEspecie);
        int GetCount();
    }
}
