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
            mockService.Setup(s => s.ObterNotificacoesComStatusPorPessoa(1))
                .Returns(GetNotificacoesComStatus());
            mockService.Setup(s => s.ObterNotificacoesComStatusPorPessoa(999))
                .Returns(new List<object>());

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

        [TestMethod]
        public void GetNotificacoesTest_Sucesso()
        {
            // Act
            var result = controller.GetNotificacoes(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            // Verifica se a resposta tem a estrutura esperada
            var response = jsonResult.Value;
            var successProperty = response.GetType().GetProperty("success");
            var dataProperty = response.GetType().GetProperty("data");

            Assert.IsNotNull(successProperty);
            Assert.IsNotNull(dataProperty);

            var success = (bool)successProperty.GetValue(response);
            var data = dataProperty.GetValue(response);

            Assert.IsTrue(success);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void GetNotificacoesTest_SemNotificacoes()
        {
            // Act
            var result = controller.GetNotificacoes(999); // ID que retorna lista vazia

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var response = jsonResult.Value;
            var successProperty = response.GetType().GetProperty("success");
            var dataProperty = response.GetType().GetProperty("data");

            var success = (bool)successProperty.GetValue(response);
            var data = (List<object>)dataProperty.GetValue(response);

            Assert.IsTrue(success);
            Assert.IsNotNull(data);
            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void GetNotificacoesTest_ExcecaoDoServico()
        {
            // Arrange
            var mockServiceWithException = new Mock<INotificacaoService>();
            mockServiceWithException.Setup(s => s.ObterNotificacoesComStatusPorPessoa(It.IsAny<uint>()))
                .Throws(new Exception("Erro simulado"));

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            var controllerWithException = new NotificacaoController(mockServiceWithException.Object, mapper);

            // Act
            var result = controllerWithException.GetNotificacoes(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var response = jsonResult.Value;
            var successProperty = response.GetType().GetProperty("success");
            var messageProperty = response.GetType().GetProperty("message");

            var success = (bool)successProperty.GetValue(response);
            var message = (string)messageProperty.GetValue(response);

            Assert.IsFalse(success);
            Assert.AreEqual("Erro simulado", message);
        }

        [TestMethod]
        public void MarcarComoLidaAjaxTest_Sucesso()
        {
            // Act
            var result = controller.MarcarComoLidaAjax(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var response = jsonResult.Value;
            var successProperty = response.GetType().GetProperty("success");

            Assert.IsNotNull(successProperty);
            var success = (bool)successProperty.GetValue(response);
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void MarcarComoLidaAjaxTest_ExcecaoDoServico()
        {
            // Arrange
            var mockServiceWithException = new Mock<INotificacaoService>();
            mockServiceWithException.Setup(s => s.MarcarComoLida(It.IsAny<uint>(), It.IsAny<uint>()))
                .Throws(new Exception("Erro ao marcar como lida"));

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            var controllerWithException = new NotificacaoController(mockServiceWithException.Object, mapper);

            // Act
            var result = controllerWithException.MarcarComoLidaAjax(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            var response = jsonResult.Value;
            var successProperty = response.GetType().GetProperty("success");
            var messageProperty = response.GetType().GetProperty("message");

            var success = (bool)successProperty.GetValue(response);
            var message = (string)messageProperty.GetValue(response);

            Assert.IsFalse(success);
            Assert.AreEqual("Erro ao marcar como lida", message);
        }

        [TestMethod]
        public void MarcarComoLidaAjaxTest_ParametrosValidos()
        {
            // Arrange
            uint idNotificacao = 5;
            uint idPessoa = 3;

            // Act
            var result = controller.MarcarComoLidaAjax(idNotificacao, idPessoa);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
        }

        [TestMethod]
        public void GetNotificacoesTest_VerificaEstruturaDados()
        {
            // Act
            var result = controller.GetNotificacoes(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;

            var response = jsonResult.Value;
            var dataProperty = response.GetType().GetProperty("data");
            var data = (List<object>)dataProperty.GetValue(response);

            Assert.IsTrue(data.Count > 0);

            var firstItem = data.First();
            var properties = firstItem.GetType().GetProperties();

            var expectedProperties = new[] { "Id", "Titulo", "Descricao", "DataEnvio", "StatusLida" };
            foreach (var expectedProp in expectedProperties)
            {
                Assert.IsTrue(properties.Any(p => p.Name == expectedProp),
                    $"Propriedade {expectedProp} não encontrada");
            }
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

        private static List<object> GetNotificacoesComStatus()
        {
            return new List<object>
            {
                new {
                    Id = 1u,
                    Titulo = "Bem-vindo",
                    Descricao = "Seja bem-vindo ao CuidaPet!",
                    DataEnvio = DateTime.Now.AddDays(-2),
                    StatusLida = (sbyte)0
                },
                new {
                    Id = 2u,
                    Titulo = "Consulta Agendada",
                    Descricao = "Sua consulta foi agendada com sucesso.",
                    DataEnvio = DateTime.Now.AddDays(-1),
                    StatusLida = (sbyte)1
                },
                new {
                    Id = 3u,
                    Titulo = "Vacina Vencendo",
                    Descricao = "A vacina do seu pet vence em 5 dias.",
                    DataEnvio = DateTime.Now,
                    StatusLida = (sbyte)0
                }
            };
        }
    }
}