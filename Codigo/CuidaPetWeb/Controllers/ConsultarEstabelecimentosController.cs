using AutoMapper;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace CuidaPetWeb.Controllers
{
    public class ConsultarEstabelecimentosController : Controller
    {
        private readonly IEstabelecimentoService estabelecimentoService;
        private readonly IProdutoService produtoService;
        private readonly IMapper mapper;

        public ConsultarEstabelecimentosController(IEstabelecimentoService estabelecimentoService, IProdutoService produtoService ,IMapper mapper)
        {
            this.estabelecimentoService = estabelecimentoService;
            this.produtoService = produtoService;
            this.mapper = mapper;
        }

        // GET: ConsultarEstabelecimentos
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize).ToList();
            var totalItems = estabelecimentoService.GetCount();

            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            var viewModel = mapper.Map<List<ConsultarEstabelecimentosViewModel>>(estabelecimentos);
            return View(viewModel);
        }

        // GET: ConsultarEstabelecimentos/Details/5
        public ActionResult Details(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);

            var detalhesViewModel = mapper.Map<DetalhesEstabelecimentoViewModel>(estabelecimento);

            var produtosDto = produtoService.GetByEstabelecimento(id);
            detalhesViewModel.Produtos = mapper.Map<List<ProdutoViewModel>>(produtosDto);

            return View(detalhesViewModel);
        }
    }
}
