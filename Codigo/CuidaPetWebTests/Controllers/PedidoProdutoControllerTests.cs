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
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
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

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<PedidoProdutoViewModel>));

            var listaPedidos = (IEnumerable<PedidoProdutoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaPedidos.Count());
        }

        [TestMethod()]
        public void IndexTest_VerificaViewBag()
        {
            // Act
            var result = controller.Index(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            Assert.IsNotNull(viewResult.ViewData["TotalItems"]);
            Assert.IsNotNull(viewResult.ViewData["Page"]);
            Assert.IsNotNull(viewResult.ViewData["PageSize"]);

            Assert.AreEqual(3, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(page, viewResult.ViewData["Page"]);
            Assert.AreEqual(pageSize, viewResult.ViewData["PageSize"]);
        }

        [TestMethod()]
        public void IndexTest_ComOrdenacao()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.GetPedidosAtivos(page, pageSize, "RealizadoEm", true))
                .Returns(GetTestPedidoProdutos().OrderByDescending(p => p.RealizadoEm));
            mockService.Setup(s => s.GetCountPedidosAtivos())
                .Returns(3);

            var controllerWithSort = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerWithSort.Index(page, pageSize, "RealizadoEm", true);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual("RealizadoEm", viewResult.ViewData["SortBy"]);
            Assert.AreEqual(true, viewResult.ViewData["Descending"]);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PedidoProdutoViewModel));

            PedidoProdutoViewModel pedidoModel = (PedidoProdutoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, pedidoModel.Id);
            Assert.AreEqual("Ração Premium", pedidoModel.ProdutoNome);
            Assert.AreEqual(2, pedidoModel.Quantidade);
            Assert.AreEqual(300.00m, pedidoModel.PrecoTotal);
            Assert.AreEqual("João Silva", pedidoModel.TutorNome);
        }

        [TestMethod()]
        public void DetailsTest_NotFound()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.GetDetalhes(999))
                .Returns((PedidoProdutoDto?)null);

            var controllerNotFound = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerNotFound.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetItensPedidoTest_Valido()
        {
            // Act
            var result = controller.GetItensPedido(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var itens = jsonResult.Value as IEnumerable<object>;
            Assert.IsNotNull(itens);
            Assert.AreEqual(2, itens.Count());
        }

        [TestMethod()]
        public void GetItensPedidoTest_ListaVazia()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.GetItensByPedidoId(999))
                .Returns(new List<PedidoProdutoDto>());

            var controllerEmpty = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerEmpty.GetItensPedido(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            var itens = jsonResult.Value as IEnumerable<object>;
            Assert.IsNotNull(itens);
            Assert.AreEqual(0, itens.Count());
        }

        [TestMethod()]
        public void AceitarTest_Valido()
        {
            // Act
            var result = controller.Aceitar(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void AceitarTest_VerificaChamadaServico()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.AlterarStatus(1, "F"))
                .Verifiable();

            var controllerVerify = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerVerify.Aceitar(1);

            // Assert
            mockService.Verify(s => s.AlterarStatus(1, "F"), Times.Once);
        }

        [TestMethod()]
        public void RecusarTest_Valido()
        {
            // Act
            var result = controller.Recusar(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void RecusarTest_VerificaChamadaServico()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.RecusarPedido(1))
                .Verifiable();

            var controllerVerify = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerVerify.Recusar(1);

            // Assert
            mockService.Verify(s => s.RecusarPedido(1), Times.Once);
        }

        [TestMethod()]
        public void IndexTest_ComPaginacao()
        {
            // Arrange
            var mockService = new Mock<IPedidoProdutoService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PedidoProdutoProfile())).CreateMapper();

            mockService.Setup(s => s.GetPedidosAtivos(2, 5, null, false))
                .Returns(GetTestPedidoProdutos().Skip(5).Take(5));
            mockService.Setup(s => s.GetCountPedidosAtivos())
                .Returns(10);

            var controllerPagination = new PedidoProdutoController(mockService.Object, mapper);

            // Act
            var result = controllerPagination.Index(2, 5);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(10, viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(2, viewResult.ViewData["Page"]);
            Assert.AreEqual(5, viewResult.ViewData["PageSize"]);
        }

        // Métodos auxiliares
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
                    Status = "P", // Pendente
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
    }
}
