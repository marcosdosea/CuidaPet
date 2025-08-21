using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CuidaPetWeb.Controllers
{
    public class VacinaController : Controller
    {
        private readonly IVacinaService vacinaService;
        private readonly IDoencaService doencaService;
        private readonly IEspecieService especieService;
        private readonly IMapper mapper;

        public VacinaController(IVacinaService vacinaService, IDoencaService doencaService, IEspecieService especieService, IMapper mapper)
        {
            this.vacinaService = vacinaService;
            this.doencaService = doencaService;
            this.especieService = especieService;
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
            var listaDeDoencas = doencaService.GetAll() ?? new List<Doenca>();
            var listaDeEspecies = especieService.GetAll() ?? new List<Especie>();

            ViewBag.Doencas = new SelectList(listaDeDoencas, "Id", "Nome");
            ViewBag.Especies = new SelectList(listaDeEspecies, "Id", "Nome");
            
            return View(new VacinaViewModel());
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
