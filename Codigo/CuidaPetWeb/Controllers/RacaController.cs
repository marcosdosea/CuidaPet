using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class RacaController : Controller
    {
        private readonly IRacaService racaService;
        private readonly IMapper mapper;

        public RacaController(IRacaService racaService, IMapper mapper)
        {
            this.racaService = racaService;
            this.mapper = mapper;
        }

        // GET: Raca
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var racas = racaService.GetAll(page, pageSize);
            var racaViewModels = mapper.Map<IEnumerable<RacaViewModel>>(racas);

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
            return View(racaViewModel);
        }

        // GET: Raca/Create
        public ActionResult Create()
        {
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
            return View(racaViewModel);
        }

        // GET: Raca/Edit/5
        public ActionResult Edit(uint id)
        {
            var raca = racaService.Get(id);
            var racaViewModel = mapper.Map<RacaViewModel>(raca);
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
            return View(racaViewModel);
        }

        // GET: Raca/Delete/5
        public ActionResult Delete(uint id)
        {
            var raca = racaService.Get(id);
            var racaViewModel = mapper.Map<RacaViewModel>(raca);
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
