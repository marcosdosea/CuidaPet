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
        IEnumerable<VacinaDTO> GetByNome(string nome);
        IEnumerable<VacinaDTO> GetByDoenca(uint idDoenca);
        IEnumerable<VacinaDTO> GetByEspecie(uint idEspecie);
        int getCount();
    }
}
