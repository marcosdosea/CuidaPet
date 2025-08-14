using Core.DTO;

namespace Core.Service
{
    public interface IEspecieService
    {
        uint Create(EspecieDto especie);
        void Edit(EspecieDto especie);
        void Delete(uint id);
        EspecieDto? Get(uint id);
        IEnumerable<EspecieDto> GetAll();
        IEnumerable<EspecieDto> GetByNome(string nome);
    }
}
