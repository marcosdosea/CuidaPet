using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class EstabelecimentoController : Controller
    {
        private readonly IEstabelecimentoService estabelecimentoService;
        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper;

        public EstabelecimentoController(IEstabelecimentoService estabelecimentoService, IPessoaService pessoaService, IMapper mapper)
        {
            this.estabelecimentoService = estabelecimentoService;
            this.pessoaService = pessoaService;
            this.mapper = mapper;
        }

        private void PopularViewBags()
        {
            var gerentes = pessoaService.GetGerentes();
            ViewBag.Gerentes = new SelectList(gerentes, "Id", "Nome");
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);
            var estabelecimentoViewModels = mapper.Map<IEnumerable<EstabelecimentoViewModel>>(estabelecimentos);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = estabelecimentoService.GetCount();

            return View(estabelecimentoViewModels);
        }

        public ActionResult Details(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);

            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            return View(estabelecimentoViewModel);
        }

        public ActionResult Create()
        {
            PopularViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstabelecimentoViewModel estabelecimentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var estabelecimento = mapper.Map<Estabelecimento>(estabelecimentoViewModel);
                estabelecimentoService.Create(estabelecimento);
                return RedirectToAction(nameof(Index));
            }

            PopularViewBags();
            return View(estabelecimentoViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);
            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            PopularViewBags();
            return View(estabelecimentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EstabelecimentoViewModel estabelecimentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var estabelecimento = mapper.Map<Estabelecimento>(estabelecimentoViewModel);
                estabelecimentoService.Edit(estabelecimento);
                return RedirectToAction(nameof(Index));
            }
            PopularViewBags();
            return View(estabelecimentoViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);
            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            return View(estabelecimentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, EstabelecimentoViewModel estabelecimentoViewModel)
        {
            estabelecimentoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
