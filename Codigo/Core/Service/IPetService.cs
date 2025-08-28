namespace Core.Service
{
    public interface IPetService
    {
        uint Create(Pet pet);
        void Edit(Pet pet);
        void Delete(uint id);
        Pet? Get(uint id);
        IEnumerable<Pet> GetAll(int page, int pageSize);
        int GetCount();
    }
}
