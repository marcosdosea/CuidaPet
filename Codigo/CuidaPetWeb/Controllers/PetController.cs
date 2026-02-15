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
    public class PetController : Controller
    {
        private readonly IPetService petService;
        private readonly IRacaService racaService;
        private readonly IMapper mapper;

        public PetController(IPetService petService, IRacaService racaService, IMapper mapper)
        {
            this.petService = petService;
            this.racaService = racaService;
            this.mapper = mapper;
        }

        // GET: PetController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var pets = petService.GetAll(page, pageSize);
            var petsViewModel = mapper.Map<IEnumerable<PetViewModel>>(pets);

            int maxPageSize = 100;
            var racas = racaService.GetAll(1, maxPageSize);
            ViewBag.Racas = racas.ToDictionary(r => r.Id, r => r.Nome);

            ViewBag.TotalItems = petService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(petsViewModel);
        }

        // GET: PetController/Details/5
        public ActionResult Details(uint id)
        {
            var pet = petService.Get(id);

            var petViewModel = mapper.Map<PetViewModel>(pet);

            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = racas.ToDictionary(r => r.Id, r => r.Nome);

            return View(petViewModel);
        }

        // GET: PetController/Create
        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = new SelectList(racas, "Id", "Nome");

            return View();
        }

        // POST: PetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetViewModel petViewModel)
        {
            if (ModelState.IsValid)
            {
                var pet = mapper.Map<Pet>(petViewModel);
                petService.Create(pet);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = new SelectList(racas, "Id", "Nome");

            return View(petViewModel);
        }

        // GET: PetController/Edit/5
        public ActionResult Edit(uint id)
        {
            var pet = petService.Get(id);

            var petViewModel = mapper.Map<PetViewModel>(pet);

            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = new SelectList(racas, "Id", "Nome");

            return View(petViewModel);
        }

        // POST: PetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetViewModel petViewModel)
        {
            if (ModelState.IsValid)
            {
                var pet = mapper.Map<Pet>(petViewModel);
                petService.Edit(pet);
                return RedirectToAction(nameof(Index));
            }

            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = new SelectList(racas, "Id", "Nome");

            return View(petViewModel);
        }

        // GET: PetController/Delete/5
        public ActionResult Delete(uint id)
        {
            var pet = petService.Get(id);

            var petViewModel = mapper.Map<PetViewModel>(pet);

            int page = 1;
            int pageSize = 100;
            var racas = racaService.GetAll(page, pageSize);

            ViewBag.Racas = racas.ToDictionary(r => r.Id, r => r.Nome);

            return View(petViewModel);
        }

        // POST: PetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, PetViewModel petViewModel)
        {
            petService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
