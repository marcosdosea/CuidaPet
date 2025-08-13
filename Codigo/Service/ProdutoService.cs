using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ProdutoService : IProdutoService
    {
        private readonly CuidaPetContext context;

        public ProdutoService(CuidaPetContext context)
        {
            this.context = context;
        }

        public uint Create(Produto produto)
        {
            context.Produtos.Add(produto);
            context.SaveChanges();
            return produto.Id;
        }

        public void Edit(Produto produto)
        {
            context.Produtos.Update(produto);
            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            var produto = context.Produtos.Find(id);
            if (produto != null)
            {
                context.Produtos.Remove(produto);
                context.SaveChanges();
            }
        }

        public Produto? Get(uint id)
        {
            return context.Produtos.Find(id);
        }

        public IEnumerable<Produto> GetAll()
        {
            return context.Produtos.AsNoTracking().ToList();
        }

        public IEnumerable<ProdutoDTO> GetByNome(string nome)
        {
            return context.Produtos
                .Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    PrecoPromocao = p.PrecoPromocao,
                    Categoria = p.IdCategoriaNavigation != null ? p.IdCategoriaNavigation.Nome : string.Empty,
                    Estabelecimento = p.IdEstabelecimentoNavigation != null ? p.IdEstabelecimentoNavigation.Nome : string.Empty
                }).ToList();
        }

        public IEnumerable<ProdutoDTO> GetByEstabelecimento(uint idEstabelecimento)
        {
            return context.Produtos
                .Where(p => p.IdEstabelecimento == idEstabelecimento)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    PrecoPromocao = p.PrecoPromocao,
                    Categoria = p.IdCategoriaNavigation != null ? p.IdCategoriaNavigation.Nome : string.Empty,
                    Estabelecimento = p.IdEstabelecimentoNavigation != null ? p.IdEstabelecimentoNavigation.Nome : string.Empty
                }).ToList();
        }

        public IEnumerable<ProdutoDTO> GetByNomeAndEstabelecimento(string nome, uint idEstabelecimento)
        {
            return context.Produtos
                .Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase) && p.IdEstabelecimento == idEstabelecimento)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    PrecoPromocao = p.PrecoPromocao,
                    Categoria = p.IdCategoriaNavigation != null ? p.IdCategoriaNavigation.Nome : string.Empty,
                    Estabelecimento = p.IdEstabelecimentoNavigation != null ? p.IdEstabelecimentoNavigation.Nome : string.Empty
                }).ToList();
        }

        public IEnumerable<ProdutoDTO> GetByCategoria(uint idCategoria)
        {
            return context.Produtos
                .Where(p => p.IdCategoria == idCategoria)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    PrecoPromocao = p.PrecoPromocao,
                    Categoria = p.IdCategoriaNavigation != null ? p.IdCategoriaNavigation.Nome : string.Empty,
                    Estabelecimento = p.IdEstabelecimentoNavigation != null ? p.IdEstabelecimentoNavigation.Nome : string.Empty
                }).ToList();
        }

        public IEnumerable<ProdutoDTO> GetProdutosPromocao()
        {
            return context.Produtos
                .Where(p => p.Status == "P" && p.PrecoPromocao.HasValue)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Descricao = p.Descricao,
                    Status = p.Status,
                    PrecoPromocao = p.PrecoPromocao,
                    Categoria = p.IdCategoriaNavigation != null ? p.IdCategoriaNavigation.Nome : string.Empty,
                    Estabelecimento = p.IdEstabelecimentoNavigation != null ? p.IdEstabelecimentoNavigation.Nome : string.Empty
                }).ToList();
        }
    }
}
