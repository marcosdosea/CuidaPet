using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Controllers;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass()]
    public class VacinaControllerTests
    {
        private VacinaController ?controller = null;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IVacinaService>();
            var mockDoencaService = new Mock<IDoencaService>();
            var mockEspecieService = new Mock<IEspecieService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new VacinaProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestVacinas());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetVacina());
            mockService.Setup(service => service.Edit(It.IsAny<Vacina>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Vacina>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new VacinaController(
                mockService.Object,
                mockDoencaService.Object,
                mockEspecieService.Object,
                mapper
            );
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller?.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<VacinaViewModel>));

            var listaVacinas = (IEnumerable<VacinaViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(4, listaVacinas.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller?.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VacinaViewModel));
            VacinaViewModel VacinaModel = (VacinaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Antirrábica", VacinaModel.Nome);
            Assert.AreEqual<uint>(1, VacinaModel.IdEspecie);
            Assert.AreEqual<uint>(1, VacinaModel.IdDoenca);
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
            var result = controller?.Create(GetNewVacina());

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
            var result = controller?.Create(GetNewVacina());

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VacinaViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller?.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VacinaViewModel));
            VacinaViewModel vacinaModel = (VacinaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Antirrábica", vacinaModel.Nome);
            Assert.AreEqual<uint>(1, vacinaModel.IdEspecie);
            Assert.AreEqual<uint>(1, vacinaModel.IdDoenca);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller?.Edit(GetTargetVacinaModel());

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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(VacinaViewModel));
            VacinaViewModel vacinaModel = (VacinaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Antirrábica", vacinaModel.Nome);
            Assert.AreEqual<uint>(1, vacinaModel.IdEspecie);
            Assert.AreEqual<uint>(1, vacinaModel.IdDoenca);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller?.Delete(1, GetTargetVacinaModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private VacinaViewModel GetNewVacina()
        {
            return new VacinaViewModel
            {
                Id = 5,
                Nome = "Vacina contra Mixomatose",
                PeriodoEmDias = 365,
                IdDoenca = 2,
                IdEspecie = 2
            };
        }

        private static Vacina GetTargetVacina()
        {
            return new Vacina
            {
                Id = 1,
                Nome = "Antirrábica",
                PeriodoEmDias = 365,
                IdDoenca = 1,
                IdEspecie = 1
            };
        }

        private VacinaViewModel GetTargetVacinaModel()
        {
            return new VacinaViewModel
            {
                Id = 1,
                Nome = "Antirrábica",
                PeriodoEmDias = 365,
                IdDoenca = 1,
                IdEspecie = 1
            };
        }

        private static IEnumerable<Vacina> GetTestVacinas()
        {
            return
            [
                new() {
                    Id = 1,
                    Nome = "Antirrábica",
                    PeriodoEmDias = 365,
                    IdDoenca = 1,
                    IdEspecie = 1
                },

                new() {
                    Id = 2,
                    Nome = "Polivalente V10",
                    PeriodoEmDias = 365,
                    IdDoenca = 2,
                    IdEspecie = 1
                },

                new() {
                    Id = 3,
                    Nome = "Quádrupla Felina",
                    PeriodoEmDias = 365,
                    IdDoenca = 3,
                    IdEspecie = 2
                },

                new() {
                    Id = 4,
                    Nome = "Giárdia Canina - 1ª Dose",
                    PeriodoEmDias = 21,
                    IdDoenca = 4,
                    IdEspecie = 1
                }
            ];
        }
    }
}