using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService funcionarioService;
        private readonly IPessoaService pessoaService;
        private readonly IEstabelecimentoService estabelecimentoService;
        private readonly IMapper mapper;
        private readonly UserManager<UsuarioIdentity> userManager;
        private readonly ILogger<FuncionarioController> logger;

        public FuncionarioController(IFuncionarioService funcionarioService, IPessoaService pessoaService, IEstabelecimentoService estabelecimentoService, IMapper mapper, UserManager<UsuarioIdentity> userManager, ILogger<FuncionarioController> logger)
        {
            this.funcionarioService = funcionarioService;
            this.pessoaService = pessoaService;
            this.estabelecimentoService = estabelecimentoService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
        }


        // GET: FuncionarioController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var funcionarios = funcionarioService.GetAll(page, pageSize);
            var funcionarioViewModels = mapper.Map<IEnumerable<FuncionarioViewModel>>(funcionarios);

            funcionarioViewModels = funcionarioViewModels
                .Where(f => f.Tipo == "V" || f.Tipo == "A")
                .ToList();

            int maxPageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(1, maxPageSize);
            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);

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

            int page = 1;
            int pageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);

            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Create
        public ActionResult Create()
        {
            int page = 1;
            int pageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View();
        }

        // POST: FuncionarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FuncionarioViewModel funcionarioViewModel)
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
                    // Verificar se o email já está cadastrado
                    var usuarioExistente = await userManager.FindByEmailAsync(funcionarioViewModel.Email);
                    if (usuarioExistente != null)
                    {
                        ModelState.AddModelError("Email", "Email já cadastrado.");
                        return View(funcionarioViewModel);
                    }

                    // Criar o usuário Identity
                    var user = new UsuarioIdentity
                    {
                        UserName = funcionarioViewModel.Nome,
                        Email = funcionarioViewModel.Email,
                        PhoneNumber = funcionarioViewModel.Telefone,
                        NormalizedUserName = funcionarioViewModel.Nome.ToUpper(),
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, funcionarioViewModel.Senha);

                    if (result.Succeeded)
                    {
                        logger.LogInformation("Usuário funcionário criado com sucesso.");

                        // Atribuir role baseado no tipo de funcionário
                        string role = funcionarioViewModel.Tipo switch
                        {
                            "V" => "Veterinário",
                            "A" => "Atendente",
                            _ => "Funcionario"
                        };
                        await userManager.AddToRoleAsync(user, role);

                        // Criar a Pessoa associada ao usuário
                        var pessoa = mapper.Map<Pessoa>(funcionarioViewModel);
                        pessoa.IdUsuario = user.Id;
                        var idPessoa = pessoaService.Create(pessoa);

                        // Criar o Funcionário
                        var funcionario = mapper.Map<Funcionario>(funcionarioViewModel);
                        funcionario.IdPessoa = idPessoa;
                        funcionarioService.Create(funcionario);

                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            int page = 1;
            int pageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Edit/5
        public ActionResult Edit(uint id)
        {
                var funcionario = funcionarioService.Get(id);
                var funcionarioViewModel = mapper.Map<FuncionarioViewModel>(funcionario);

                int page = 1;
                int pageSize = 100;
                var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

                ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

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

            int page = 1;
            int pageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Estabelecimentos = new SelectList(estabelecimentos, "Id", "Nome");

            return View(funcionarioViewModel);
        }

        // GET: FuncionarioController/Delete/5
        public ActionResult Delete(uint id)
        {
            var funcionario = funcionarioService.Get(id);
            var funcionarioViewModel = mapper.Map<FuncionarioViewModel>(funcionario);

            int page = 1;
            int pageSize = 100;
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            ViewBag.Estabelecimentos = estabelecimentos.ToDictionary(e => e.Id, e => e.Nome);

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
