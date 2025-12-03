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
    public class DoencaControllerTests
    {
        private static DoencaController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IDoencaService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestDoencas());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetDoenca());
            mockService.Setup(service => service.Edit(It.IsAny<Doenca>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Doenca>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();
            controller = new DoencaController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<DoencaViewModel>));
            var listaDoencas = (IEnumerable<DoencaViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaDoencas.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel));
            DoencaViewModel doencaModel = (DoencaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Raiva", doencaModel.Nome);
            Assert.AreEqual(1u, doencaModel.IdEspecie);
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
            var result = controller.Create(GetNewDoenca());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");

            var result = controller.Create(GetNewDoenca());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel));
            DoencaViewModel doencaModel = (DoencaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Raiva", doencaModel.Nome);
            Assert.AreEqual(1u, doencaModel.IdEspecie);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(GetTargetDoencaModel());
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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel));
            DoencaViewModel doencaModel = (DoencaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Raiva", doencaModel.Nome);
            Assert.AreEqual(1u, doencaModel.IdEspecie);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetDoencaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Post_DeveRetornarView_QuandoModelStateInvalido()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
            var doencaModel = GetTargetDoencaModel();

            // Act
            var result = controller.Edit(doencaModel);

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount, "Deve haver exatamente um erro no ModelState");
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult quando há erros");
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel), "O modelo deve ser do tipo DoencaViewModel");
            Assert.AreEqual(doencaModel.Nome, ((DoencaViewModel)viewResult.ViewData.Model).Nome, "O modelo retornado deve manter os dados originais");
        }

        [TestMethod()]
        public void DetailsTest_DeveRetornarViewComDadosCorretos_QuandoIdValido()
        {
            // Arrange
            uint idDoenca = 1;

            // Act
            var result = controller.Details(idDoenca);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult");
            ViewResult viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.ViewData.Model, "O modelo não deve ser nulo");
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DoencaViewModel), "O modelo deve ser do tipo DoencaViewModel");
            
            DoencaViewModel doencaModel = (DoencaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(idDoenca, doencaModel.Id, "O ID da doença deve corresponder ao solicitado");
            Assert.AreEqual("Raiva", doencaModel.Nome, "O nome da doença deve ser 'Raiva'");
            Assert.AreEqual(1u, doencaModel.IdEspecie, "O ID da espécie deve ser 1");
        }

        [TestMethod()]
        public void IndexTest_DeveRetornarListaVazia_QuandoNaoHouverDoencas()
        {
            // Arrange
            var mockService = new Mock<IDoencaService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(new List<Doenca>());
            var controllerVazio = new DoencaController(mockService.Object, mapper);

            // Act
            var result = controllerVazio.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult");
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<DoencaViewModel>), "O modelo deve ser uma coleção de DoencaViewModel");
            
            var listaDoencas = (IEnumerable<DoencaViewModel>)viewResult.ViewData.Model;
            Assert.IsNotNull(listaDoencas, "A lista não deve ser nula");
            Assert.AreEqual(0, listaDoencas.Count(), "A lista deve estar vazia quando não há doenças");
        }

        private DoencaViewModel GetNewDoenca()
        {
            return new DoencaViewModel()
            {
                Id = 0,
                Nome = "Leishmaniose",
                IdEspecie = 1
            };
        }

        private static Doenca GetTargetDoenca()
        {
            return new Doenca()
            {
                Id = 1,
                Nome = "Raiva",
                IdEspecie = 1
            };
        }

        private DoencaViewModel GetTargetDoencaModel()
        {
            return new DoencaViewModel()
            {
                Id = 1,
                Nome = "Raiva",
                IdEspecie = 1
            };
        }

        private IEnumerable<Doenca> GetTestDoencas()
        {
            return new List<Doenca>
            {
                new Doenca()
                {
                    Id = 1,
                    Nome = "Raiva",
                    IdEspecie = 1
                },
                new Doenca()
                {
                    Id = 2,
                    Nome = "Cinomose",
                    IdEspecie = 1
                },
                new Doenca()
                {
                    Id = 3,
                    Nome = "Leucemia Felina",
                    IdEspecie = 2
                }
            };
        }
    }
}
