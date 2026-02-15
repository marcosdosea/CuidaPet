using AutoMapper;
using Core;
using Core.Context;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly IProdutoService produtoService;
        private readonly IEstabelecimentoService estabelecimentoService;
        private readonly CuidaPetContext context;
        private readonly IMapper mapper;

        public ProdutoController(IProdutoService produtoService, IEstabelecimentoService estabelecimentoService, CuidaPetContext context, IMapper mapper)
        {
            this.produtoService = produtoService;
            this.estabelecimentoService = estabelecimentoService;
            this.context = context;
            this.mapper = mapper;
        }

        // GET: Produto
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var produtos = produtoService.GetAll(page, pageSize);
            var produtoViewModels = mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);

            int maxPageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(1, maxPageSize);

            ViewBag.Categorias = categorias.ToDictionary(c => c.Id, c => c.Nome);
            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);
            ViewBag.TotalItems = produtoService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(produtoViewModels);
        }

        // GET: Produto/Details/5
        public ActionResult Details(uint id)
        {
            var produto = produtoService.Get(id);

            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);

            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = categorias.ToDictionary(c => c.Id, c => c.Nome);
            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);

            return View(produtoViewModel);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                var produto = mapper.Map<Produto>(produtoViewModel);
                produtoService.Create(produto);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View(produtoViewModel);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(uint id)
        {
            var produto = produtoService.Get(id);
            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);

            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View(produtoViewModel);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                var produto = mapper.Map<Produto>(produtoViewModel);
                produtoService.Edit(produto);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View(produtoViewModel);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(uint id)
        {
            var produto = produtoService.Get(id);
            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);

            int page = 1;
            int pageSize = 100;
            var categorias = context.Categoria.OrderBy(c => c.Nome).ToList();
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Categorias = categorias.ToDictionary(c => c.Id, c => c.Nome);
            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);

            return View(produtoViewModel);
        }

        // POST: Produto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ProdutoViewModel produtoViewModel)
        {
            produtoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
