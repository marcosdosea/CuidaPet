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
    public class DoencaController : Controller
    {
        private readonly IDoencaService doencaService;
        private readonly IEspecieService especieService;
        private readonly IMapper mapper;

        public DoencaController(IDoencaService doencaService, IEspecieService especieService, IMapper mapper)
        {
            this.doencaService = doencaService;
            this.especieService = especieService;
            this.mapper = mapper;
        }

        // GET: DoencaController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var doencas = doencaService.GetAll(page, pageSize);
            var doencaViewModels = mapper.Map<IEnumerable<DoencaViewModel>>(doencas);

            int maxPageSize = 100;
            var especies = especieService.GetAll(1, maxPageSize);
            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = doencaService.GetCount();

            return View(doencaViewModels);
        }

        // GET: DoencaController/Details/5
        public ActionResult Details(uint id)
        {
            var doenca = doencaService.Get(id);
            var doencaViewModel = mapper.Map<DoencaViewModel>(doenca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            return View(doencaViewModel);
        }

        // GET: DoencaController/Create
        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View();
        }

        // POST: DoencaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoencaViewModel doencaViewModel)
        {
            if (ModelState.IsValid)
            {
                var doenca = mapper.Map<Doenca>(doencaViewModel);
                doencaService.Create(doenca);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(doencaViewModel);
        }

        // GET: DoencaController/Edit/5
        public ActionResult Edit(uint id)
        {
            var doenca = doencaService.Get(id);
            var doencaViewModel = mapper.Map<DoencaViewModel>(doenca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(doencaViewModel);
        }

        // POST: DoencaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoencaViewModel doencaViewModel)
        {
            if (ModelState.IsValid)
            {
                var doenca = mapper.Map<Doenca>(doencaViewModel);
                doencaService.Edit(doenca);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = new SelectList(especies, "Id", "Nome");

            return View(doencaViewModel);
        }

        // GET: DoencaController/Delete/5
        public ActionResult Delete(uint id)
        {
            var doenca = doencaService.Get(id);
            var doencaViewModel = mapper.Map<DoencaViewModel>(doenca);

            int page = 1;
            int pageSize = 100;
            var especies = especieService.GetAll(page, pageSize);

            ViewBag.Especies = especies.ToDictionary(e => e.Id, e => e.Nome);

            return View(doencaViewModel);
        }

        // POST: DoencaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, DoencaViewModel doencaViewModel)
        {
            doencaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
