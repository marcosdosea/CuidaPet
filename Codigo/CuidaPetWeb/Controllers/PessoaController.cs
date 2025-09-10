using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class PessoaController : Controller
    {
        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper;

        public PessoaController(IPessoaService pessoaService, IMapper mapper)
        {
            this.pessoaService = pessoaService;
            this.mapper = mapper;
        }

        // GET: PessoaController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var pessoas = pessoaService.GetAll(page, pageSize);
            var viewModel = mapper.Map<IEnumerable<PessoaViewModel>>(pessoas);

            ViewBag.TotalItems = pessoaService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(viewModel);
        }

        // GET: PessoaController/Details/5
        public ActionResult Details(uint id)
        {
            var pessoa = pessoaService.Get(id);

            var viewModel = mapper.Map<PessoaViewModel>(pessoa);
            return View(viewModel);
        }

        // GET: PessoaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var pessoa = mapper.Map<Pessoa>(viewModel);
                pessoaService.Create(pessoa);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: PessoaController/Edit/5
        public ActionResult Edit(uint id)
        {
            var pessoa = pessoaService.Get(id);
            var viewModel = mapper.Map<PessoaViewModel>(pessoa);
            return View(viewModel);
        }

        // POST: PessoaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PessoaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var pessoa = mapper.Map<Pessoa>(viewModel);
                pessoaService.Edit(pessoa);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: PessoaController/Delete/5
        public ActionResult Delete(uint id)
        {
            var pessoa = pessoaService.Get(id);
            var viewModel = mapper.Map<PessoaViewModel>(pessoa);
            return View(viewModel);
        }

        // POST: PessoaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, PessoaViewModel viewModel)
        {
            pessoaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
