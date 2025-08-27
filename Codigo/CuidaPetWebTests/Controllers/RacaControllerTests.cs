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
    public class RacaControllerTests
    {
        private static RacaController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IRacaService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new RacaProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestRacas());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetRaca());
            mockService.Setup(service => service.Edit(It.IsAny<Raca>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Raca>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new RacaController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<RacaViewModel>));

            var listaRacas = (IEnumerable<RacaViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaRacas.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(RacaViewModel));
            RacaViewModel racaModel = (RacaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Poodle", racaModel.Nome);
            Assert.AreEqual(1u, racaModel.IdEspecie);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            var result = controller.Create(GetNewRaca());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewRaca());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(RacaViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(RacaViewModel));
            RacaViewModel racaModel = (RacaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Poodle", racaModel.Nome);
            Assert.AreEqual(1u, racaModel.IdEspecie);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(GetTargetRacaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(RacaViewModel));
            RacaViewModel racaModel = (RacaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Poodle", racaModel.Nome);
            Assert.AreEqual(1u, racaModel.IdEspecie);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetRacaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private RacaViewModel GetNewRaca()
        {
            return new RacaViewModel
            {
                Id = 4,
                Nome = "Bulldog",
                IdEspecie = 2
            };
        }

        private static Raca GetTargetRaca()
        {
            return new Raca
            {
                Id = 1,
                Nome = "Poodle",
                IdEspecie = 1
            };
        }

        private RacaViewModel GetTargetRacaModel()
        {
            return new RacaViewModel
            {
                Id = 1,
                Nome = "Poodle",
                IdEspecie = 1
            };
        }

        private static IEnumerable<Raca> GetTestRacas()
        {
            return new List<Raca>
            {
                new Raca { Id = 1, Nome = "Poodle", IdEspecie = 1 },
                new Raca { Id = 2, Nome = "Persa", IdEspecie = 2 },
                new Raca { Id = 3, Nome = "Labrador", IdEspecie = 1 }
            };
        }
    }
}