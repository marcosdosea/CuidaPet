using Core.DTO;

namespace Core.Service
{
    public interface IDoencaService
    {
        uint Create(Doenca doenca);
        void Edit(Doenca doenca);
        void Delete(uint id);
        Doenca? Get(uint id);
        IEnumerable<Doenca> GetAll();
        IEnumerable<DoencaDTO> GetByNome(string nome);

    }
}
