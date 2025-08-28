using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService funcionarioService;
        private readonly IMapper mapper;

        public FuncionarioController(IFuncionarioService funcionarioService, IMapper mapper)
        {
            this.funcionarioService = funcionarioService;
            this.mapper = mapper;
        }


        // GET: FuncionarioController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var funcionarios = funcionarioService.GetAll(page, pageSize);
            var funcionarioViewModel = mapper.Map<IEnumerable<FuncionarioViewModel>>(funcionarios);

            ViewBag.TotalItems = funcionarioService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Details/5
        public ActionResult Details(uint id)
        {
            var funcionario = funcionarioService.Get(id);

            var funcionarioViewModel = mapper.Map<FuncionarioViewModel>(funcionario);
            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuncionarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FuncionarioViewModel funcionarioViewModel)
        {
            if (ModelState.IsValid)
            {
                var funcionario = mapper.Map<Funcionario>(funcionarioViewModel);
                funcionarioService.Create(funcionario);
                return RedirectToAction(nameof(Index));
            }
            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Edit/5
        public ActionResult Edit(uint id)
        {
            var funcionario = funcionarioService.Get(id);
            var funcionarioViewModel = mapper.Map<FuncionarioViewModel>(funcionario);
            return View(funcionarioViewModel);
        }

        // POST: FuncionarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuncionarioViewModel funcionarioViewModel)
        {
            if (ModelState.IsValid)
            {
                var funcionario = mapper.Map<Funcionario>(funcionarioViewModel);
                funcionarioService.Edit(funcionario);
                return RedirectToAction(nameof(Index));
            }
            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Delete/5
        public ActionResult Delete(uint id)
        {
            var funcionario = funcionarioService.Get(id);
            var funcionarioViewModel = mapper.Map<FuncionarioViewModel>(funcionario);
            return View(funcionarioViewModel);
        }

        // POST: FuncionarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, FuncionarioViewModel funcionarioViewModel)
        {
            funcionarioService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
