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
    public class AgendamentoControllerTests
    {
        private static AgendamentoController? controller = null;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IAgendamentoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AgendamentoProfile())).CreateMapper();

            mockService.Setup(s => s.GetAll(page, pageSize))
                .Returns(GetTestAgendamentos());
            mockService.Setup(s => s.Get(1))
                .Returns(GetTargetAgendamento());
            mockService.Setup(s => s.Edit(It.IsAny<Agendamento>()))
                .Verifiable();
            mockService.Setup(s => s.Create(It.IsAny<Agendamento>()))
                .Returns(4u);
            mockService.Setup(s => s.Delete(It.IsAny<uint>()))
                .Verifiable();
            mockService.Setup(s => s.GetCount())
                .Returns(GetTestAgendamentos().Count());

            controller = new AgendamentoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller!.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<AgendamentoViewModel>));

            var lista = (IEnumerable<AgendamentoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count());

            Assert.AreEqual(GetTestAgendamentos().Count(), viewResult.ViewData["TotalItems"]);
            Assert.AreEqual(page, viewResult.ViewData["Page"]);
            Assert.AreEqual(pageSize, viewResult.ViewData["PageSize"]);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller!.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AgendamentoViewModel));

            var model = (AgendamentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1u, model.Id);
            Assert.AreEqual("S", model.Status);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            var result = controller!.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            var result = controller!.Create(GetNewAgendamentoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            controller!.ModelState.AddModelError("DataSolicitacao", "Campo requerido");

            var result = controller.Create(GetNewAgendamentoModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AgendamentoViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            var result = controller!.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AgendamentoViewModel));

            var model = (AgendamentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1u, model.Id);
            Assert.AreEqual("S", model.Status);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var result = controller!.Edit(GetTargetAgendamentoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            var result = controller!.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AgendamentoViewModel));

            var model = (AgendamentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1u, model.Id);
            Assert.AreEqual("S", model.Status);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var result = controller!.Delete(1, GetTargetAgendamentoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private AgendamentoViewModel GetNewAgendamentoModel()
        {
            return new AgendamentoViewModel
            {
                Id = 4,
                DataSolicitacao = DateTime.Today,
                Horario = new TimeSpan(9, 0, 0),
                Status = "S",
                IdPet = 1,
                IdFuncionario = 1,
                IdTutor = 1
            };
        }

        private static Agendamento GetTargetAgendamento()
        {
            return new Agendamento
            {
                Id = 1,
                DataSolicitacao = new DateTime(2025, 1, 1),
                DataConfirmacao = null,
                Horario = new TimeSpan(10, 0, 0),
                Status = "S",
                IdPet = 1,
                IdFuncionario = 1,
                IdTutor = 1
            };
        }

        private AgendamentoViewModel GetTargetAgendamentoModel()
        {
            return new AgendamentoViewModel
            {
                Id = 1,
                DataSolicitacao = new DateTime(2025, 1, 1),
                DataConfirmacao = null,
                Horario = new TimeSpan(10, 0, 0),
                Status = "S",
                IdPet = 1,
                IdFuncionario = 1,
                IdTutor = 1
            };
        }

        private static IEnumerable<Agendamento> GetTestAgendamentos()
        {
            return new List<Agendamento>
            {
                new Agendamento { Id = 1, DataSolicitacao = new DateTime(2025,1,1), Horario = new TimeSpan(10,0,0), Status = "S", IdPet = 1, IdFuncionario = 1, IdTutor = 1 },
                new Agendamento { Id = 2, DataSolicitacao = new DateTime(2025,1,2), Horario = new TimeSpan(11,0,0), Status = "A", IdPet = 2, IdFuncionario = 1, IdTutor = 2 },
                new Agendamento { Id = 3, DataSolicitacao = new DateTime(2025,1,3), Horario = new TimeSpan(12,0,0), Status = "R", IdPet = 3, IdFuncionario = 2, IdTutor = 3 }
            };
        }
    }
}