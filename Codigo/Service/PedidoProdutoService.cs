using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PedidoProdutoService : IPedidoProdutoService
    {
        private readonly CuidaPetContext context;

        public PedidoProdutoService(CuidaPetContext context)
        {
            this.context = context;
        }

        public uint Create(Pedidoproduto pedidoProduto)
        {
            context.Pedidoprodutos.Add(pedidoProduto);
            context.SaveChanges();
            return pedidoProduto.Id;
        }

        public void Edit(Pedidoproduto pedidoProduto)
        {
            context.Pedidoprodutos.Update(pedidoProduto);
            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            var pedidoProduto = context.Pedidoprodutos.Find(id);
            if (pedidoProduto != null)
            {
                context.Pedidoprodutos.Remove(pedidoProduto);
                context.SaveChanges();
            }
        }

        public Pedidoproduto? Get(uint id)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .FirstOrDefault(pp => pp.Id == id);
        }

        public IEnumerable<Pedidoproduto> GetAll(int page, int pageSize)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .OrderBy(pp => pp.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<PedidoProdutoDto> GetByStatus(string status)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedidoNavigation.Status == status)
                .Select(pp => new PedidoProdutoDto
                {
                    Id = pp.Id,
                    PedidoId = pp.IdPedido,
                    ProdutoId = pp.IdProduto,
                    Quantidade = pp.Quantidade,
                    PrecoUnitario = pp.Preco,
                    PrecoTotal = pp.Preco * pp.Quantidade,
                    RealizadoEm = pp.IdPedidoNavigation.RealizadoEm,
                    Status = pp.IdPedidoNavigation.Status,
                    ProdutoNome = pp.IdProdutoNavigation.Nome,
                    TutorId = pp.IdPedidoNavigation.IdTutorNavigation.Id,
                    TutorNome = pp.IdPedidoNavigation.IdTutorNavigation.Nome,
                    TutorTelefone = pp.IdPedidoNavigation.IdTutorNavigation.Telefone
                }).ToList();
        }

        public IEnumerable<PedidoProdutoDto> GetByTutor(uint tutorId)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedidoNavigation.IdTutorNavigation.Id == tutorId)
                .Select(pp => new PedidoProdutoDto
                {
                    Id = pp.Id,
                    PedidoId = pp.IdPedido,
                    ProdutoId = pp.IdProduto,
                    Quantidade = pp.Quantidade,
                    PrecoUnitario = pp.Preco,
                    PrecoTotal = pp.Preco * pp.Quantidade,
                    RealizadoEm = pp.IdPedidoNavigation.RealizadoEm,
                    Status = pp.IdPedidoNavigation.Status,
                    ProdutoNome = pp.IdProdutoNavigation.Nome,
                    TutorId = pp.IdPedidoNavigation.IdTutorNavigation.Id,
                    TutorNome = pp.IdPedidoNavigation.IdTutorNavigation.Nome,
                    TutorTelefone = pp.IdPedidoNavigation.IdTutorNavigation.Telefone
                }).ToList();
        }

        public void AlterarStatus(uint id, string novoStatus)
        {
            var pedidoProduto = context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .FirstOrDefault(pp => pp.Id == id);

            if (pedidoProduto != null && pedidoProduto.IdPedidoNavigation.Status == "Pendente")
            {
                pedidoProduto.IdPedidoNavigation.Status = novoStatus;
                context.SaveChanges();
            }
        }

        public PedidoProdutoDto? GetDetalhes(uint id)
        {
            var pp = context.Pedidoprodutos
                .Include(p => p.IdPedidoNavigation)
                .Include(p => p.IdProdutoNavigation)
                .FirstOrDefault(p => p.Id == id);

            if (pp == null) return null;

            return new PedidoProdutoDto
            {
                Id = pp.Id,
                PedidoId = pp.IdPedido,
                ProdutoId = pp.IdProduto,
                Quantidade = pp.Quantidade,
                PrecoUnitario = pp.Preco,
                PrecoTotal = pp.Preco * pp.Quantidade,
                RealizadoEm = pp.IdPedidoNavigation.RealizadoEm,
                Status = pp.IdPedidoNavigation.Status,
                ProdutoNome = pp.IdProdutoNavigation.Nome,
                TutorId = pp.IdPedidoNavigation.IdTutorNavigation.Id,
                TutorNome = pp.IdPedidoNavigation.IdTutorNavigation.Nome,
                TutorTelefone = pp.IdPedidoNavigation.IdTutorNavigation.Telefone
            };
        }
    }
}
