using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class EstabelecimentoController : Controller
    {
        private readonly IEstabelecimentoService estabelecimentoService;
        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper;

        public EstabelecimentoController(IEstabelecimentoService estabelecimentoService, IPessoaService pessoaService, IMapper mapper)
        {
            this.estabelecimentoService = estabelecimentoService;
            this.pessoaService = pessoaService;
            this.mapper = mapper;
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var estabelecimentos = estabelecimentoService.GetAll(page, pageSize);
            var estabelecimentoViewModels = mapper.Map<IEnumerable<EstabelecimentoViewModel>>(estabelecimentos);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = estabelecimentoService.GetCount();

            return View(estabelecimentoViewModels);
        }

        public ActionResult Details(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);

            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            return View(estabelecimentoViewModel);
        }

        public ActionResult Create()
        {
            var gerentes = pessoaService.GetGerentes();
            ViewBag.Gerentes = gerentes.Select(g => new { g.Id, Nome = g.IdUsuarioNavigation.UserName });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstabelecimentoViewModel estabelecimentoViewModel)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Criando estabelecimento: {estabelecimentoViewModel.Nome}, CNPJ: {estabelecimentoViewModel.Cnpj}");
                var estabelecimento = mapper.Map<Estabelecimento>(estabelecimentoViewModel);
                estabelecimentoService.Create(estabelecimento);
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState inválido. Erros:");
            foreach (var key in ModelState.Keys)
            {
                // Correção 1: Armazenamos a entrada e usamos o operador ?.
                var stateEntry = ModelState[key];
                
                if (stateEntry?.Errors.Count > 0)
                {
                    Console.WriteLine($"Erro no campo {key}: {string.Join(", ", stateEntry.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            var gerentes = pessoaService.GetGerentes();
            
            // Correção 2: Usamos ?. para evitar NullReference caso IdUsuarioNavigation seja nulo, 
            // e ?? para definir um valor padrão (fallback)
            ViewBag.Gerentes = gerentes.Select(g => new 
            { 
                g.Id, 
                Nome = g.IdUsuarioNavigation?.UserName ?? "Usuário não definido" 
            });

            return View(estabelecimentoViewModel);
        }

        public ActionResult Edit(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);
            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            var gerentes = pessoaService.GetGerentes();
            ViewBag.Gerentes = gerentes.Select(g => new { g.Id, Nome = g.IdUsuarioNavigation.UserName });
            return View(estabelecimentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EstabelecimentoViewModel estabelecimentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var estabelecimento = mapper.Map<Estabelecimento>(estabelecimentoViewModel);
                estabelecimentoService.Edit(estabelecimento);
                return RedirectToAction(nameof(Index));
            }
            var gerentes = pessoaService.GetGerentes();
            ViewBag.Gerentes = gerentes.Select(g => new { g.Id, Nome = g.IdUsuarioNavigation.UserName });
            return View(estabelecimentoViewModel);
        }

        public ActionResult Delete(uint id)
        {
            var estabelecimento = estabelecimentoService.Get(id);
            var estabelecimentoViewModel = mapper.Map<EstabelecimentoViewModel>(estabelecimento);
            return View(estabelecimentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, EstabelecimentoViewModel estabelecimentoViewModel)
        {
            estabelecimentoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
