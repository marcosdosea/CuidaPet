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

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var vacinas = vacinaService.GetAll(page, pageSize);
            var vacinaViewModels = mapper.Map<IEnumerable<VacinaViewModel>>(vacinas);

            int maxPageSize = 100;
            var doencas = doencaService.GetAll(1, maxPageSize);
            var especies = especieService.GetAll(1, maxPageSize);

            ViewBag.Doencas = doencas.ToDictionary(d => d.Id, d => d.Nome);
            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = vacinaService.GetCount();

            return View(vacinaViewModels);
        }

        public ActionResult Details(uint id)
        {
            var vacina = vacinaService.Get(id);

            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);

            int page = 1;
            int pageSize = 100;
            var doencas = doencaService.GetAll(page, pageSize);
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Doencas = doencas.ToDictionary(d => d.Id, d => d.Nome);
            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            return View(vacinaViewModel);
        }

        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 20;
            var listaDeDoencas = doencaService.GetAll(page, pageSize) ?? new List<Doenca>();
            var listaDeEspecies = especieService.GetAll(page, pageSize) ?? new List<Especie>();

            ViewBag.Doencas = new SelectList(listaDeDoencas, "Id", "Nome");
            ViewBag.Especies = new SelectList(listaDeEspecies, "Id", "Nome");

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

            int page = 1;
            int pageSize = 20;
            var listaDeDoencas = doencaService.GetAll(page, pageSize) ?? new List<Doenca>();
            var listaDeEspecies = especieService.GetAll(page, pageSize) ?? new List<Especie>();

            ViewBag.Doencas = new SelectList(listaDeDoencas, "Id", "Nome");
            ViewBag.Especies = new SelectList(listaDeEspecies, "Id", "Nome");

            return View(vacinaViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var vacina = vacinaService.Get(id);
            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);

            int page = 1;
            int pageSize = 20;
            var listaDeDoencas = doencaService.GetAll(page, pageSize) ?? new List<Doenca>();
            var listaDeEspecies = especieService.GetAll(page, pageSize) ?? new List<Especie>();

            ViewBag.Doencas = new SelectList(listaDeDoencas, "Id", "Nome");
            ViewBag.Especies = new SelectList(listaDeEspecies, "Id", "Nome");

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

            int page = 1;
            int pageSize = 20;
            var listaDeDoencas = doencaService.GetAll(page, pageSize) ?? new List<Doenca>();
            var listaDeEspecies = especieService.GetAll(page, pageSize) ?? new List<Especie>();

            ViewBag.Doencas = new SelectList(listaDeDoencas, "Id", "Nome");
            ViewBag.Especies = new SelectList(listaDeEspecies, "Id", "Nome");

            return View(vacinaViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var vacina = vacinaService.Get(id);
            var vacinaViewModel = mapper.Map<VacinaViewModel>(vacina);

            int page = 1;
            int pageSize = 100;
            var doencas = doencaService.GetAll(page, pageSize);
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Doencas = doencas.ToDictionary(d => d.Id, d => d.Nome);
            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

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
