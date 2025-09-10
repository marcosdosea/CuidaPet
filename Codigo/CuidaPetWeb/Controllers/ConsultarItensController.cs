using Core.DTO;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class ConsultarItensController : Controller
    {
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly IProdutoService _produtoService;
        private readonly AutoMapper.IMapper _mapper;

        public ConsultarItensController(IEstabelecimentoService estabelecimentoService, IProdutoService produtoService, AutoMapper.IMapper mapper)
        {
            _estabelecimentoService = estabelecimentoService;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        // GET: ConsultarItens
        public ActionResult Index(string? termoPesquisa, int page = 1, int pageSize = 10)
        {
            var estabelecimentos = _estabelecimentoService.GetAll(page, pageSize).ToList();
            var totalItems = _estabelecimentoService.GetCount();

            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            var viewModel = new ConsultarItensViewModel
            {
                TermoPesquisa = termoPesquisa,
                MostrarItens = true
            };

            foreach (var est in estabelecimentos)
            {
                var produtos = string.IsNullOrEmpty(termoPesquisa)
                    ? _produtoService.GetByEstabelecimento(est.Id).Take(5).ToList()
                    : _produtoService.GetByNomeAndEstabelecimento(termoPesquisa, est.Id).Take(5).ToList();

                if (!produtos.Any() && !string.IsNullOrEmpty(termoPesquisa))
                    continue;

                var produtosViewModel = _mapper.Map<List<ProdutoViewModel>>(produtos);

                viewModel.Estabelecimentos.Add(new EstabelecimentoComProdutosViewModel
                {
                    Id = est.Id,
                    Nome = est.Nome,
                    Produtos = produtosViewModel
                });
            }

            return View(viewModel);
        }

        public ActionResult Petshops()
        {
            return RedirectToAction("Index", "ConsultarEstabelecimentos");
        }
    }
}