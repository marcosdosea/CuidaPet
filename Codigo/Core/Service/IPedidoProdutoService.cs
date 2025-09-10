using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;

namespace Core.Service
{
    public interface IPedidoProdutoService
    {
        // Criar um novo pedido de produto
        uint Create(Pedidoproduto pedidoProduto);

        // Editar um pedido de produto existente
        void Edit(Pedidoproduto pedidoProduto);

        // Deletar um pedido de produto
        void Delete(uint id);

        // Buscar um pedido de produto pelo Id
        Pedidoproduto? Get(uint id);

        // Buscar todos os pedidos de produto (paginado)
        IEnumerable<Pedidoproduto> GetAll(int page, int pageSize);

        // Buscar pedidos de produto por status
        IEnumerable<PedidoProdutoDto> GetByStatus(string status);

        // Buscar pedidos de produto por tutor
        IEnumerable<PedidoProdutoDto> GetByTutor(uint tutorId);

        // Alterar status do pedido (aceitar, recusar, concluir, cancelar)
        void AlterarStatus(uint id, string novoStatus);

        // Buscar detalhes do pedido para exibição
        PedidoProdutoDto? GetDetalhes(uint id);

        // Buscar todos os itens de um pedido específico
        IEnumerable<PedidoProdutoDto> GetItensByPedidoId(uint pedidoId);

        // Recusar pedido: deletar itens e desativar pedido
        void RecusarPedido(uint pedidoProdutoId);

        // Buscar pedidos com filtros e ordenação
        IEnumerable<PedidoProdutoDto> GetPedidosAtivos(int page, int pageSize, string? sortBy = null, bool descending = false);

        // Contar total de pedidos ativos
        int GetCountPedidosAtivos();
    }
}
