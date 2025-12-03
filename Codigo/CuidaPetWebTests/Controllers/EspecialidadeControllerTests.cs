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
    public class EspecialidadeControllerTests
    {
        private static EspecialidadeController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IEspecialidadeService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EspecialidadeProfile())).CreateMapper();
            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestEspecialidades());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetEspecialidade());
            mockService.Setup(service => service.Edit(It.IsAny<Especialidade>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Especialidade>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new EspecialidadeController(mockService.Object, mapper);
        }
        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<EspecialidadeViewModel>));
            var listaEspecialidades = (IEnumerable<EspecialidadeViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaEspecialidades.Count());
        }
        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecialidadeViewModel));
            var especialidade = (EspecialidadeViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardiologia", especialidade.Nome);
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
            var result = controller.Create(GetNewEspecialidade());
            
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
            // Act
            var result = controller.Create(GetNewEspecialidade());
            
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecialidadeViewModel));  
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller.Edit(1);
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecialidadeViewModel));
            EspecialidadeViewModel especialidadeModel = (EspecialidadeViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardiologia", especialidadeModel.Nome);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(GetTargetEspecialidadeModel());

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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecialidadeViewModel));
            EspecialidadeViewModel especialidadeModel = (EspecialidadeViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardiologia", especialidadeModel.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetEspecialidadeModel());

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
            var especialidadeModel = GetTargetEspecialidadeModel();

            // Act
            var result = controller.Edit(especialidadeModel);

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount, "Deve haver exatamente um erro no ModelState");
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult quando há erros de validação");
            
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(EspecialidadeViewModel), "O modelo deve ser do tipo EspecialidadeViewModel");
            
            var modelRetornado = (EspecialidadeViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(especialidadeModel.Nome, modelRetornado.Nome, "O modelo retornado deve preservar os dados originais");
        }

        [TestMethod()]
        public void IndexTest_DeveRetornarListaVazia_QuandoNaoHouverEspecialidades()
        {
            // Arrange
            var mockService = new Mock<IEspecialidadeService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EspecialidadeProfile())).CreateMapper();
            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(new List<Especialidade>());
            
            var controllerVazio = new EspecialidadeController(mockService.Object, mapper);

            // Act
            var result = controllerVazio.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult");
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<EspecialidadeViewModel>), "O modelo deve ser uma coleção de EspecialidadeViewModel");
            
            var listaEspecialidades = (IEnumerable<EspecialidadeViewModel>)viewResult.ViewData.Model;
            Assert.IsNotNull(listaEspecialidades, "A lista não deve ser nula");
            Assert.AreEqual(0, listaEspecialidades.Count(), "A lista deve estar vazia quando não há especialidades");
        }

        [TestMethod()]
        public void DetailsTest_DeveRetornarViewComDescricao_QuandoEspecialidadePossuiDescricao()
        {
            // Arrange
            var mockService = new Mock<IEspecialidadeService>();
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EspecialidadeProfile())).CreateMapper();
            
            var especialidadeComDescricao = new Especialidade
            {
                Id = 1,
                Nome = "Cardiologia",
                Descricao = "Especialista em coração e sistema cardiovascular"
            };
            
            mockService.Setup(service => service.Get(1))
                .Returns(especialidadeComDescricao);
            
            var controllerComDescricao = new EspecialidadeController(mockService.Object, mapper);

            // Act
            var result = controllerComDescricao.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Deve retornar uma ViewResult");
            ViewResult viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.ViewData.Model, "O modelo não deve ser nulo");
            
            var especialidadeViewModel = (EspecialidadeViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardiologia", especialidadeViewModel.Nome, "O nome deve corresponder ao esperado");
            Assert.AreEqual("Especialista em coração e sistema cardiovascular", especialidadeViewModel.Descricao, "A descrição deve estar presente e correta");
        }

        private EspecialidadeViewModel GetNewEspecialidade()
        {
            return new EspecialidadeViewModel
            {
                Id = 4,
                Nome = "Neurologia"
            };
        }

        private static Especialidade GetTargetEspecialidade()
        {
            return new Especialidade
            {
                Id = 1,
                Nome = "Cardiologia"
            };
        }

        private static EspecialidadeViewModel GetTargetEspecialidadeModel()
        {
            return new EspecialidadeViewModel
            {
                Id = 1,
                Nome = "Cardiologia"
            };
        }

        private static IEnumerable<Especialidade> GetTestEspecialidades()
        {
            return new List<Especialidade>
            {
                new Especialidade
                {
                    Id = 1,
                    Nome = "Cardiologia"
                },
                new Especialidade
                {
                    Id = 2,
                    Nome = "Dermatologia"
                },
                new Especialidade
                {
                    Id = 3,
                    Nome = "Ortopedia"
                }
            };
        }
    }
}
