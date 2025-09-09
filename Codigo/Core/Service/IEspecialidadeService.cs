namespace Core.Service
{
    public interface IEspecialidadeService
    {
        uint Create(Especialidade especialidade);
        void Edit(Especialidade especialidade);
        void Delete(uint id);
        Especialidade? Get(uint id);
        IEnumerable<Especialidade> GetAll(int page, int pageSize);
        IEnumerable<Especialidade> GetByNome(string nome);
        int GetCount();
    }
}
