using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using CuidaPetWeb.Controllers;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWebTests.Controllers.Tests
{
    [TestClass()]
    public class PedidoProdutoControllerTests
    {
        private static PedidoProdutoController controller = null!;
        private static Mock<IPedidoProdutoService> mockService = null!;
        private static IMapper mapper = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IPedidoProdutoService>();
            mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(service => service.GetPedidosAtivos(page, pageSize, null, false))
                .Returns(GetTestPedidoProdutos());
            mockService.Setup(service => service.GetCountPedidosAtivos())
                .Returns(3);
            mockService.Setup(service => service.GetDetalhes(1))
                .Returns(GetTargetPedidoProduto());
            mockService.Setup(service => service.GetItensByPedidoId(1))
                .Returns(GetItensPedido());
            mockService.Setup(service => service.AlterarStatus(It.IsAny<uint>(), It.IsAny<string>()))
                .Verifiable();
            mockService.Setup(service => service.RecusarPedido(It.IsAny<uint>()))
                .Verifiable();

            controller = new PedidoProdutoController(mockService.Object, mapper);
        }

        #region Testes Index

        [TestMethod()]
        public void Index_QuandoChamado_DeveRetornarViewResult()
        {
            // Act
            var result = controller.Index(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void Index_QuandoChamado_DeveRetornarListaDePedidos()
        {
            // Act
            var result = controller.Index(page, pageSize);

            // Assert
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<PedidoProdutoViewModel>));

            var listaPedidos = (IEnumerable<PedidoProdutoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaPedidos.Count());
        }

        [TestMethod()]
        public void Index_QuandoChamado_DevePopularViewBagCorretamente()
        {
            // Act
            var result = controller.Index(page, pageSize);

            // Assert
            ViewResult viewResult = (ViewResult)result;

            Assert.IsNotNull(viewResult.ViewData["TotalItems"]);
            Assert.IsNotNull(viewResult.ViewData["Page"]);
            Assert.IsNotNull(viewResult.ViewData["PageSize"]);

            Assert.AreEqual(3, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(page, viewResult.ViewData["Page"]);
            Assert.AreEqual(pageSize, viewResult.ViewData["PageSize"]);
        }

        [TestMethod()]
        public void Index_QuandoOrdenado_DevePassarParametrosDeOrdenacao()
        {
            // Arrange
            var mockServiceSort = new Mock<IPedidoProdutoService>();
            mockServiceSort.Setup(s => s.GetPedidosAtivos(page, pageSize, "RealizadoEm", true))
                .Returns(GetTestPedidoProdutos().OrderByDescending(p => p.RealizadoEm));
            mockServiceSort.Setup(s => s.GetCountPedidosAtivos())
                .Returns(3);

            var controllerWithSort = new PedidoProdutoController(mockServiceSort.Object, mapper);

            // Act
            var result = controllerWithSort.Index(page, pageSize, "RealizadoEm", true);

            // Assert
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual("RealizadoEm", viewResult.ViewData["SortBy"]);
            Assert.AreEqual(true, viewResult.ViewData["Descending"]);
            mockServiceSort.Verify(s => s.GetPedidosAtivos(page, pageSize, "RealizadoEm", true), Times.Once);
        }

        [TestMethod()]
        public void Index_QuandoPaginado_DeveRetornarPaginaCorreta()
        {
            // Arrange
            var mockServicePag = new Mock<IPedidoProdutoService>();
            mockServicePag.Setup(s => s.GetPedidosAtivos(2, 5, null, false))
                .Returns(GetTestPedidoProdutos().Skip(5).Take(5));
            mockServicePag.Setup(s => s.GetCountPedidosAtivos())
                .Returns(10);

            var controllerPagination = new PedidoProdutoController(mockServicePag.Object, mapper);

            // Act
            var result = controllerPagination.Index(2, 5);

            // Assert
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(10, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(2, viewResult.ViewData["Page"]);
            Assert.AreEqual(5, viewResult.ViewData["PageSize"]);
            mockServicePag.Verify(s => s.GetPedidosAtivos(2, 5, null, false), Times.Once);
        }

        [TestMethod()]
        public void Index_QuandoNaoHaPedidos_DeveRetornarListaVazia()
        {
            // Arrange
            var mockServiceEmpty = new Mock<IPedidoProdutoService>();
            mockServiceEmpty.Setup(s => s.GetPedidosAtivos(page, pageSize, null, false))
                .Returns(new List<PedidoProdutoDto>());
            mockServiceEmpty.Setup(s => s.GetCountPedidosAtivos())
                .Returns(0);

            var controllerEmpty = new PedidoProdutoController(mockServiceEmpty.Object, mapper);

            // Act
            var result = controllerEmpty.Index(page, pageSize);

            // Assert
            ViewResult viewResult = (ViewResult)result;
            var listaPedidos = (IEnumerable<PedidoProdutoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(0, listaPedidos.Count());
            Assert.AreEqual(0, viewResult.ViewData["TotalItems"]);
        }

        #endregion

        #region Testes Details

        [TestMethod()]
        public void Details_QuandoPedidoExiste_DeveRetornarViewComModelo()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PedidoProdutoViewModel));
        }

        [TestMethod()]
        public void Details_QuandoPedidoExiste_DeveRetornarDadosCorretos()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            ViewResult viewResult = (ViewResult)result;
            PedidoProdutoViewModel pedidoModel = (PedidoProdutoViewModel)viewResult.ViewData.Model;
            
            Assert.AreEqual<uint>(1, pedidoModel.Id);
            Assert.AreEqual("Ração Premium", pedidoModel.ProdutoNome);
            Assert.AreEqual(2, pedidoModel.Quantidade);
            Assert.AreEqual(300.00m, pedidoModel.PrecoTotal);
            Assert.AreEqual("João Silva", pedidoModel.TutorNome);
        }

        [TestMethod()]
        public void Details_QuandoPedidoNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var mockServiceNotFound = new Mock<IPedidoProdutoService>();
            mockServiceNotFound.Setup(s => s.GetDetalhes(999))
                .Returns((PedidoProdutoDto?)null);

            var controllerNotFound = new PedidoProdutoController(mockServiceNotFound.Object, mapper);

            // Act
            var result = controllerNotFound.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void Details_QuandoChamado_DeveChamarServiceUmaVez()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.GetDetalhes(1))
                .Returns(GetTargetPedidoProduto());

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.Details(1);

            // Assert
            mockServiceVerify.Verify(s => s.GetDetalhes(1), Times.Once);
        }

        #endregion

        #region Testes GetItensPedido

        [TestMethod()]
        public void GetItensPedido_QuandoHaItens_DeveRetornarJsonResult()
        {
            // Act
            var result = controller.GetItensPedido(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod()]
        public void GetItensPedido_QuandoHaItens_DeveRetornarListaDeItens()
        {
            // Act
            var result = controller.GetItensPedido(1);

            // Assert
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var itens = jsonResult.Value as IEnumerable<object>;
            Assert.IsNotNull(itens);
            Assert.AreEqual(2, itens.Count());
        }

        [TestMethod()]
        public void GetItensPedido_QuandoNaoHaItens_DeveRetornarListaVazia()
        {
            // Arrange
            var mockServiceEmpty = new Mock<IPedidoProdutoService>();
            mockServiceEmpty.Setup(s => s.GetItensByPedidoId(999))
                .Returns(new List<PedidoProdutoDto>());

            var controllerEmpty = new PedidoProdutoController(mockServiceEmpty.Object, mapper);

            // Act
            var result = controllerEmpty.GetItensPedido(999);

            // Assert
            JsonResult jsonResult = (JsonResult)result;
            var itens = jsonResult.Value as IEnumerable<object>;
            Assert.IsNotNull(itens);
            Assert.AreEqual(0, itens.Count());
        }

        [TestMethod()]
        public void GetItensPedido_QuandoChamado_DeveChamarServiceComIdCorreto()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.GetItensByPedidoId(1))
                .Returns(GetItensPedido());

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.GetItensPedido(1);

            // Assert
            mockServiceVerify.Verify(s => s.GetItensByPedidoId(1), Times.Once);
        }

        #endregion

        #region Testes Aceitar

        [TestMethod()]
        public void Aceitar_QuandoChamado_DeveRetornarRedirectToAction()
        {
            // Act
            var result = controller.Aceitar(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod()]
        public void Aceitar_QuandoChamado_DeveRedirecionarParaIndex()
        {
            // Act
            var result = controller.Aceitar(1);

            // Assert
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void Aceitar_QuandoChamado_DeveChamarServiceComParametrosCorretos()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.AlterarStatus(1, "F"))
                .Verifiable();

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.Aceitar(1);

            // Assert
            mockServiceVerify.Verify(s => s.AlterarStatus(1, "F"), Times.Once);
        }

        [TestMethod()]
        public void Aceitar_QuandoPedidoValido_DeveAlterarStatusParaFinalizado()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.AlterarStatus(It.IsAny<uint>(), It.IsAny<string>()))
                .Callback<uint, string>((id, status) => 
                {
                    Assert.AreEqual<uint>(1, id);
                    Assert.AreEqual("F", status);
                });

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.Aceitar(1);

            // Assert
            mockServiceVerify.Verify(s => s.AlterarStatus(1, "F"), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        #endregion

        #region Testes Recusar

        [TestMethod()]
        public void Recusar_QuandoChamado_DeveRetornarRedirectToAction()
        {
            // Act
            var result = controller.Recusar(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod()]
        public void Recusar_QuandoChamado_DeveRedirecionarParaIndex()
        {
            // Act
            var result = controller.Recusar(1);

            // Assert
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void Recusar_QuandoChamado_DeveChamarServiceComIdCorreto()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.RecusarPedido(1))
                .Verifiable();

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.Recusar(1);

            // Assert
            mockServiceVerify.Verify(s => s.RecusarPedido(1), Times.Once);
        }

        [TestMethod()]
        public void Recusar_QuandoPedidoValido_DeveRecusarPedido()
        {
            // Arrange
            var mockServiceVerify = new Mock<IPedidoProdutoService>();
            mockServiceVerify.Setup(s => s.RecusarPedido(It.IsAny<uint>()))
                .Callback<uint>((id) => 
                {
                    Assert.AreEqual<uint>(1, id);
                });

            var controllerVerify = new PedidoProdutoController(mockServiceVerify.Object, mapper);

            // Act
            var result = controllerVerify.Recusar(1);

            // Assert
            mockServiceVerify.Verify(s => s.RecusarPedido(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        #endregion

        #region Métodos auxiliares

        private static IEnumerable<PedidoProdutoDto> GetTestPedidoProdutos()
        {
            return new List<PedidoProdutoDto>
            {
                new PedidoProdutoDto
                {
                    Id = 1,
                    PedidoId = 1,
                    ProdutoId = 1,
                    RealizadoEm = DateTime.Now.AddDays(-2),
                    Status = "P",
                    ProdutoNome = "Ração Premium",
                    Quantidade = 2,
                    PrecoUnitario = 150.00m,
                    PrecoTotal = 300.00m,
                    TutorId = 1,
                    TutorNome = "João Silva",
                    TutorTelefone = "5527999999999"
                },
                new PedidoProdutoDto
                {
                    Id = 2,
                    PedidoId = 2,
                    ProdutoId = 2,
                    RealizadoEm = DateTime.Now.AddDays(-1),
                    Status = "P",
                    ProdutoNome = "Brinquedo Interativo",
                    Quantidade = 3,
                    PrecoUnitario = 45.00m,
                    PrecoTotal = 135.00m,
                    TutorId = 2,
                    TutorNome = "Maria Santos",
                    TutorTelefone = "5527988888888"
                },
                new PedidoProdutoDto
                {
                    Id = 3,
                    PedidoId = 3,
                    ProdutoId = 3,
                    RealizadoEm = DateTime.Now,
                    Status = "P",
                    ProdutoNome = "Cama Confortável",
                    Quantidade = 1,
                    PrecoUnitario = 120.00m,
                    PrecoTotal = 120.00m,
                    TutorId = 3,
                    TutorNome = "Pedro Costa",
                    TutorTelefone = "5527977777777"
                }
            };
        }

        private static PedidoProdutoDto GetTargetPedidoProduto()
        {
            return new PedidoProdutoDto
            {
                Id = 1,
                PedidoId = 1,
                ProdutoId = 1,
                RealizadoEm = DateTime.Now.AddDays(-2),
                Status = "P",
                ProdutoNome = "Ração Premium",
                Quantidade = 2,
                PrecoUnitario = 150.00m,
                PrecoTotal = 300.00m,
                TutorId = 1,
                TutorNome = "João Silva",
                TutorTelefone = "5527999999999"
            };
        }

        private static IEnumerable<PedidoProdutoDto> GetItensPedido()
        {
            return new List<PedidoProdutoDto>
            {
                new PedidoProdutoDto
                {
                    Id = 1,
                    PedidoId = 1,
                    ProdutoId = 1,
                    ProdutoNome = "Ração Premium",
                    Quantidade = 2,
                    PrecoUnitario = 150.00m,
                    PrecoTotal = 300.00m,
                    RealizadoEm = DateTime.Now.AddDays(-2),
                    Status = "P",
                    TutorId = 1,
                    TutorNome = "João Silva",
                    TutorTelefone = "5527999999999"
                },
                new PedidoProdutoDto
                {
                    Id = 2,
                    PedidoId = 1,
                    ProdutoId = 2,
                    ProdutoNome = "Brinquedo Interativo",
                    Quantidade = 1,
                    PrecoUnitario = 45.00m,
                    PrecoTotal = 45.00m,
                    RealizadoEm = DateTime.Now.AddDays(-2),
                    Status = "P",
                    TutorId = 1,
                    TutorNome = "João Silva",
                    TutorTelefone = "5527999999999"
                }
            };
        }

        #endregion
    }
}
