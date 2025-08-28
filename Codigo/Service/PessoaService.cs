using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PessoaService : IPessoaService
    {
        private readonly CuidaPetContext context;

        public PessoaService(CuidaPetContext context)
        {
            this.context = context;
        }

        public uint Create(Pessoa pessoa)
        {
            context.Pessoas.Add(pessoa);
            context.SaveChanges();
            return pessoa.Id;
        }

        public void Delete(uint id)
        {
            var entity = context.Pessoas.Find(id);
            if (entity != null)
            {
                entity.Status = "I"; // Inativo
                context.SaveChanges();
            }
        }

        public void Edit(Pessoa pessoa)
        {
            var entity = context.Pessoas.Find(pessoa.Id);

            if (entity != null)
            {
                context.Pessoas.Update(pessoa);
                context.SaveChanges();
            }
        }

        public Pessoa? Get(uint id)
        {
            return context.Pessoas.Find(id);
        }

        public IEnumerable<Pessoa> GetAll(int page, int pageSize)
        {
            return context.Pessoas
                .AsNoTracking()
                .Where(p => p.Tipo == "T" && p.Status == "A")
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetCount()
        {
            return context.Pessoas.Count(p => p.Tipo == "T" && p.Status == "A");
        }

        // TODO: CPF
        // public string GetCpf(uint idPessoa);
    }
}
