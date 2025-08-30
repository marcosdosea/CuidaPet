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
    public class EstabelecimentoControllerTests
    {
        private EstabelecimentoController? controller = null;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IEstabelecimentoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EstabelecimentoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestEstabelecimentos());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetEstabelecimento());
            mockService.Setup(service => service.Edit(It.IsAny<Estabelecimento>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Estabelecimento>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new EstabelecimentoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller?.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<EstabelecimentoViewModel>));

            var listaEstabelecimentos = (IEnumerable<EstabelecimentoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(4, listaEstabelecimentos.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller?.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EstabelecimentoViewModel));
            EstabelecimentoViewModel EstabelecimentoModel = (EstabelecimentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Salão Pet Feliz", EstabelecimentoModel.Nome);
            Assert.AreEqual<uint>(1, EstabelecimentoModel.IdGerente);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller?.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            // Act
            var result = controller?.Create(GetNewEstabelecimento());

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
            controller?.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller?.Create(GetNewEstabelecimento());

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EstabelecimentoViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller?.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EstabelecimentoViewModel));
            EstabelecimentoViewModel vacinaModel = (EstabelecimentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Salão Pet Feliz", vacinaModel.Nome);
            Assert.AreEqual<uint>(1, vacinaModel.IdGerente);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller?.Edit(GetTargetEstabelecimentoModel());

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
            var result = controller?.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EstabelecimentoViewModel));
            EstabelecimentoViewModel vacinaModel = (EstabelecimentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Salão Pet Feliz", vacinaModel.Nome);
            Assert.AreEqual<uint>(1, vacinaModel.IdGerente);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller?.Delete(1, GetTargetEstabelecimentoModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private EstabelecimentoViewModel GetNewEstabelecimento()
        {
            return new EstabelecimentoViewModel
            {
                Id = 5,
                Nome = "Pet Shop Novo",
                Tipo = "C",
                Cnpj = "00.000.000/0001-00",
                Telefone = "(00) 0000-0000",
                Logradouro = "Rua A",
                Numero = "123",
                IdGerente = 1
            };
        }

        private static Estabelecimento GetTargetEstabelecimento()
        {
            return new Estabelecimento
            {
                Id = 1,
                Nome = "Salão Pet Feliz",
                Tipo = "V",
                Cnpj = "00.000.000/0001-00",
                Telefone = "(00) 0000-0000",
                Logradouro = "Rua A",
                Numero = "123",
                IdGerente = 1
            };
        }

        private EstabelecimentoViewModel GetTargetEstabelecimentoModel()
        {
            return new EstabelecimentoViewModel
            {
                Id = 1,
                Nome = "Salão Pet Feliz",
                Tipo = "V",
                Cnpj = "00.000.000/0001-00",
                Telefone = "(00) 0000-0000",
                Logradouro = "Rua A",
                Numero = "123",
                IdGerente = 1
            };
        }

        private static IEnumerable<Estabelecimento> GetTestEstabelecimentos()
        {
            return
            [
                new() {
                    Id = 1,
                    Nome = "Salão Pet Feliz",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 1
                },

                new() {
                    Id = 2,
                    Nome = "Salão Pet Top",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Logradouro = "Rua B",
                    Numero = "123",
                    IdGerente = 2
                },

                new() {
                    Id = 3,
                    Nome = "Clínica Veterinária Bicho Feliz",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 2
                },

                new() {
                    Id = 4,
                    Nome = "Pet Shop Amor de Bicho",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 1
                }
            ];
        }
    }
}