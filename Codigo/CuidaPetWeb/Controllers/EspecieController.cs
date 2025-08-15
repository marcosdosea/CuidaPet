using AutoMapper;
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
            var viewModel = mapper.Map<IEnumerable<EspecieViewModel>>(especies);
            return View(viewModel);
        }

        // GET: EspecieController/Details/5
        public IActionResult Details(uint id)
        {
            var especie = especieService.Get(id);
            
            var viewModel = mapper.Map<EspecieViewModel>(especie);
            return View(viewModel);
        }

        // GET: EspecieController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EspecieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EspecieViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = mapper.Map<EspecieDto>(viewModel);
                especieService.Create(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: EspecieController/Edit/5
        public IActionResult Edit(uint id)
        {
            var especie = especieService.Get(id);
            
            var viewModel = mapper.Map<EspecieViewModel>(especie);
            return View(viewModel);
        }

        // POST: EspecieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(uint id, EspecieViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var dto = mapper.Map<EspecieDto>(viewModel);
                especieService.Edit(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: EspecieController/Delete/5
        public IActionResult Delete(uint id)
        {
            var especie = especieService.Get(id);
            
            var viewModel = mapper.Map<EspecieViewModel>(especie);
            return View(viewModel);
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
