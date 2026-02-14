using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Context;
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
                    .ThenInclude(p => p.IdTutorNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .FirstOrDefault(pp => pp.Id == id);
        }

        public IEnumerable<Pedidoproduto> GetAll(int page, int pageSize)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                    .ThenInclude(p => p.IdTutorNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedidoNavigation.Status == "A" || pp.IdPedidoNavigation.Status == "F") // Filtrar apenas pendentes e concluídos
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
                    .ThenInclude(p => p.IdTutorNavigation)
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
                    .ThenInclude(p => p.IdTutorNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedidoNavigation.IdTutorNavigation.Id == tutorId && 
                           (pp.IdPedidoNavigation.Status == "A" || pp.IdPedidoNavigation.Status == "F")) // Filtrar apenas pendentes e concluídos
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

            if (pedidoProduto != null && pedidoProduto.IdPedidoNavigation.Status == "A")
            {
                pedidoProduto.IdPedidoNavigation.Status = novoStatus;
                context.SaveChanges();
            }
        }

        public PedidoProdutoDto? GetDetalhes(uint id)
        {
            var pp = context.Pedidoprodutos
                .Include(p => p.IdPedidoNavigation)
                    .ThenInclude(pedido => pedido.IdTutorNavigation)
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

        // Buscar todos os itens de um pedido específico
        public IEnumerable<PedidoProdutoDto> GetItensByPedidoId(uint pedidoId)
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                    .ThenInclude(p => p.IdTutorNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedido == pedidoId)
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

        // Novo método para recusar pedido: deletar itens e marcar pedido como cancelado
        public void RecusarPedido(uint pedidoProdutoId)
        {
            var pedidoProduto = context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .FirstOrDefault(pp => pp.Id == pedidoProdutoId);

            if (pedidoProduto != null && pedidoProduto.IdPedidoNavigation.Status == "A")
            {
                var pedidoId = pedidoProduto.IdPedido;

                // Deletar todos os itens do pedido
                var itensParaDeletar = context.Pedidoprodutos
                    .Where(pp => pp.IdPedido == pedidoId)
                    .ToList();

                context.Pedidoprodutos.RemoveRange(itensParaDeletar);

                // Marcar o pedido como cancelado (C)
                var pedido = context.Pedidos.Find(pedidoId);
                if (pedido != null)
                {
                    pedido.Status = "C";
                }

                context.SaveChanges();
            }
        }

        // Novo método para buscar pedidos ativos com ordenação
        public IEnumerable<PedidoProdutoDto> GetPedidosAtivos(int page, int pageSize, string? sortBy = null, bool descending = false)
        {
            var query = context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                    .ThenInclude(p => p.IdTutorNavigation)
                .Include(pp => pp.IdProdutoNavigation)
                .Where(pp => pp.IdPedidoNavigation.Status == "A" || pp.IdPedidoNavigation.Status == "F")
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
                });

            // Aplicar ordenação se especificada
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "id" => descending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                    "tutor" => descending ? query.OrderByDescending(p => p.TutorNome) : query.OrderBy(p => p.TutorNome),
                    "produto" => descending ? query.OrderByDescending(p => p.ProdutoNome) : query.OrderBy(p => p.ProdutoNome),
                    "data" => descending ? query.OrderByDescending(p => p.RealizadoEm) : query.OrderBy(p => p.RealizadoEm),
                    "status" => descending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
                    _ => query.OrderByDescending(p => p.RealizadoEm) // Padrão: mais recentes primeiro
                };
            }
            else
            {
                query = query.OrderByDescending(p => p.RealizadoEm); // Padrão: mais recentes primeiro
            }

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();
        }

        // Contar total de pedidos ativos
        public int GetCountPedidosAtivos()
        {
            return context.Pedidoprodutos
                .Include(pp => pp.IdPedidoNavigation)
                .Where(pp => pp.IdPedidoNavigation.Status == "A" || pp.IdPedidoNavigation.Status == "F")
                .Count();
        }
    }
}
