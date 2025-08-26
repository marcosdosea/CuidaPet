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

        /// <summary>
        /// Criar um novo produto na base de dados
        /// </summary>
        /// <param name="produto">Dados do Produto</param>
        /// <returns>ID do Produto</returns>
        public uint Create(Produto produto)
        {
            if(produto.PrecoPromocao > produto.Preco)
                throw new ServiceException("O preço promocional não pode ser maior que o preço normal.");

            context.Produtos.Add(produto);
            context.SaveChanges();
            return produto.Id;
        }

        /// <summary>
        /// Editar um produto existente na base de dados
        /// </summary>
        /// <param name="produto">Dados do Produto</param>
        public void Edit(Produto produto)
        {
            if(produto.PrecoPromocao > produto.Preco)
                throw new ServiceException("O preço promocional não pode ser maior que o preço normal.");

            context.Produtos.Update(produto);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletar um produto da base de dados
        /// </summary>
        /// <param name="id">ID do Produto</param>
        public void Delete(uint id)
        {
            var produto = context.Produtos.Find(id);
            if (produto != null)
            {
                context.Produtos.Remove(produto);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um produto na base de dados
        /// </summary>
        /// <param name="id">ID do Produto</param>
        /// <returns>Dados do Produto</returns>
        public Produto? Get(uint id)
        {
            return context.Produtos.Find(id);
        }

        /// <summary>
        /// Buscar todos os produtos na base de dados
        /// </summary>
        /// <returns>Lista de Produtos</returns>
        public IEnumerable<Produto> GetAll(int page = 1, int pageSize = 10)
        {
            return context.Produtos
                .Include(p => p.IdCategoriaNavigation)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }


        /// <summary>
        /// Buscar produtos pelo nome
        /// </summary>
        /// <param name="nome">Nome do Produto</param>
        /// <returns>Lista de Produtos</returns>
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

        /// <summary>
        /// Buscar produtos pelo estabelecimento
        /// </summary>
        /// <param name="idEstabelecimento">ID do Estabelecimento</param>
        /// <returns>Lista de Produtos</returns>
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

        /// <summary>
        /// Buscar produtos pelo nome e estabelecimento
        /// </summary>
        /// <param name="nome">Nome do Produto</param>
        /// <param name="idEstabelecimento">ID do Estabelecimento</param>
        /// <returns>Lista de Produtos</returns>
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

        /// <summary>
        /// Buscar produtos pela categoria
        /// </summary>
        /// <param name="idCategoria">ID da Categoria</param>
        /// <returns>Lista de Produtos</returns>
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

        /// <summary>
        /// Buscar produtos em promoção
        /// </summary>
        /// <returns>Lista de Produtos</returns>
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
        public int getCount()
        {
            return context.Produtos.Count();
        }
    }
}
