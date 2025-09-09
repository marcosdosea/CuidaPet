using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass]
    public class NotificacaoControllerTests
    {
        private static NotificacaoController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<INotificacaoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            mockService.Setup(s => s.GetAll(page, pageSize))
                .Returns(GetTestNotificacoes());
            mockService.Setup(s => s.Get(1))
                .Returns(GetTargetNotificacao());
            mockService.Setup(s => s.Edit(It.IsAny<Notificacao>()))
                .Verifiable();
            mockService.Setup(s => s.Create(It.IsAny<Notificacao>()))
                .Returns(4);
            mockService.Setup(s => s.Delete(It.IsAny<uint>()))
                .Verifiable();
            mockService.Setup(s => s.GetCount())
                .Returns(GetTestNotificacoes().Count());
            mockService.Setup(s => s.ObterNotificacoesPorPessoa(1))
                .Returns(GetNotificacoesPorPessoa());
            mockService.Setup(s => s.MarcarComoLida(It.IsAny<uint>(), It.IsAny<uint>()))
                .Verifiable();

            controller = new NotificacaoController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<NotificacaoViewModel>));

            var lista = (IEnumerable<NotificacaoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count());
            Assert.AreEqual("Bem-vindo", lista.First().Titulo);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NotificacaoViewModel));

            var model = (NotificacaoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("Bem-vindo", model.Titulo);
            Assert.AreEqual("Seja bem-vindo ao CuidaPet!", model.Descricao);
        }

        [TestMethod]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateTest_Post_Valido()
        {
            var result = controller.Create(GetNewNotificacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Titulo", "Campo requerido");

            var result = controller.Create(GetNewNotificacaoModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NotificacaoViewModel));
        }

        [TestMethod]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NotificacaoViewModel));
            var model = (NotificacaoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("Bem-vindo", model.Titulo);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(1, GetTargetNotificacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NotificacaoViewModel));
            var model = (NotificacaoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("Bem-vindo", model.Titulo);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetNotificacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void MinhasNotificacoesTest_Valido()
        {
            var result = controller.MinhasNotificacoes(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<NotificacaoViewModel>));

            var lista = (IEnumerable<NotificacaoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(2, lista.Count());
            Assert.IsTrue(lista.Any(n => n.Titulo == "Bem-vindo"));
            Assert.IsTrue(lista.Any(n => n.Titulo == "Consulta Agendada"));
        }

        [TestMethod]
        public void MarcarComoLidaTest_Valido()
        {
            var result = controller.MarcarComoLida(1, 1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("MinhasNotificacoes", redirect.ActionName);
            Assert.AreEqual<uint>(1, (uint)redirect.RouteValues["idPessoa"]);
        }

        private static IEnumerable<Notificacao> GetTestNotificacoes()
        {
            return new List<Notificacao>
            {
                new Notificacao {
                    Id = 1,
                    Titulo = "Bem-vindo",
                    Descricao = "Seja bem-vindo ao CuidaPet!",
                    DataEnvio = DateTime.Now.AddDays(-2)
                },
                new Notificacao {
                    Id = 2,
                    Titulo = "Consulta Agendada",
                    Descricao = "Sua consulta foi agendada com sucesso.",
                    DataEnvio = DateTime.Now.AddDays(-1)
                },
                new Notificacao {
                    Id = 3,
                    Titulo = "Pedido Aprovado",
                    Descricao = "Seu pedido foi aprovado e está sendo processado.",
                    DataEnvio = DateTime.Now
                }
            };
        }

        private static Notificacao GetTargetNotificacao()
        {
            return new Notificacao
            {
                Id = 1,
                Titulo = "Bem-vindo",
                Descricao = "Seja bem-vindo ao CuidaPet!",
                DataEnvio = DateTime.Now.AddDays(-2)
            };
        }

        private static NotificacaoViewModel GetTargetNotificacaoModel()
        {
            return new NotificacaoViewModel
            {
                Id = 1,
                Titulo = "Bem-vindo",
                Descricao = "Seja bem-vindo ao CuidaPet!",
                DataEnvio = DateTime.Now.AddDays(-2)
            };
        }

        private static NotificacaoViewModel GetNewNotificacaoModel()
        {
            return new NotificacaoViewModel
            {
                Id = 4,
                Titulo = "Nova Notificação",
                Descricao = "Descrição da nova notificação",
                DataEnvio = DateTime.Now
            };
        }

        private static List<Notificacao> GetNotificacoesPorPessoa()
        {
            return new List<Notificacao>
            {
                new Notificacao {
                    Id = 1,
                    Titulo = "Bem-vindo",
                    Descricao = "Seja bem-vindo ao CuidaPet!",
                    DataEnvio = DateTime.Now.AddDays(-2)
                },
                new Notificacao {
                    Id = 2,
                    Titulo = "Consulta Agendada",
                    Descricao = "Sua consulta foi agendada com sucesso.",
                    DataEnvio = DateTime.Now.AddDays(-1)
                }
            };
        }
    }
}