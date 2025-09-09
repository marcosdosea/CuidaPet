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
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var pedidos = pedidoProdutoService.GetAll(page, pageSize);
            var pedidosViewModel = mapper.Map<IEnumerable<PedidoProdutoViewModel>>(pedidos);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = pedidos.Count();
            return View(pedidosViewModel);
        }

        // GET: PedidoProdutoController/Details/5
        public ActionResult Details(uint id)
        {
            var pedido = pedidoProdutoService.Get(id);
            var pedidoViewModel = mapper.Map<PedidoProdutoViewModel>(pedido);
            return View(pedidoViewModel);
        }

        // POST: PedidoProdutoController/Aceitar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aceitar(uint id)
        {
            pedidoProdutoService.AlterarStatus(id, "Concluída");
            return RedirectToAction(nameof(Index));
        }

        // POST: PedidoProdutoController/Recusar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recusar(uint id)
        {
            pedidoProdutoService.AlterarStatus(id, "Cancelada");
            return RedirectToAction(nameof(Index));
        }
    }
}
