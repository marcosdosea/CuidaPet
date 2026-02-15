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
    public class RacaController : Controller
    {
        private readonly IRacaService racaService;
        private readonly IEspecieService especieService;
        private readonly IMapper mapper;

        public RacaController(IRacaService racaService, IEspecieService especieService, IMapper mapper)
        {
            this.racaService = racaService;
            this.especieService = especieService;
            this.mapper = mapper;
        }

        // GET: Raca
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var racas = racaService.GetAll(page, pageSize);
            var racaViewModels = mapper.Map<IEnumerable<RacaViewModel>>(racas);

            int maxPageSize = 100;
            var especies = especieService.GetAll(1, maxPageSize);
            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = racaService.GetCount();

            return View(racaViewModels);
        }

        // GET: Raca/Details/5
        public ActionResult Details(uint id)
        {
            var raca = racaService.Get(id);
            var racaViewModel = mapper.Map<RacaViewModel>(raca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            return View(racaViewModel);
        }

        // GET: Raca/Create
        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View();
        }

        // POST: Raca/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RacaViewModel racaViewModel)
        {
            if (ModelState.IsValid)
            {
                var raca = mapper.Map<Raca>(racaViewModel);
                racaService.Create(raca);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(racaViewModel);
        }

        // GET: Raca/Edit/5
        public ActionResult Edit(uint id)
        {
            var raca = racaService.Get(id);
            var racaViewModel = mapper.Map<RacaViewModel>(raca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(racaViewModel);
        }

        // POST: Raca/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RacaViewModel racaViewModel)
        {
            if (ModelState.IsValid)
            {
                var raca = mapper.Map<Raca>(racaViewModel);
                racaService.Edit(raca);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(racaViewModel);
        }

        // GET: Raca/Delete/5
        public ActionResult Delete(uint id)
        {
            var raca = racaService.Get(id);
            var racaViewModel = mapper.Map<RacaViewModel>(raca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            return View(racaViewModel);
        }

        // POST: Raca/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, RacaViewModel racaViewModel)
        {
            racaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
