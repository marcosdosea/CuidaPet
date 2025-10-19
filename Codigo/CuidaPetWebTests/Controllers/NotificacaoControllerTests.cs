using AutoMapper;
using Core;
using Core.DTO;
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

            // Setup para retornar NotificacaoDto
            mockService.Setup(s => s.ObterNotificacoesComStatusPorPessoa(1))
                .Returns(GetNotificacoesDto());
            mockService.Setup(s => s.ObterNotificacoesComStatusPorPessoa(999))
                .Returns(new List<NotificacaoDto>());

            // Setup para contagem de não lidas
            mockService.Setup(s => s.ObterContagemNaoLidas(1))
                .Returns(2);
            mockService.Setup(s => s.ObterContagemNaoLidas(999))
                .Returns(0);

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
        public void IndexTest_VerificaBadge()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;

            // Verifica se ViewBag tem os valores esperados
            Assert.IsNotNull(viewResult.ViewData["TotalNotificacoes"]);
            Assert.IsNotNull(viewResult.ViewData["NotificacoesNaoLidas"]);
            Assert.AreEqual(3, viewResult.ViewData["TotalNotificacoes"]);
            Assert.AreEqual(2, viewResult.ViewData["NotificacoesNaoLidas"]);
        }

        [TestMethod]
        public void IndexTest_SemNotificacoes()
        {
            // Arrange
            var mockService = new Mock<INotificacaoService>();
            mockService.Setup(s => s.ObterNotificacoesComStatusPorPessoa(It.IsAny<uint>()))
                .Returns(new List<NotificacaoDto>());

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            var controllerEmpty = new NotificacaoController(mockService.Object, mapper);

            // Act
            var result = controllerEmpty.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            var lista = viewResult.ViewData.Model as IEnumerable<NotificacaoViewModel> ?? Enumerable.Empty<NotificacaoViewModel>();
            Assert.AreEqual(0, lista.Count());
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
        public void MarcarComoLidaTest_Valido()
        {
            var result = controller.MarcarComoLida(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void GetContagemNaoLidasTest_Sucesso()
        {
            // Act
            var result = controller.GetContagemNaoLidas();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            // Verifica estrutura da resposta
            var response = jsonResult.Value;
            var successProperty = response?.GetType().GetProperty("success");
            var countProperty = response?.GetType().GetProperty("count");

            Assert.IsNotNull(successProperty);
            Assert.IsNotNull(countProperty);

            var successObj = successProperty?.GetValue(response);
            Assert.IsNotNull(successObj);
            var success = successObj is bool b ? b : false;

            var countObj = countProperty?.GetValue(response);
            Assert.IsNotNull(countObj);
            var count = countObj is int i ? i : 0;

            Assert.IsTrue(success);
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetContagemNaoLidasTest_SemNotificacoes()
        {
            // Arrange
            var mockService = new Mock<INotificacaoService>();
            mockService.Setup(s => s.ObterContagemNaoLidas(999))
                .Returns(0);

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            var controllerEmpty = new NotificacaoController(mockService.Object, mapper);

            // Act
            var result = controllerEmpty.GetContagemNaoLidas();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            var response = jsonResult.Value;
            var countProperty = response?.GetType().GetProperty("count");
            var countObj = countProperty != null ? countProperty.GetValue(response) : null;
            var count = countObj is int i ? i : 0;

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void GetContagemNaoLidasTest_ExcecaoDoServico()
        {
            // Arrange
            var mockServiceWithException = new Mock<INotificacaoService>();
            mockServiceWithException.Setup(s => s.ObterContagemNaoLidas(It.IsAny<uint>()))
                .Throws(new Exception("Erro simulado"));

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new NotificacaoProfile())).CreateMapper();

            var controllerWithException = new NotificacaoController(mockServiceWithException.Object, mapper);

            // Act
            var result = controllerWithException.GetContagemNaoLidas();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            var response = jsonResult.Value;

            var successProperty = response?.GetType().GetProperty("success");
            var messageProperty = response?.GetType().GetProperty("message");

            var successObj = successProperty?.GetValue(response);
            Assert.IsNotNull(successObj);
            var success = successObj is bool b ? b : false;

            var messageObj = messageProperty?.GetValue(response);
            Assert.IsNotNull(messageObj);
            var message = messageObj as string ?? string.Empty;

            Assert.IsFalse(success);
            Assert.AreEqual("Erro simulado", message);
        }

        [TestMethod]
        public void IndexTest_VerificaStatusLida()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            var lista = viewResult.ViewData.Model as IEnumerable<NotificacaoViewModel>;

            // Verifica se as notificações têm o status correto
            Assert.IsNotNull(lista, "A lista de notificações não pode ser nula.");
            Assert.IsTrue(lista.Any(), "A lista de notificações não pode estar vazia.");

            var primeiraNotificacao = lista.First();
            Assert.IsFalse(primeiraNotificacao.EstaLida); // Primeira deve ser não lida (StatusLida = 0)

            var segundaNotificacao = lista.ElementAt(1);
            Assert.IsTrue(segundaNotificacao.EstaLida); // Segunda deve ser lida (StatusLida = 1)
        }

        // Métodos auxiliares
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

        // Novo método para retornar NotificacaoDto
        private static List<NotificacaoDto> GetNotificacoesDto()
        {
            return new List<NotificacaoDto>
            {
                new NotificacaoDto {
                    Id = 1,
                    Titulo = "Bem-vindo",
                    Descricao = "Seja bem-vindo ao CuidaPet!",
                    DataEnvio = DateTime.Now.AddDays(-2),
                    IdPessoa = 1,
                    Lida = false // StatusLida = 0
                },
                new NotificacaoDto {
                    Id = 2,
                    Titulo = "Consulta Agendada",
                    Descricao = "Sua consulta foi agendada com sucesso.",
                    DataEnvio = DateTime.Now.AddDays(-1),
                    IdPessoa = 1,
                    Lida = true // StatusLida = 1
                },
                new NotificacaoDto {
                    Id = 3,
                    Titulo = "Vacina Vencendo",
                    Descricao = "A vacina do seu pet vence em 5 dias.",
                    DataEnvio = DateTime.Now,
                    IdPessoa = 1,
                    Lida = false // StatusLida = 0
                }
            };
        }
    }
}