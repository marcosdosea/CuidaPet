using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class AgendamentoServiceTests
    {
        private CuidaPetContext context = null!;
        private IAgendamentoService agendamentoService = null!;

        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            builder.UseInMemoryDatabase("AgendamentoServiceTests");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var pessoas = new List<Pessoa>
            {
                new() 
                { 
                    Id = 1, 
                    Nome = "João Silva", 
                    Cpf = "12345678901", 
                    Tipo = "T", 
                    Email = "joao@email.com", 
                    Telefone = "11999999999",
                    Senha = "senha123",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() 
                { 
                    Id = 2, 
                    Nome = "Maria Santos", 
                    Cpf = "98765432101", 
                    Tipo = "F", 
                    Email = "maria@email.com", 
                    Telefone = "11888888888",
                    Senha = "senha456",
                    Status = "A",
                    Logradouro = "Rua B",
                    Numero = "200",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                }
            };

            context.AddRange(pessoas);

            var especies = new List<Especie>
            {
                new() { Id = 1, Nome = "Canina" },
                new() { Id = 2, Nome = "Felina" }
            };

            context.AddRange(especies);

            var racas = new List<Raca>
            {
                new() { Id = 1, Nome = "Labrador", IdEspecie = 1 },
                new() { Id = 2, Nome = "Siamês", IdEspecie = 2 }
            };

            context.AddRange(racas);

            var pets = new List<Pet>
            {      
                new() { Id = 1, Nome = "Rex", DataNascimento = DateTime.Now.AddYears(-3), IdRaca = 1, Sexo = "M" },
                new() { Id = 2, Nome = "Miau", DataNascimento = DateTime.Now.AddYears(-2), IdRaca = 2, Sexo = "F" }
            };

            context.AddRange(pets);

            var estabelecimento = new Estabelecimento
            {
                Id = 1,
                Nome = "Pet Clínica",
                Cnpj = "12345678901234",
                Telefone = "11977777777",
                Logradouro = "Av Principal",
                Numero = "500",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                IdGerente = 1,
                Tipo = "C"
            };

            context.Add(estabelecimento);

            var funcionarios = new List<Funcionario>
            {
                new() { Id = 2, IdPessoa = 2, Crmv = "123456", IdEstabelecimento = 1 }
            };

            context.AddRange(funcionarios);

            var agendamentos = new List<Agendamento>
            {
                new() { Id = 1, DataSolicitacao = DateTime.Now, Horario = new TimeSpan(10, 0, 0), Status = "S", IdPet = 1, IdFuncionario = 2, IdTutor = 1 },
                new() { Id = 2, DataSolicitacao = DateTime.Now.AddDays(-1), Horario = new TimeSpan(14, 0, 0), Status = "A", IdPet = 2, IdFuncionario = 2, IdTutor = 1 },
                new() { Id = 3, DataSolicitacao = DateTime.Now.AddDays(-2), DataConfirmacao = DateTime.Now, Horario = new TimeSpan(16, 0, 0), Status = "R", IdPet = 1, IdFuncionario = 2, IdTutor = 1 }
            };

            context.AddRange(agendamentos);
            context.SaveChanges();

            agendamentoService = new AgendamentoService(context);
        }

        [TestMethod()]
        public void AgendamentoServiceTest()
        {
            // Assert
            Assert.IsNotNull(agendamentoService);
            Assert.IsInstanceOfType(agendamentoService, typeof(IAgendamentoService));
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novoAgendamentoId = agendamentoService.Create(new Agendamento()
            {
                Id = 4,
                DataSolicitacao = DateTime.Now,
                Horario = new TimeSpan(09, 0, 0),
                Status = "S",
                IdPet = 2,
                IdFuncionario = 2,
                IdTutor = 1
            });

            // Assert
            Assert.AreEqual((uint)4, novoAgendamentoId);
            Assert.AreEqual(4, agendamentoService.GetAll(page, pageSize).Count());
            var agendamento = agendamentoService.Get(4);
            Assert.IsNotNull(agendamento);
            Assert.AreEqual("S", agendamento.Status);
            Assert.AreEqual((uint)2, agendamento.IdPet);
            Assert.AreEqual(new TimeSpan(09, 0, 0), agendamento.Horario);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Act
            var agendamento = agendamentoService.Get(2);
            Assert.IsNotNull(agendamento);
            agendamento.Status = "C";
            agendamento.DataConfirmacao = DateTime.Now;
            agendamentoService.Edit(agendamento);

            // Assert
            agendamento = agendamentoService.Get(2);
            Assert.IsNotNull(agendamento);
            Assert.AreEqual("C", agendamento.Status);
            Assert.IsNotNull(agendamento.DataConfirmacao);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            agendamentoService.Delete(1);

            // Assert
            Assert.AreEqual(2, agendamentoService.GetAll(page, pageSize).Count());
            var agendamento = agendamentoService.Get(1);
            Assert.IsNull(agendamento);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var agendamento = agendamentoService.Get(3);

            // Assert
            Assert.IsNotNull(agendamento);
            Assert.AreEqual("R", agendamento.Status);
            Assert.AreEqual((uint)1, agendamento.IdPet);
            Assert.AreEqual(new TimeSpan(16, 0, 0), agendamento.Horario);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaAgendamentos = agendamentoService.GetAll(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(listaAgendamentos, typeof(IEnumerable<Agendamento>));
            Assert.IsNotNull(listaAgendamentos);
            Assert.AreEqual(3, listaAgendamentos.Count());
            Assert.AreEqual((uint)1, listaAgendamentos.First().Id);
        }

        [TestMethod()]
        public void GetByPetTest()
        {
            // Act
            var agendamentos = agendamentoService.GetByPet(1);

            // Assert
            Assert.IsInstanceOfType(agendamentos, typeof(IEnumerable<AgendamentoDto>));
            Assert.IsNotNull(agendamentos);
            Assert.AreEqual(2, agendamentos.Count());
            Assert.IsTrue(agendamentos.Any(a => a.Id == 1));
            Assert.IsTrue(agendamentos.Any(a => a.Id == 3));
        }

        [TestMethod()]
        public void GetByFuncionarioTest()
        {
            // Act
            var agendamentos = agendamentoService.GetByFuncionario(2);

            // Assert
            Assert.IsInstanceOfType(agendamentos, typeof(IEnumerable<AgendamentoDto>));
            Assert.IsNotNull(agendamentos);
            Assert.AreEqual(3, agendamentos.Count());
            Assert.IsTrue(agendamentos.All(a => a.IdFuncionario == 2));
        }

        [TestMethod()]
        public void GetByTutorTest()
        {
            // Act
            var agendamentos = agendamentoService.GetByTutor(1);

            // Assert
            Assert.IsInstanceOfType(agendamentos, typeof(IEnumerable<AgendamentoDto>));
            Assert.IsNotNull(agendamentos);
            Assert.AreEqual(3, agendamentos.Count());
            Assert.IsTrue(agendamentos.All(a => a.IdTutor == 1));
        }

        [TestMethod()]
        public void GetCountTest()
        {
            // Act
            var count = agendamentoService.GetCount();

            // Assert
            Assert.AreEqual(3, count);
        }

        [TestMethod()]
        public void GetAllTest_Paginacao_FluxoControle()
        {
            var novoAgendamento = new Agendamento { Id = 4, DataSolicitacao = DateTime.Now, Horario = TimeSpan.Zero, Status = "S", IdPet = 1, IdFuncionario = 2, IdTutor = 1 };
            context.Agendamentos.Add(novoAgendamento);
            context.SaveChanges();

            var pagina1 = agendamentoService.GetAll(1, 2);
            var pagina2 = agendamentoService.GetAll(2, 2);

            Assert.AreEqual(2, pagina1.Count());
            Assert.AreEqual(2, pagina2.Count());
            Assert.IsFalse(pagina1.Any(p => pagina2.Select(x => x.Id).Contains(p.Id)));
        }

        [TestMethod()]
        public void DeleteTest_IdInexistente_CondicaoFalsa()
        {
            uint idInexistente = 999;
            int totalAntes = agendamentoService.GetAll(1, 100).Count();

            agendamentoService.Delete(idInexistente);
            int totalDepois = agendamentoService.GetAll(1, 100).Count();

            Assert.AreEqual(totalAntes, totalDepois);
        }

        [TestMethod()]
        public void GetByPetTest_FiltroExclusivo_FluxoDados()
        {
            var agendamentosPet1 = agendamentoService.GetByPet(1);

            Assert.IsTrue(agendamentosPet1.All(a => a.IdPet == 1));
            Assert.IsFalse(agendamentosPet1.Any(a => a.IdPet == 2));
            Assert.AreEqual(2, agendamentosPet1.Count());
        }
    }
}