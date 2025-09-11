using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass()]
    public class ConsultarEstabelecimentosControllerTests
    {
        private ConsultarEstabelecimentosController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockEstabelecimentoService = new Mock<IEstabelecimentoService>();
            var mockProdutoService = new Mock<IProdutoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
            {
                // Adicione o profile correto do AutoMapper aqui
                cfg.CreateMap<Estabelecimento, ConsultarEstabelecimentosViewModel>();
                cfg.CreateMap<Estabelecimento, DetalhesEstabelecimentoViewModel>();
                cfg.CreateMap<ProdutoDTO, ProdutoViewModel>();
            }).CreateMapper();

            mockEstabelecimentoService.Setup(s => s.GetAll(page, pageSize))
                .Returns(GetTestEstabelecimentos());
            mockEstabelecimentoService.Setup(s => s.GetCount())
                .Returns(3);
            mockEstabelecimentoService.Setup(s => s.Get(1))
                .Returns(GetTargetEstabelecimento());

            mockProdutoService.Setup(s => s.GetByEstabelecimento(1))
                .Returns(GetTestProdutos());

            controller = new ConsultarEstabelecimentosController(
                mockEstabelecimentoService.Object,
                mockProdutoService.Object,
                mapper
            );
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index(page, pageSize);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<ConsultarEstabelecimentosViewModel>));
            var lista = (List<ConsultarEstabelecimentosViewModel>)viewResult.Model;
            Assert.AreEqual(3, lista.Count);
            Assert.AreEqual(3, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(pageSize, viewResult.ViewData["PageSize"]);
            Assert.AreEqual(page, viewResult.ViewData["Page"]);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(DetalhesEstabelecimentoViewModel));
            var detalhes = (DetalhesEstabelecimentoViewModel)viewResult.Model;
            Assert.AreEqual(1u, detalhes.Id);
            Assert.AreEqual("Petshop Central", detalhes.Nome);
            Assert.IsNotNull(detalhes.Produtos);
            Assert.AreEqual(2, detalhes.Produtos.Count);
        }

        private IEnumerable<Estabelecimento> GetTestEstabelecimentos()
        {
            return new List<Estabelecimento>
            {
                new Estabelecimento { Id = 1, Nome = "Petshop Central", Tipo = "P", Telefone = "1111-1111", Cidade = "CidadeA", Bairro = "Centro", Logradouro = "Rua A", Numero = "100", IdGerente = 1 },
                new Estabelecimento { Id = 2, Nome = "Clínica Animal", Tipo = "C", Telefone = "2222-2222", Cidade = "CidadeB", Bairro = "BairroB", Logradouro = "Rua B", Numero = "200", IdGerente = 2 },
                new Estabelecimento { Id = 3, Nome = "Pet & Vet", Tipo = "A", Telefone = "3333-3333", Cidade = "CidadeC", Bairro = "BairroC", Logradouro = "Rua C", Numero = "300", IdGerente = 3 }
            };
        }

        private Estabelecimento GetTargetEstabelecimento()
        {
            return new Estabelecimento
            {
                Id = 1,
                Nome = "Petshop Central",
                Tipo = "P",
                Telefone = "1111-1111",
                Cidade = "CidadeA",
                Bairro = "Centro",
                Logradouro = "Rua A",
                Numero = "100",
                IdGerente = 1
            };
        }

        private IEnumerable<ProdutoDTO> GetTestProdutos()
        {
            return new List<ProdutoDTO>
            {
                new ProdutoDTO { Id = 1, Nome = "Ração Premium", Preco = 99.90M, Status = "D", Categoria = "Ração", Estabelecimento = "Petshop Central" },
                new ProdutoDTO { Id = 2, Nome = "Brinquedo Bola", Preco = 19.90M, Status = "D", Categoria = "Brinquedo", Estabelecimento = "Petshop Central" }
            };
        }
    }
}