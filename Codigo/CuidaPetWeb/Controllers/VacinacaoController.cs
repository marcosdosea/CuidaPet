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
    public class VacinacaoController : Controller
    {
        private readonly IVacinacaoService vacinacaoService;
        private readonly IPetService petService;
        private readonly IVacinaService vacinaService;
        private readonly IFuncionarioService funcionarioService;
        private readonly IPessoaService tutorService;
        private readonly IMapper mapper;

        public VacinacaoController(IVacinacaoService vacinacaoService, IPetService petService, IVacinaService vacinaService, IFuncionarioService funcionarioService, IPessoaService tutorService, IMapper mapper)
        {
            this.vacinacaoService = vacinacaoService;
            this.petService = petService;
            this.vacinaService = vacinaService;
            this.funcionarioService = funcionarioService;
            this.tutorService = tutorService;
            this.mapper = mapper;
        }

        private void PopularViewBags()
        {
            int page = 1;
            int pageSize = 20;
            
            var listaDePets = petService.GetAll(page, pageSize) ?? new List<Pet>();
            var listaDeVacinas = vacinaService.GetAll(page, pageSize) ?? new List<Vacina>();
            var listaDeFuncionarios = funcionarioService.GetAll(page, pageSize) ?? new List<Funcionario>();
            var listaDeTutores = tutorService.GetTutores() ?? new List<Pessoa>();
            
            ViewBag.Pets = new SelectList(listaDePets, "Id", "Nome");
            ViewBag.Vacinas = new SelectList(listaDeVacinas, "Id", "Nome");
            ViewBag.Funcionarios = new SelectList(listaDeFuncionarios, "Id", "Nome");
            ViewBag.Tutores = new SelectList(listaDeTutores, "Id", "Nome");
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var vacinacoes = vacinacaoService.GetAll(page, pageSize);
            var vacinacaoViewModels = mapper.Map<IEnumerable<VacinacaoViewModel>>(vacinacoes);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = vacinacaoService.GetCount();

            return View(vacinacaoViewModels);
        }

        public ActionResult Details(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            PopularViewBags();

            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        public ActionResult Create()
        {
            PopularViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VacinacaoViewModel vacinacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacinacao = mapper.Map<Core.Vacinacao>(vacinacaoViewModel);
                vacinacaoService.Create(vacinacao);
                return RedirectToAction(nameof(Index));
            }
            
            PopularViewBags();
            return View(vacinacaoViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            PopularViewBags();
            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VacinacaoViewModel vacinacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var vacinacao = mapper.Map<Core.Vacinacao>(vacinacaoViewModel);
                vacinacaoService.Edit(vacinacao);
                return RedirectToAction(nameof(Index));
            }
            
            PopularViewBags();
            return View(vacinacaoViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var vacinacao = vacinacaoService.Get(id);
            if (vacinacao == null)
                return NotFound();

            var vacinacaoViewModel = mapper.Map<VacinacaoViewModel>(vacinacao);
            return View(vacinacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, VacinacaoViewModel vacinacaoViewModel)
        {
            vacinacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
