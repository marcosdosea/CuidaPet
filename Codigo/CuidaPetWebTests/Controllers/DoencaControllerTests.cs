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
        public void IndexTest_CondicaoComposta_PaginaPadrao()
        {
            var result = controller.Index(1, 10);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(1, viewResult.ViewData["Page"]);
            Assert.AreEqual(10, viewResult.ViewData["PageSize"]);
        }

        [TestMethod()]
        public void IndexTest_CondicaoComposta_PaginaPersonalizada()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.GetAll(2, 5)).Returns(GetTestDoencas().Take(2));
            mockService.Setup(s => s.GetCount()).Returns(3);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Index(2, 5);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(2, viewResult.ViewData["Page"]);
            Assert.AreEqual(5, viewResult.ViewData["PageSize"]);
        }

        [TestMethod()]
        public void IndexTest_CondicaoComposta_PaginaZero()
        {
            var result = controller.Index(0, 10);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void IndexTest_CondicaoComposta_PageSizeZero()
        {
            var result = controller.Index(1, 0);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void DetailsTest_AdivinhacaoErro_IdInexistente()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.Get(999)).Returns((Doenca?)null);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Details(999);
        }

        [TestMethod()]
        public void DetailsTest_AdivinhacaoErro_IdZero()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.Get(0)).Returns((Doenca?)null);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Details(0);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void DetailsTest_AdivinhacaoErro_IdMaximo()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.Get(uint.MaxValue)).Returns((Doenca?)null);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Details(uint.MaxValue);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void EditTest_Get_AdivinhacaoErro_IdInexistente()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.Get(999)).Returns((Doenca?)null);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Edit(999);
        }

        [TestMethod()]
        public void DeleteTest_Get_AdivinhacaoErro_IdInexistente()
        {
            var mockService = new Mock<IDoencaService>();
            mockService.Setup(s => s.Get(999)).Returns((Doenca?)null);
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DoencaProfile())).CreateMapper();
            var tempController = new DoencaController(mockService.Object, mapper);
            
            var result = tempController.Delete(999);
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
