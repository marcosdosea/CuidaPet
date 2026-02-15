using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService funcionarioService;
        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper;

        public FuncionarioController(IFuncionarioService funcionarioService, IPessoaService pessoaService, IMapper mapper)
        {
            this.funcionarioService = funcionarioService;
            this.pessoaService = pessoaService;
            this.mapper = mapper;
        }


        // GET: FuncionarioController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var funcionarios = funcionarioService.GetAll(page, pageSize);
            var funcionarioViewModels = mapper.Map<IEnumerable<FuncionarioViewModel>>(funcionarios);

            funcionarioViewModels = funcionarioViewModels
                .Where(f => f.Tipo == "V" || f.Tipo == "A")
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = funcionarioService.GetCount();

            return View(funcionarioViewModels);
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
            if (funcionarioViewModel.Tipo == "V" && string.IsNullOrWhiteSpace(funcionarioViewModel.Crmv))
            {
                ModelState.AddModelError("Crmv", "O campo CRMV é obrigatório para veterinários.");
            }

            if (ModelState.IsValid)
            {
                var pessoaExistente = pessoaService.GetByCpf(funcionarioViewModel.Cpf);

                if (pessoaExistente != null)
                {
                    funcionarioViewModel.IdPessoa = pessoaExistente.Id;
                    var funcionario = mapper.Map<Funcionario>(funcionarioViewModel);
                    funcionarioService.Create(funcionario);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var pessoa = mapper.Map<Pessoa>(funcionarioViewModel);
                    var idPessoa = pessoaService.Create(pessoa);

                    var funcionario = mapper.Map<Funcionario>(funcionarioViewModel);
                    funcionario.IdPessoa = idPessoa;
                    funcionarioService.Create(funcionario);

                    return RedirectToAction(nameof(Index));
                }
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

        [HttpGet]
        public JsonResult GetPessoaByCpf(string cpf)
        {
            var pessoa = pessoaService.GetByCpf(cpf);
            if (pessoa == null)
                return Json(null);

            // Carregar os dados do usuário associado
            var usuario = pessoa.IdUsuarioNavigation;

            return Json(new
            {
                id = pessoa.Id,
                nome = usuario?.UserName ?? "",
                email = usuario?.Email ?? "",
                telefone = usuario?.PhoneNumber ?? "",
                logradouro = pessoa.Logradouro,
                numero = pessoa.Numero,
                complemento = pessoa.Complemento,
                bairro = pessoa.Bairro,
                cidade = pessoa.Cidade,
                estado = pessoa.Estado
            });
        }
    }
}
