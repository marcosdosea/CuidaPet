using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class PetController : Controller
    {
        private readonly IPetService petService;
        private readonly IMapper mapper;

        public PetController(IPetService petService, IMapper mapper)
        {
            this.petService = petService;
            this.mapper = mapper;
        }

        // GET: PetController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var pets = petService.GetAll(page, pageSize);
            var petsViewModel = mapper.Map<IEnumerable<PetViewModel>>(pets);

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
            return View(petViewModel);
        }

        // GET: PetController/Create
        public ActionResult Create()
        {
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
            return View(petViewModel);
        }

        // GET: PetController/Edit/5
        public ActionResult Edit(uint id)
        {
            var pet = petService.Get(id);

            var petViewModel = mapper.Map<PetViewModel>(pet);
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
            return View(petViewModel);
        }

        // GET: PetController/Delete/5
        public ActionResult Delete(uint id)
        {
            var pet = petService.Get(id);

            var petViewModel = mapper.Map<PetViewModel>(pet);
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
