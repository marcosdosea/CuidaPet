using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass()]
    public class ProdutoControllerTests
    {
        private static ProdutoController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IProdutoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ProdutoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestProdutos());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetProduto());
            mockService.Setup(service => service.Edit(It.IsAny<Produto>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Produto>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new ProdutoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<ProdutoViewModel>));

            var listaProdutos = (IEnumerable<ProdutoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaProdutos.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ProdutoViewModel));
            ProdutoViewModel produtoModel = (ProdutoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Ração Premium", produtoModel.Nome);
            Assert.AreEqual(150.00m, produtoModel.Preco);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            // Act
            var result = controller.Create(GetNewProduto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller.Create(GetNewProduto());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ProdutoViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ProdutoViewModel));
            ProdutoViewModel produtoModel = (ProdutoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Ração Premium", produtoModel.Nome);
            Assert.AreEqual(150.00m, produtoModel.Preco);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller.Edit(GetTargetProdutoModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ProdutoViewModel));
            ProdutoViewModel produtoModel = (ProdutoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Ração Premium", produtoModel.Nome);
            Assert.AreEqual(150.00m, produtoModel.Preco);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller.Delete(1, GetTargetProdutoModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private ProdutoViewModel GetNewProduto()
        {
            return new ProdutoViewModel
            {
                Id = 4,
                Nome = "Coleira de Couro",
                Descricao = "Coleira resistente de couro legítimo",
                Preco = 89.90m,
                IdCategoria = 1,
                IdEstabelecimento = 1
            };
        }

        private static Produto GetTargetProduto()
        {
            return new Produto
            {
                Id = 1,
                Nome = "Ração Premium",
                Descricao = "Ração de alta qualidade para cães adultos.",
                Preco = 150.00m,
                IdCategoria = 1,
                IdEstabelecimento = 1
            };
        }

        private ProdutoViewModel GetTargetProdutoModel()
        {
            return new ProdutoViewModel
            {
                Id = 1,
                Nome = "Ração Premium",
                Descricao = "Ração de alta qualidade para cães adultos.",
                Preco = 150.00m,
                IdCategoria = 1,
                IdEstabelecimento = 1
            };
        }

        private static IEnumerable<Produto> GetTestProdutos()
        {
            return new List<Produto>
            {
                new Produto { Id = 1, Nome = "Ração Premium", Descricao = "Ração de alta qualidade para cães adultos.", Preco = 150.00m, IdCategoria = 1, IdEstabelecimento = 1 },
                new Produto { Id = 2, Nome = "Brinquedo Interativo", Descricao = "Brinquedo que estimula a mente do seu pet.", Preco = 45.00m, IdCategoria = 2, IdEstabelecimento = 1 },
                new Produto { Id = 3, Nome = "Cama Confortável", Descricao = "Cama macia e confortável para gatos.", Preco = 120.00m, IdCategoria = 3, IdEstabelecimento = 2 }
            };
        }
    }
}