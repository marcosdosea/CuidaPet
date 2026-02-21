using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            var pessoa = context.Pessoas.Find(id);
            if (pessoa != null)
            {
                pessoa.Status = "I"; // Inativo
                context.SaveChanges();
            }
        }

        public void Edit(Pessoa pessoa)
        {
            if (pessoa != null)
            {
                context.Pessoas.Update(pessoa);
                context.SaveChanges();
            }
        }

        public Pessoa? Get(uint id)
        {
            return context.Pessoas
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pessoa> GetAll(int page, int pageSize)
        {
            return context.Pessoas
                .Include(p => p.IdUsuarioNavigation)
                .AsNoTracking()
                .Where(p => p.Status == "A")
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetCount()
        {
            return context.Pessoas.Count(p => p.Status == "A");
        }

        public Pessoa? GetByCpf(string cpf)
        {
            return context.Pessoas
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefault(p => p.Cpf == cpf);
        }

        public IEnumerable<Pessoa> GetGerentes()
        {
            var query = from pessoa in context.Pessoas.Include(p => p.IdUsuarioNavigation)
                        join userRole in context.UserRoles on pessoa.IdUsuario equals userRole.UserId
                        join role in context.Roles on userRole.RoleId equals role.Id
                        where role.Name == "Gerente" && pessoa.Status == "A"
                        orderby pessoa.IdUsuarioNavigation.UserName
                        select pessoa;

            return query.AsNoTracking().ToList();
        }

        public IEnumerable<Pessoa> GetTutores()
        {
            var query = from pessoa in context.Pessoas.Include(p => p.IdUsuarioNavigation)
                        join userRole in context.UserRoles on pessoa.IdUsuario equals userRole.UserId
                        join role in context.Roles on userRole.RoleId equals role.Id
                        where role.Name == "Tutor" && pessoa.Status == "A"
                        orderby pessoa.IdUsuarioNavigation.UserName
                        select pessoa;

            return query.AsNoTracking().ToList() ?? new List<Pessoa>();
        }
    }
}
