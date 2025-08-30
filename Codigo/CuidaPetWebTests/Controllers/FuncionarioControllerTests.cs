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
    public class FuncionarioControllerTests
    {
        private FuncionarioController? controller = null;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockFuncionarioService = new Mock<IFuncionarioService>();
            var mockPessoaService = new Mock<IPessoaService>();

            IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FuncionarioProfile());
                cfg.AddProfile(new PessoaProfile());

                // Mapeamento customizado para garantir que Tipo seja mapeado corretamente
                cfg.CreateMap<Funcionario, FuncionarioViewModel>()
                  .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.IdPessoaNavigation.Tipo))
                  .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.IdPessoaNavigation.Nome))
                  .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.IdPessoaNavigation.Cpf))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.IdPessoaNavigation.Email))
                  .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.IdPessoaNavigation.Senha))
                  .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.IdPessoaNavigation.Telefone))
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IdPessoaNavigation.Status))
                  .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.IdPessoaNavigation.Logradouro))
                  .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.IdPessoaNavigation.Numero))
                  .ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => src.IdPessoaNavigation.Complemento))
                  .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.IdPessoaNavigation.Bairro))
                  .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.IdPessoaNavigation.Cidade))
                  .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.IdPessoaNavigation.Estado));

                cfg.CreateMap<FuncionarioViewModel, Funcionario>().ReverseMap();
            }).CreateMapper();

            mockFuncionarioService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestFuncionarios());
            mockFuncionarioService.Setup(service => service.Get(1))
                .Returns(GetTargetFuncionario());
            mockFuncionarioService.Setup(service => service.GetCount())
                .Returns(3);
            mockFuncionarioService.Setup(service => service.Edit(It.IsAny<Funcionario>()))
                .Verifiable();
            mockFuncionarioService.Setup(service => service.Create(It.IsAny<Funcionario>()))
                .Returns(4);
            mockFuncionarioService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            mockPessoaService.Setup(service => service.Get(1))
                .Returns(GetTargetPessoa());
            mockPessoaService.Setup(service => service.GetByCpf("12345678901"))
                .Returns(GetTargetPessoa());
            mockPessoaService.Setup(service => service.GetByCpf("98765432100"))
                .Returns((Pessoa?)null);
            mockPessoaService.Setup(service => service.Create(It.IsAny<Pessoa>()))
                .Returns(5);
            mockPessoaService.Setup(service => service.Edit(It.IsAny<Pessoa>()))
                .Verifiable();

            controller = new FuncionarioController(mockFuncionarioService.Object, mockPessoaService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller?.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<FuncionarioViewModel>));

            var listaFuncionarios = (IEnumerable<FuncionarioViewModel>)viewResult.ViewData.Model;
            // Deve retornar apenas funcionários com tipo "V" ou "T" (filtro no controller)
            Assert.AreEqual(3, listaFuncionarios.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller?.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
            FuncionarioViewModel funcionarioModel = (FuncionarioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, funcionarioModel.Id);
            Assert.AreEqual("CRMV123", funcionarioModel.Crmv);
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
        public void CreateTest_Post_Valido_PessoaExistente()
        {
            // Act
            var result = controller?.Create(GetNewFuncionarioComPessoaExistente());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Valido_PessoaNova()
        {
            // Act
            var result = controller?.Create(GetNewFuncionarioComPessoaNova());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido_CrmvObrigatorio()
        {
            // Arrange
            var funcionario = GetNewFuncionarioVeterinario();
            funcionario.Crmv = null;

            // Act
            var result = controller?.Create(funcionario);

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsTrue(controller?.ModelState.ContainsKey("Crmv"));
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido_ModelState()
        {
            // Arrange
            controller?.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller?.Create(GetNewFuncionarioComPessoaNova());

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller?.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
            FuncionarioViewModel funcionarioModel = (FuncionarioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, funcionarioModel.Id);
            Assert.AreEqual("João Silva", funcionarioModel.Nome);
            Assert.AreEqual("12345678901", funcionarioModel.Cpf);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller?.Edit(GetTargetFuncionarioModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Post_Invalido()
        {
            // Arrange
            controller?.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller?.Edit(GetTargetFuncionarioModel());

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            // Act
            var result = controller?.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FuncionarioViewModel));
            FuncionarioViewModel funcionarioModel = (FuncionarioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual<uint>(1, funcionarioModel.Id);
            Assert.AreEqual("CRMV123", funcionarioModel.Crmv);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller?.Delete(1, GetTargetFuncionarioModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void GetPessoaByCpfTest_PessoaExistente()
        {
            // Act
            var result = controller?.GetPessoaByCpf("12345678901");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);

            // Verifica se contém as propriedades esperadas
            var jsonObject = jsonResult.Value?.GetType().GetProperty("id")?.GetValue(jsonResult.Value);
            Assert.IsNotNull(jsonObject);
        }

        [TestMethod()]
        public void GetPessoaByCpfTest_PessoaNaoExistente()
        {
            // Act
            var result = controller?.GetPessoaByCpf("00000000000");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNull(jsonResult.Value);
        }

        private FuncionarioViewModel GetNewFuncionarioComPessoaExistente()
        {
            return new FuncionarioViewModel
            {
                Id = 4,
                Crmv = null,
                IdPessoa = 1,
                IdEstabelecimento = 1,
                Nome = "João Silva",
                Cpf = "12345678901", // CPF existente
                Email = "joao@email.com",
                Senha = "123456",
                Telefone = "11999999999",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua A",
                Numero = "123",
                Complemento = null,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private FuncionarioViewModel GetNewFuncionarioComPessoaNova()
        {
            return new FuncionarioViewModel
            {
                Id = 5,
                Crmv = null,
                IdPessoa = 5,
                IdEstabelecimento = 1,
                Nome = "Maria Santos",
                Cpf = "98765432100", // CPF não existente
                Email = "maria@email.com",
                Senha = "123456",
                Telefone = "11888888888",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua B",
                Numero = "456",
                Complemento = "Apto 10",
                Bairro = "Vila Nova",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private FuncionarioViewModel GetNewFuncionarioVeterinario()
        {
            return new FuncionarioViewModel
            {
                Id = 6,
                Crmv = "CRMV456",
                IdPessoa = 6,
                IdEstabelecimento = 1,
                Nome = "Dr. Carlos",
                Cpf = "11111111111",
                Email = "carlos@email.com",
                Senha = "123456",
                Telefone = "11777777777",
                Tipo = "V",
                Status = "A",
                Logradouro = "Rua C",
                Numero = "789",
                Complemento = null,
                Bairro = "Jardins",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private static Funcionario GetTargetFuncionario()
        {
            return new Funcionario
            {
                Id = 1,
                Crmv = "CRMV123",
                IdPessoa = 1,
                IdEstabelecimento = 1
            };
        }

        private static Pessoa GetTargetPessoa()
        {
            return new Pessoa
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Email = "joao@email.com",
                Senha = "123456",
                Telefone = "11999999999",
                Tipo = "V",
                Status = "A",
                Logradouro = "Rua A",
                Numero = "123",
                Complemento = null,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private FuncionarioViewModel GetTargetFuncionarioModel()
        {
            return new FuncionarioViewModel
            {
                Id = 1,
                Crmv = "CRMV123",
                IdPessoa = 1,
                IdEstabelecimento = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Email = "joao@email.com",
                Senha = "123456",
                Telefone = "11999999999",
                Tipo = "V",
                Status = "A",
                Logradouro = "Rua A",
                Numero = "123",
                Complemento = null,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
        }

        private static IEnumerable<Funcionario> GetTestFuncionarios()
        {
            return
            [
                new Funcionario {
                    Id = 1,
                    Crmv = "CRMV123",
                    IdPessoa = 1,
                    IdEstabelecimento = 1,
                    IdPessoaNavigation = new Pessoa {
                        Id = 1,
                        Tipo = "V",
                        Nome = "João Silva",
                        Cpf = "12345678901",
                        Email = "joao@email.com",
                        Senha = "123456",
                        Telefone = "11999999999",
                        Status = "A",
                        Logradouro = "Rua A",
                        Numero = "123",
                        Bairro = "Centro",
                        Cidade = "São Paulo",
                        Estado = "SP"
                    }
                },
                new Funcionario {
                    Id = 2,
                    Crmv = null,
                    IdPessoa = 2,
                    IdEstabelecimento = 1,
                    IdPessoaNavigation = new Pessoa {
                        Id = 2,
                        Tipo = "T",
                        Nome = "Maria Santos",
                        Cpf = "98765432100",
                        Email = "maria@email.com",
                        Senha = "123456",
                        Telefone = "11888888888",
                        Status = "A",
                        Logradouro = "Rua B",
                        Numero = "456",
                        Bairro = "Vila Nova",
                        Cidade = "São Paulo",
                        Estado = "SP"
                    }
                },
                new Funcionario {
                    Id = 3,
                    Crmv = null,
                    IdPessoa = 3,
                    IdEstabelecimento = 1,
                    IdPessoaNavigation = new Pessoa {
                        Id = 3,
                        Tipo = "G",
                        Nome = "Carlos Gerente",
                        Cpf = "11111111111",
                        Email = "carlos@email.com",
                        Senha = "123456",
                        Telefone = "11777777777",
                        Status = "A",
                        Logradouro = "Rua C",
                        Numero = "789",
                        Bairro = "Jardins",
                        Cidade = "São Paulo",
                        Estado = "SP"
                    }
                }
            ];
        }
    }
}