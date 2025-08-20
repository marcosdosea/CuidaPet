namespace Core.Service
{
    public interface IRacaService
    {
        IEnumerable<Raca> GetAll();
        Raca? Get(uint id);
        uint Create(Raca raca);
        void Edit(Raca raca);
        void Delete(uint id);
    }
}

