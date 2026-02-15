using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PetService : IPetService
    {
        private readonly CuidaPetContext context;

        public PetService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Cria um novo pet na base de dados.
        /// </summary>
        /// <param name="pet">Dados do Pet</param>
        /// <returns>ID do Pet</returns>
        public uint Create(Pet pet)
        {
            context.Pets.Add(pet);
            context.SaveChanges();
            return pet.Id;
        }

        /// <summary>
        /// Edita um pet existente na base de dados.
        /// </summary>
        /// <param name="pet">Dados do Pet</param>
        public void Edit(Pet pet)
        {
            context.Pets.Update(pet);
            context.SaveChanges();
        }

        /// <summary>
        /// Deleta um pet da base de dados.
        /// </summary>
        /// <param name="id">ID do Pet</param>
        public void Delete(uint id)
        {
            var pet = context.Pets.Find(id);
            if (pet != null)
            {
                context.Pets.Remove(pet);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um pet na base de dados.
        /// </summary>
        /// <param name="id">ID do Pet</param>
        /// <returns>Dados do Pet</returns>
        public Pet? Get(uint id)
        {
            return context.Pets.Find(id);
        }

        /// <summary>
        /// Busca todos os pets na base de dados com paginação.
        /// </summary>
        /// <param name="page">Página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>Lista de Pets</returns>
        public IEnumerable<Pet> GetAll(int page, int pageSize)
        {
            return context.Pets
                .OrderBy(p => p.Id)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Retorna a quantidade total de pets cadastrados.
        /// </summary>
        /// <returns>Total de Pets</returns>
        public int GetCount()
        {
            return context.Pets.Count();
        }
    }
}
