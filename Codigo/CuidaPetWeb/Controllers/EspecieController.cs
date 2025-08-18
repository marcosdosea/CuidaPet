using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class EspecieController : Controller
    {
        private readonly IEspecieService especieService;
        private readonly IMapper mapper;

        public EspecieController(IEspecieService especieService, IMapper mapper)
        {
            this.especieService = especieService;
            this.mapper = mapper;
        }

        // GET: EspecieController
        public IActionResult Index()
        {
            var especies = especieService.GetAll();
            var especiesViewModel = mapper.Map<IEnumerable<EspecieViewModel>>(especies);
            return View(especiesViewModel);
        }

        // GET: EspecieController/Details/5
        public IActionResult Details(uint id)
        {
            var especie = especieService.Get(id);

            var especieViewModel = mapper.Map<EspecieViewModel>(especie);
            return View(especieViewModel);
        }

        // GET: EspecieController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EspecieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EspecieViewModel especieViewModel)
        {
            if (ModelState.IsValid)
            {
                var especie = mapper.Map<Especie>(especieViewModel);
                especieService.Create(especie);
                return RedirectToAction(nameof(Index));
            }
            return View(especieViewModel);
        }

        // GET: EspecieController/Edit/5
        public IActionResult Edit(uint id)
        {
            var especie = especieService.Get(id);

            var especieViewModel = mapper.Map<EspecieViewModel>(especie);
            return View(especieViewModel);
        }

        // POST: EspecieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EspecieViewModel especieViewModel)
        {
            if (ModelState.IsValid)
            {
                var especie = mapper.Map<Especie>(especieViewModel);
                especieService.Edit(especie);
                return RedirectToAction(nameof(Index));
            }
            return View(especieViewModel);
        }

        // GET: EspecieController/Delete/5
        public IActionResult Delete(uint id)
        {
            var especie = especieService.Get(id);

            var especieViewModel = mapper.Map<EspecieViewModel>(especie);
            return View(especieViewModel);
        }

        // POST: EspecieController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(uint id)
        {
            especieService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
