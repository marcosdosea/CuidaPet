using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EstabelecimentoService : IEstabelecimentoService
    {
        private readonly CuidaPetContext context;

        public EstabelecimentoService(CuidaPetContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo estabelecimento na base de dados
        /// </summary>
        /// <param name="estabelecimento">Dados do estabelecimento</param>
        /// <returns>ID do estabelecimento</returns>
        public uint Create(Estabelecimento estabelecimento)
        {
            context.Estabelecimentos.Add(estabelecimento);
            context.SaveChanges();
            return estabelecimento.Id;
        }

        /// <summary>
        /// Editar um estabelecimento existente na base de dados
        /// </summary>
        /// <param name="estabelecimento">Dados do estabelecimento</param>
        public void Edit(Estabelecimento estabelecimento)
        {
            var existingEstabelecimento = context.Estabelecimentos.Find(estabelecimento.Id);
            if (existingEstabelecimento != null)
            {
                context.Estabelecimentos.Update(estabelecimento);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletar um Estabelecimento da base de dados
        /// </summary>
        /// <param name="id">ID do Estabelecimento</param>
        public void Delete(uint id)
        {
            var estabelecimento = context.Estabelecimentos.Find(id);
            if (estabelecimento != null)
            {
                context.Estabelecimentos.Remove(estabelecimento);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um estabelecimento na base de dados
        /// </summary>
        /// <param name="id">ID do estabelecimento</param>
        /// <returns>Dados do estabelecimento</returns>
        public Estabelecimento? Get(uint id)
        {
            var estabelecimento = context.Estabelecimentos.Find(id);
            if (estabelecimento != null)
            {
                return estabelecimento;
            }

            return null;
        }

        /// <summary>
        /// Buscar todos os estabelecimentos na base de dados
        /// </summary>
        /// <returns>Lista de estabelecimentos</returns>
        public IEnumerable<Estabelecimento> GetAll(int page, int pageSize)
        {
            return context.Estabelecimentos
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        }

        /// <summary>
        /// Buscar estabelecimento pelo nome
        /// </summary>
        /// <param name="nome">Nome da Vacina</param>
        /// <returns>Lista de estabelecimento</returns>
        public IEnumerable<EstabelecimentoDto> GetByNome(string nome)
        {
            return context.Estabelecimentos
                .Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .Select(e => new EstabelecimentoDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Tipo = e.Tipo,
                    Telefone = e.Telefone,
                    Cidade = e.Cidade,
                    Bairro = e.Bairro,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Gerente = e.IdGerenteNavigation
                }).ToList();
        }

        /// <summary>
        /// Buscar estabelecimentos pelo gerente
        /// </summary>
        /// <param name="idGerente">ID do gerente</param>
        /// <returns>Lista de estabelecimentos</returns>
        public IEnumerable<EstabelecimentoDto> GetByGerente(uint idGerente)
        {
            return context.Estabelecimentos
                .Where(e => e.IdGerente == idGerente)
                .Select(e => new EstabelecimentoDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Tipo = e.Tipo,
                    Telefone = e.Telefone,
                    Cidade = e.Cidade,
                    Bairro = e.Bairro,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Gerente = e.IdGerenteNavigation
                }).ToList();
        }

        public int GetCount()
        {
            return context.Estabelecimentos.Count();
        }
    }
}
