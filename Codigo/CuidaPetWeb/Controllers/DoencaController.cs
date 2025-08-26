using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class DoencaController : Controller
    {
        private readonly IDoencaService doencaService;
        private readonly IMapper mapper;

        public DoencaController(IDoencaService doencaService, IMapper mapper)
        {
            this.doencaService = doencaService;
            this.mapper = mapper;
        }

        // GET: DoencaController
        public ActionResult Index(int page, int pageSize)
        {
            var doencas = doencaService.GetAll(page, pageSize);
            var doencaViewModels = mapper.Map<IEnumerable<DoencaViewModel>>(doencas);
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
            return View(doencaViewModel);
        }

        // GET: DoencaController/Create
        public ActionResult Create()
        {
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
            return View(doencaViewModel);
        }

        // GET: DoencaController/Edit/5
        public ActionResult Edit(uint id)
        {
            var doenca = doencaService.Get(id);
            var doencaViewModel = mapper.Map<DoencaViewModel>(doenca);
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
            return View(doencaViewModel);
        }

        // GET: DoencaController/Delete/5
        public ActionResult Delete(uint id)
        {
            var doenca = doencaService.Get(id);
            var doencaViewModel = mapper.Map<DoencaViewModel>(doenca);
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
