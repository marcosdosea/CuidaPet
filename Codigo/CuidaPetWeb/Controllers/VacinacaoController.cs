using AutoMapper;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class VacinacaoController : Controller
    {
        private readonly IVacinacaoService vacinacaoService;
        private readonly IMapper mapper;

        public VacinacaoController(IVacinacaoService vacinacaoService, IMapper mapper)
        {
            this.vacinacaoService = vacinacaoService;
            this.mapper = mapper;
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var vacinacoes = vacinacaoService.GetAll(page, pageSize);
            var vacinacaoViewModels = mapper.Map<IEnumerable<VacinacaoViewModel>>(vacinacoes);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = vacinacaoService.GetCount();

            return View(vacinacaoViewModels);
        }

        public ActionResult Details(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        public ActionResult Create()
        {
            // Carregar listas para selects se necessário
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody] VacinacaoViewModel vacinacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacinacao = mapper.Map<Core.Vacinacao>(vacinacaoViewModel);
                vacinacaoService.Create(vacinacao);
                return RedirectToAction(nameof(Index));
            }
            return View(vacinacaoViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VacinacaoViewModel vacinacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacinacao = mapper.Map<Core.Vacinacao>(vacinacaoViewModel);
                vacinacaoService.Edit(vacinacao);
                return RedirectToAction(nameof(Index));
            }
            return View(vacinacaoViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, VacinacaoViewModel vacinacaoViewModel)
        {
            vacinacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
