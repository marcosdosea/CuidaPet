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
    public class EspecieControllerTests
    {
        private static EspecieController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IEspecieService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EspecieProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestEspecies());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetEspecie());
            mockService.Setup(service => service.Edit(It.IsAny<Especie>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Especie>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new EspecieController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<EspecieViewModel>));

            var listaEspecies = (IEnumerable<EspecieViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaEspecies.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecieViewModel));
            EspecieViewModel especieModel = (EspecieViewModel)viewResult.ViewData.Model;
            Assert.IsTrue(1 == especieModel.Id);
            Assert.AreEqual("Cachorro", especieModel.Nome);
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
            var result = controller.Create(GetNewEspecie());

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
            var result = controller.Create(GetNewEspecie());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecieViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecieViewModel));
            EspecieViewModel especieModel = (EspecieViewModel)viewResult.ViewData.Model;
            Assert.IsTrue(1 == especieModel.Id);
            Assert.AreEqual("Cachorro", especieModel.Nome);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller.Edit(GetTargetEspecieModel());

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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecieViewModel));
            EspecieViewModel especieModel = (EspecieViewModel)viewResult.ViewData.Model;
            Assert.IsTrue(1 == especieModel.Id);
            Assert.AreEqual("Cachorro", especieModel.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller.Delete(1, GetTargetEspecieModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private EspecieViewModel GetNewEspecie()
        {
            return new EspecieViewModel
            {
                Id = 4,
                Nome = "Ave"
            };
        }

        private static Especie GetTargetEspecie()
        {
            return new Especie
            {
                Id = 1,
                Nome = "Cachorro"
            };
        }

        private EspecieViewModel GetTargetEspecieModel()
        {
            return new EspecieViewModel
            {
                Id = 1,
                Nome = "Cachorro"
            };
        }

        private static IEnumerable<Especie> GetTestEspecies()
        {
            return
            [
                new Especie { Id = 1, Nome = "Cachorro"},
                new Especie { Id = 2, Nome = "Gato"},
                new Especie { Id = 3, Nome = "Peixe"}
            ];
        }
    }
}