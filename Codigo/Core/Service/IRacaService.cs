namespace Core.Service
{
    public interface IRacaService
    {
        IEnumerable<Raca> GetAll(int page, int pageSize);
        Raca? Get(uint id);
        uint Create(Raca raca);
        void Edit(Raca raca);
        void Delete(uint id);
        int GetCount();
    }
}

