using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class EspecialidadeController : Controller
    {
        private readonly IEspecialidadeService especialidadeService;
        private readonly IMapper mapper;

        public EspecialidadeController(IEspecialidadeService especialidadeService, IMapper mapper)
        {
            this.especialidadeService = especialidadeService;
            this.mapper = mapper;
        }

        // GET: EspecialidadeController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var especialidades = especialidadeService.GetAll(page, pageSize);
            var especialidadeViewModels = mapper.Map<IEnumerable<EspecialidadeViewModel>>(especialidades);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = especialidadeService.GetCount();

            return View(especialidadeViewModels);
        }

        // GET: EspecialidadeController/Details/5
        public ActionResult Details(uint id)
        {
            var especialidade = especialidadeService.Get(id);
            var especialidadeViewModel = mapper.Map<EspecialidadeViewModel>(especialidade);
            return View(especialidadeViewModel);
        }

        // GET: EspecialidadeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EspecialidadeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EspecialidadeViewModel especialidadeViewModel)
        {
            if (ModelState.IsValid)
            {
                var especialidade = mapper.Map<Especialidade>(especialidadeViewModel);
                especialidadeService.Create(especialidade);
                return RedirectToAction(nameof(Index));
            }
            return View(especialidadeViewModel);
        }

        // GET: EspecialidadeController/Edit/5
        public ActionResult Edit(uint id)
        {
            var especialidade = especialidadeService.Get(id);
            var especialidadeViewModel = mapper.Map<EspecialidadeViewModel>(especialidade);
            return View(especialidadeViewModel);
        }

        // POST: EspecialidadeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EspecialidadeViewModel especialidadeViewModel)
        {
            if (ModelState.IsValid)
            {
                var especialidade = mapper.Map<Especialidade>(especialidadeViewModel);
                especialidadeService.Edit(especialidade);
                return RedirectToAction(nameof(Index));
            }
            return View(especialidadeViewModel);
        }

        // GET: EspecialidadeController/Delete/5
        public ActionResult Delete(uint id)
        {
            var especialidade = especialidadeService.Get(id);
            var especialidadeViewModel = mapper.Map<EspecialidadeViewModel>(especialidade);
            return View(especialidadeViewModel);
        }

        // POST: EspecialidadeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, EspecialidadeViewModel especialidadeViewModel)
        {
            especialidadeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
