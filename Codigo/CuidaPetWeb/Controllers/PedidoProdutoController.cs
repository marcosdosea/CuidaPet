using AutoMapper;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class PedidoProdutoController : Controller
    {
        private readonly IPedidoProdutoService pedidoProdutoService;
        private readonly IMapper mapper;

        public PedidoProdutoController(IPedidoProdutoService pedidoProdutoService, IMapper mapper)
        {
            this.pedidoProdutoService = pedidoProdutoService;
            this.mapper = mapper;
        }

        // GET: PedidoProdutoController
        public ActionResult Index(int page = 1, int pageSize = 10, string? sortBy = null, bool descending = false)
        {
            // Usar o método que já retorna DTOs ordenados
            var pedidosDto = pedidoProdutoService.GetPedidosAtivos(page, pageSize, sortBy, descending);

            // Mapeamento manual
            var pedidosViewModel = pedidosDto.Select(dto => new PedidoProdutoViewModel
            {
                Id = dto.Id,
                RealizadoEm = dto.RealizadoEm,
                Status = dto.Status,
                ProdutoNome = dto.ProdutoNome,
                Quantidade = dto.Quantidade,
                PrecoTotal = dto.PrecoTotal,
                TutorNome = dto.TutorNome,
                TutorTelefone = dto.TutorTelefone
            }).ToList();

            // Contar total para paginação
            var totalItems = pedidoProdutoService.GetCountPedidosAtivos();

            ViewBag.TotalItems = totalItems;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SortBy = sortBy;
            ViewBag.Descending = descending;
            
            return View(pedidosViewModel);
        }

        // GET: PedidoProdutoController/Details/5
        public ActionResult Details(uint id)
        {
            var pedidoDto = pedidoProdutoService.GetDetalhes(id);
            if (pedidoDto == null)
            {
                return NotFound();
            }
            
            // Mapeamento manual
            var pedidoViewModel = new PedidoProdutoViewModel
            {
                Id = pedidoDto.Id,
                RealizadoEm = pedidoDto.RealizadoEm,
                Status = pedidoDto.Status,
                ProdutoNome = pedidoDto.ProdutoNome,
                Quantidade = pedidoDto.Quantidade,
                PrecoTotal = pedidoDto.PrecoTotal,
                TutorNome = pedidoDto.TutorNome,
                TutorTelefone = pedidoDto.TutorTelefone
            };
            
            return View(pedidoViewModel);
        }

        // GET: Buscar itens de um pedido via AJAX
        [HttpGet]
        public JsonResult GetItensPedido(uint pedidoId)
        {
            var itens = pedidoProdutoService.GetItensByPedidoId(pedidoId);
            var itensViewModel = itens.Select(item => new
            {
                id = item.Id,
                produtoNome = item.ProdutoNome,
                quantidade = item.Quantidade,
                precoUnitario = item.PrecoUnitario.ToString("F2"),
                precoTotal = item.PrecoTotal.ToString("F2")
            }).ToList();

            return Json(itensViewModel);
        }

        // POST: PedidoProdutoController/Aceitar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aceitar(uint id)
        {
            pedidoProdutoService.AlterarStatus(id, "F"); // F = Finalizado
            return RedirectToAction(nameof(Index));
        }

        // POST: PedidoProdutoController/Recusar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recusar(uint id)
        {
            // Deletar os itens do pedido e desativar o pedido
            pedidoProdutoService.RecusarPedido(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
