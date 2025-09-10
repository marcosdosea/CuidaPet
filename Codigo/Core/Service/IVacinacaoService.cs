using Core.DTO;

namespace Core.Service
{
    public interface IVacinacaoService
    {
        uint Create(Vacinacao vacinacao);
        void Edit(Vacinacao vacinacao);
        void Delete(uint id);
        Vacinacao? Get(uint id);
        IEnumerable<Vacinacao> GetAll(int page, int pageSize);
        IEnumerable<VacinacaoDto> GetByPet(uint idPet);
        IEnumerable<VacinacaoDto> GetByVacina(uint idVacina);
        IEnumerable<VacinacaoDto> GetByFuncionario(uint idFuncionario);
        int GetCount();
    }
}
