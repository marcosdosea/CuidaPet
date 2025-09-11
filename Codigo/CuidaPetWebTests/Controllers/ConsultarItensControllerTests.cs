using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using CuidaPetWeb.Controllers;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass()]
    public class ConsultarItensControllerTests
    {
        private ConsultarItensController? controller = null;
        private Mock<IEstabelecimentoService>? mockEstabelecimentoService;
        private Mock<IProdutoService>? mockProdutoService;
        private IMapper? mapper;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            mockEstabelecimentoService = new Mock<IEstabelecimentoService>();
            mockProdutoService = new Mock<IProdutoService>();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoProfile());
                cfg.CreateMap<ProdutoDTO, ProdutoViewModel>()
                  .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria));
            }).CreateMapper();

            // Setup mocks
            mockEstabelecimentoService.Setup(service => service.GetAll(1, 10))
                .Returns(GetTestEstabelecimentos());
            mockEstabelecimentoService.Setup(service => service.GetCount())
                .Returns(2);

            mockProdutoService.Setup(service => service.GetByEstabelecimento(1))
                .Returns(GetTestProdutosByEstabelecimento(1));
            mockProdutoService.Setup(service => service.GetByEstabelecimento(2))
                .Returns(GetTestProdutosByEstabelecimento(2));
            mockProdutoService.Setup(service => service.GetByNomeAndEstabelecimento("ração", 1))
                .Returns(GetTestProdutosPorNome("ração", 1));
            mockProdutoService.Setup(service => service.GetByNomeAndEstabelecimento("ração", 2))
                .Returns(GetTestProdutosPorNome("ração", 2));
            mockProdutoService.Setup(service => service.GetByNomeAndEstabelecimento("coleira", 1))
                .Returns(new List<ProdutoDTO>());
            mockProdutoService.Setup(service => service.GetByNomeAndEstabelecimento("coleira", 2))
                .Returns(GetTestProdutosPorNome("coleira", 2));

            controller = new ConsultarItensController(
                mockEstabelecimentoService.Object,
                mockProdutoService.Object,
                mapper);
        }

        [TestMethod()]
        public void IndexTest_SemTermoPesquisa_Valido()
        {
            // Act
            var result = controller?.Index(null, 1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ConsultarItensViewModel));

            var viewModel = (ConsultarItensViewModel)viewResult.ViewData.Model;
            Assert.IsNull(viewModel.TermoPesquisa);
            Assert.IsTrue(viewModel.MostrarItens);
            Assert.AreEqual(2, viewModel.Estabelecimentos.Count);

            // Verifica se todos os estabelecimentos têm produtos
            Assert.IsTrue(viewModel.Estabelecimentos.All(e => e.Produtos.Any()));

            // Verifica ViewBag
            Assert.AreEqual(2, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(10, viewResult.ViewData["PageSize"]);
            Assert.AreEqual(1, viewResult.ViewData["Page"]);
        }

        [TestMethod()]
        public void IndexTest_ComTermoPesquisa_Valido()
        {
            // Act
            var result = controller?.Index("ração", 1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ConsultarItensViewModel));

            var viewModel = (ConsultarItensViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("ração", viewModel.TermoPesquisa);
            Assert.IsTrue(viewModel.MostrarItens);
            Assert.AreEqual(2, viewModel.Estabelecimentos.Count);

            // Verifica se os produtos contêm o termo pesquisado
            foreach (var estabelecimento in viewModel.Estabelecimentos)
            {
                Assert.IsTrue(estabelecimento.Produtos.Any());
                Assert.IsTrue(estabelecimento.Produtos.Any(p => p.Nome.Contains("ração", StringComparison.OrdinalIgnoreCase)));
            }
        }

        [TestMethod()]
        public void IndexTest_ComTermoPesquisaNaoEncontrado_Valido()
        {
            // Act
            var result = controller?.Index("produto_inexistente", 1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ConsultarItensViewModel));

            var viewModel = (ConsultarItensViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("produto_inexistente", viewModel.TermoPesquisa);
            Assert.IsTrue(viewModel.MostrarItens);

            // Como nenhum estabelecimento tem produtos com esse termo, 
            // a lista deve estar vazia
            Assert.AreEqual(0, viewModel.Estabelecimentos.Count);
        }

        [TestMethod()]
        public void IndexTest_ComTermoPesquisaParcial_Valido()
        {
            // Act
            var result = controller?.Index("coleira", 1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ConsultarItensViewModel));

            var viewModel = (ConsultarItensViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("coleira", viewModel.TermoPesquisa);
            Assert.IsTrue(viewModel.MostrarItens);

            // Apenas estabelecimento 2 tem produto com "coleira"
            Assert.AreEqual(1, viewModel.Estabelecimentos.Count);
            Assert.AreEqual(2u, viewModel.Estabelecimentos.First().Id);
            Assert.AreEqual("PetShop Feliz", viewModel.Estabelecimentos.First().Nome);
        }

        [TestMethod()]
        public void IndexTest_LimiteCincoProdutosPorEstabelecimento()
        {
            // Arrange - simular estabelecimento com mais de 5 produtos
            mockProdutoService.Setup(service => service.GetByEstabelecimento(1))
                .Returns(GetTestProdutosLimitados());

            // Act
            var result = controller?.Index(null, 1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            var viewModel = (ConsultarItensViewModel)viewResult.ViewData.Model;

            var estabelecimento1 = viewModel.Estabelecimentos.FirstOrDefault(e => e.Id == 1);
            Assert.IsNotNull(estabelecimento1);

            // Deve retornar no máximo 5 produtos
            Assert.IsTrue(estabelecimento1.Produtos.Count <= 5);
        }

        [TestMethod()]
        public void IndexTest_PaginacaoValida()
        {
            // Act
            var result = controller?.Index(null, 2, 5);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            // Verifica ViewBag de paginação
            Assert.AreEqual(2, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(5, viewResult.ViewData["PageSize"]);
            Assert.AreEqual(2, viewResult.ViewData["Page"]);

            // Verifica se o serviço foi chamado com os parâmetros corretos
            mockEstabelecimentoService?.Verify(s => s.GetAll(2, 5), Times.Once);
        }

        [TestMethod()]
        public void PetshopsTest_Valido()
        {
            // Act
            var result = controller?.Petshops();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("ConsultarEstabelecimentos", redirectResult.ControllerName);
        }

        [TestMethod()]
        public void IndexTest_VerificaServicesCall()
        {
            // Act
            controller?.Index("teste", 1, 10);

            // Assert
            mockEstabelecimentoService?.Verify(s => s.GetAll(1, 10), Times.Once);
            mockEstabelecimentoService?.Verify(s => s.GetCount(), Times.Once);
            mockProdutoService?.Verify(s => s.GetByNomeAndEstabelecimento("teste", It.IsAny<uint>()), Times.AtLeastOnce);
        }

        private static IEnumerable<Estabelecimento> GetTestEstabelecimentos()
        {
            return new List<Estabelecimento>
            {
                new Estabelecimento
                {
                    Id = 1,
                    Nome = "Clínica Veterinária ABC",
                    Tipo = "C",
                    Cnpj = "12345678000100",
                    Telefone = "11999999999",
                    Logradouro = "Rua A",
                    Numero = "123",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    IdGerente = 1
                },
                new Estabelecimento
                {
                    Id = 2,
                    Nome = "PetShop Feliz",
                    Tipo = "P",
                    Cnpj = "98765432000100",
                    Telefone = "11888888888",
                    Logradouro = "Rua B",
                    Numero = "456",
                    Bairro = "Vila Nova",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    IdGerente = 2
                }
            };
        }

        private static IEnumerable<ProdutoDTO> GetTestProdutosByEstabelecimento(uint idEstabelecimento)
        {
            if (idEstabelecimento == 1)
            {
                return new List<ProdutoDTO>
                {
                    new ProdutoDTO
                    {
                        Id = 1,
                        Nome = "Ração Premium Cães",
                        Preco = 89.90m,
                        Status = "D",
                        Descricao = "Ração de alta qualidade",
                        Categoria = "Alimentação",
                        Estabelecimento = "Clínica Veterinária ABC"
                    },
                    new ProdutoDTO
                    {
                        Id = 2,
                        Nome = "Shampoo Pet",
                        Preco = 25.50m,
                        Status = "D",
                        Descricao = "Shampoo para pets",
                        Categoria = "Higiene",
                        Estabelecimento = "Clínica Veterinária ABC"
                    }
                };
            }
            else if (idEstabelecimento == 2)
            {
                return new List<ProdutoDTO>
                {
                    new ProdutoDTO
                    {
                        Id = 3,
                        Nome = "Ração Gatos Filhotes",
                        Preco = 45.90m,
                        Status = "P",
                        PrecoPromocao = 39.90m,
                        Descricao = "Ração para gatos filhotes",
                        Categoria = "Alimentação",
                        Estabelecimento = "PetShop Feliz"
                    },
                    new ProdutoDTO
                    {
                        Id = 4,
                        Nome = "Coleira Ajustável",
                        Preco = 35.00m,
                        Status = "D",
                        Descricao = "Coleira ajustável para cães",
                        Categoria = "Acessórios",
                        Estabelecimento = "PetShop Feliz"
                    }
                };
            }

            return new List<ProdutoDTO>();
        }

        private static IEnumerable<ProdutoDTO> GetTestProdutosPorNome(string nome, uint idEstabelecimento)
        {
            var todosProdutos = GetTestProdutosByEstabelecimento(idEstabelecimento);
            return todosProdutos.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<ProdutoDTO> GetTestProdutosLimitados()
        {
            return new List<ProdutoDTO>
            {
                new ProdutoDTO { Id = 1, Nome = "Produto 1", Preco = 10.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 2, Nome = "Produto 2", Preco = 20.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 3, Nome = "Produto 3", Preco = 30.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 4, Nome = "Produto 4", Preco = 40.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 5, Nome = "Produto 5", Preco = 50.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 6, Nome = "Produto 6", Preco = 60.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" },
                new ProdutoDTO { Id = 7, Nome = "Produto 7", Preco = 70.00m, Status = "D", Categoria = "Cat1", Estabelecimento = "Est1" }
            };
        }
    }
}