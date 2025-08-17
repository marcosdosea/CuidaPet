using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoService produtoService;
        private readonly IMapper mapper;

        public ProdutoController(IProdutoService produtoService, IMapper mapper)
        {
            this.produtoService = produtoService;
            this.mapper = mapper;
        }

        // GET: Produto
        public ActionResult Index()
        {
            var produtos = produtoService.GetAll();
            var produtoViewModels = mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            return View(produtoViewModels);
        }

        // GET: Produto/Details/5
        public ActionResult Details(uint id)
        {
            var produto = produtoService.Get(id);

            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);
            return View(produtoViewModel);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
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
            return View(produtoViewModel);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(uint id)
        {
            var produto = produtoService.Get(id);
            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);
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
            return View(produtoViewModel);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(uint id)
        {
            var produto = produtoService.Get(id);
            var produtoViewModel = mapper.Map<ProdutoViewModel>(produto);
            return View(produtoViewModel);
        }

        // POST: Produto/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ProdutoViewModel produtoViewModel)
        {
            produtoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
