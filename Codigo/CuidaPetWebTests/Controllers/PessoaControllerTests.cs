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
    public class PessoaControllerTests
    {
        private static PessoaController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IPessoaService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PessoaProfile())).CreateMapper();

            mockService.Setup(s => s.GetAll(page, pageSize))
                .Returns(GetTestPessoas());
            mockService.Setup(s => s.Get(1))
                .Returns(GetTargetPessoa());
            mockService.Setup(s => s.Edit(It.IsAny<Pessoa>()))
                .Verifiable();
            mockService.Setup(s => s.Create(It.IsAny<Pessoa>()))
                .Returns(3);
            mockService.Setup(s => s.Delete(It.IsAny<uint>()))
                .Verifiable();
            mockService.Setup(s => s.GetCount())
                .Returns(GetTestPessoas().Count());

            controller = new PessoaController(mockService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<PessoaViewModel>));

            var lista = (IEnumerable<PessoaViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count());
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));

            var model = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("João Silva", model.Nome);
            Assert.AreEqual("joao@example.com", model.Email);
            Assert.AreEqual("A", model.Status);
            Assert.AreEqual("T", model.Tipo);
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
            var result = controller.Create(GetNewPessoaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewPessoaModel());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
        }

        [TestMethod]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
            var model = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("João Silva", model.Nome);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(GetTargetPessoaModel());

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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
            var model = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, model.Id);
            Assert.AreEqual("João Silva", model.Nome);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetPessoaModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private static IEnumerable<Pessoa> GetTestPessoas()
        {
            return new List<Pessoa>
            {
                new Pessoa {
                    Id = 1, Nome = "João Silva", Senha = "123", Email = "joao@example.com",
                    Telefone = "11999990000", Cpf = "12345678900", Tipo = "T", Status = "A",
                    Logradouro = "Rua 1", Numero = "10", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP"
                },
                new Pessoa {
                    Id = 2, Nome = "Maria Souza", Senha = "123", Email = "maria@example.com",
                    Telefone = "11999990001", Cpf = "12345678901", Tipo = "T", Status = "A",
                    Logradouro = "Rua 2", Numero = "20", Bairro = "Bairro 2", Cidade = "Rio", Estado = "RJ"
                },
                new Pessoa {
                    Id = 3, Nome = "Pedro Santos", Senha = "123", Email = "pedro@example.com",
                    Telefone = "11999990002", Cpf = "12345678902", Tipo = "T", Status = "A",
                    Logradouro = "Rua 3", Numero = "30", Bairro = "Bairro 3", Cidade = "BH", Estado = "MG"
                }
            };
        }

        private static Pessoa GetTargetPessoa()
        {
            return new Pessoa
            {
                Id = 1,
                Nome = "João Silva",
                Senha = "123",
                Email = "joao@example.com",
                Telefone = "11999990000",
                Cpf = "12345678900",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua 1",
                Numero = "10",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private static PessoaViewModel GetTargetPessoaModel()
        {
            return new PessoaViewModel
            {
                Id = 1,
                Nome = "João Silva",
                Senha = "123",
                Email = "joao@example.com",
                Telefone = "11999990000",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua 1",
                Numero = "10",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private static PessoaViewModel GetNewPessoaModel()
        {
            return new PessoaViewModel
            {
                Id = 4,
                Nome = "Novo Tutor",
                Senha = "abc",
                Email = "novo@example.com",
                Telefone = "11988887777",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua Nova",
                Numero = "100",
                Bairro = "Centro",
                Cidade = "Aracaju",
                Estado = "SE"
            };
        }
    }
}