using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class VacinaController : Controller
    {
        private readonly IVacinaService vacinaService;
        private readonly IMapper mapper;

        public VacinaController(IVacinaService vacinaService, IMapper mapper)
        {
            this.vacinaService = vacinaService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var vacinas = vacinaService.GetAll();
            var vacinaViewModels = mapper.Map<IEnumerable<VacinaViewModel>>(vacinas);
            return View(vacinaViewModels);
        }

        public ActionResult Details(uint id)
        {
            var vacina = vacinaService.Get(id);

            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);
            return View(vacinaViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VacinaViewModel vacinaViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacina = mapper.Map<Vacina>(vacinaViewModel);
                vacinaService.Create(vacina);
                return RedirectToAction(nameof(Index));
            }
            return View(vacinaViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var vacina = vacinaService.Get(id);
            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);
            return View(vacinaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VacinaViewModel vacinaViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacina = mapper.Map<Vacina>(vacinaViewModel);
                vacinaService.Edit(vacina);
                return RedirectToAction(nameof(Index));
            }
            return View(vacinaViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var vacina = vacinaService.Get(id);
            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);
            return View(vacinaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, VacinaViewModel vacinaViewModel)
        {
            vacinaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
