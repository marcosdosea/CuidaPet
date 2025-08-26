using Core.DTO;

namespace Core.Service
{
    public interface IEspecieService
    {
        uint Create(Especie especie);
        void Edit(Especie especie);
        void Delete(uint id);
        Especie? Get(uint id);
        IEnumerable<Especie> GetAll(int page, int pageSize);
        IEnumerable<EspecieDto> GetByNome(string nome);
        int getCount();
    }
}
