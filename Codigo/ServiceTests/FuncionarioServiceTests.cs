using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class FuncionarioServiceTests
    {
        private CuidaPetContext context = null!;
        private IFuncionarioService funcionarioService = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            builder.UseInMemoryDatabase("cuidapetdb");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed Pessoas
            var pessoas = new List<Pessoa>
            {
                new() {
                    Id = 1,
                    Nome = "Dr. João Silva",
                    Senha = "senha123",
                    Email = "joao@gmail.com",
                    Telefone = "99216932392",
                    Cpf = "58348761888",
                    Tipo = "V",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Complemento = "Sala 101",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() {
                    Id = 2,
                    Nome = "Maria Santos",
                    Senha = "senha123",
                    Email = "maria@gmail.com",
                    Telefone = "60349037183",
                    Cpf = "36288887005",
                    Tipo = "T",
                    Status = "A",
                    Logradouro = "Rua B",
                    Numero = "200",
                    Complemento = null,
                    Bairro = "Vila Nova",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() {
                    Id = 3,
                    Nome = "Carlos Atendente",
                    Senha = "senha123",
                    Email = "carlos@gmail.com",
                    Telefone = "70040362868",
                    Cpf = "99512480123",
                    Tipo = "A",
                    Status = "A",
                    Logradouro = "Rua C",
                    Numero = "300",
                    Complemento = null,
                    Bairro = "Jardins",
                    Cidade = "São Paulo",
                    Estado = "SP"
                }
            };

            context.AddRange(pessoas);

            // Seed Estabelecimentos
            var estabelecimentos = new List<Estabelecimento>
            {
                new() {
                    Id = 1,
                    Nome = "Clínica Veterinária Feliz",
                    Tipo = "V",
                    Cnpj = "12.345.678/0001-90",
                    Telefone = "(11) 3333-4444",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Av. Principal",
                    Numero = "500",
                    IdGerente = 1
                },
                new() {
                    Id = 2,
                    Nome = "Pet Shop Amigo",
                    Tipo = "C",
                    Cnpj = "98.765.432/0001-10",
                    Telefone = "(11) 5555-6666",
                    Bairro = "Vila Nova",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua Secundária",
                    Numero = "250",
                    IdGerente = 2
                }
            };

            context.AddRange(estabelecimentos);

            // Seed Funcionarios
            var funcionarios = new List<Funcionario>
            {
                new() {
                    Id = 1,
                    Crmv = "CRMV12345",
                    IdPessoa = 1,
                    IdEstabelecimento = 1
                },
                new() {
                    Id = 2,
                    Crmv = null,
                    IdPessoa = 2,
                    IdEstabelecimento = 1
                },
                new() {
                    Id = 3,
                    Crmv = null,
                    IdPessoa = 3,
                    IdEstabelecimento = 2
                }
            };

            context.AddRange(funcionarios);
            context.SaveChanges();

            funcionarioService = new FuncionarioService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Arrange - Criar uma nova pessoa primeiro
            var novaPessoa = new Pessoa()
            {
                Nome = "Dr. Ana Costa",
                Senha = "senha123",
                Email = "ana@gmail.com",
                Telefone = "27658823635",
                Cpf = "86059708633",
                Tipo = "V",
                Status = "A",
                Logradouro = "Rua D",
                Numero = "400",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
            context.Pessoas.Add(novaPessoa);
            context.SaveChanges();

            // Act
            var novoFuncionarioId = funcionarioService.Create(new Funcionario()
            {
                Crmv = "CRMV67890",
                IdPessoa = novaPessoa.Id,
                IdEstabelecimento = 1
            });

            // Assert
            Assert.IsTrue(novoFuncionarioId > 0);
            Assert.AreEqual(4, funcionarioService.GetAll(page, pageSize).Count());
            var funcionario = funcionarioService.Get(novoFuncionarioId);
            Assert.IsNotNull(funcionario);
            Assert.AreEqual("CRMV67890", funcionario.Crmv);
            Assert.AreEqual(novaPessoa.Id, funcionario.IdPessoa);
            Assert.AreEqual((uint)1, funcionario.IdEstabelecimento);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            funcionarioService.Delete(2);

            // Assert
            Assert.AreEqual(2, funcionarioService.GetAll(page, pageSize).Count());
            var funcionario = funcionarioService.Get(2);
            Assert.IsNull(funcionario);
        }

        [TestMethod()]
        public void DeleteTest_FuncionarioInexistente()
        {
            // Act
            funcionarioService.Delete(999);

            // Assert - Não deve afetar o total de funcionários
            Assert.AreEqual(3, funcionarioService.GetAll(page, pageSize).Count());
        }

        [TestMethod()]
        public void EditTest()
        {
            // Act
            var funcionario = funcionarioService.Get(3);
            Assert.IsNotNull(funcionario);
            funcionario.Crmv = "CRMV99999";
            funcionario.IdEstabelecimento = 1;
            funcionarioService.Edit(funcionario);

            // Assert
            funcionario = funcionarioService.Get(3);
            Assert.IsNotNull(funcionario);
            Assert.AreEqual("CRMV99999", funcionario.Crmv);
            Assert.AreEqual((uint)1, funcionario.IdEstabelecimento);
            Assert.AreEqual((uint)3, funcionario.IdPessoa);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var funcionario = funcionarioService.Get(1);

            // Assert
            Assert.IsNotNull(funcionario);
            Assert.AreEqual("CRMV12345", funcionario.Crmv);
            Assert.AreEqual((uint)1, funcionario.IdPessoa);
            Assert.AreEqual((uint)1, funcionario.IdEstabelecimento);

            // Verificar navegação para Pessoa
            Assert.IsNotNull(funcionario.IdPessoaNavigation);
            Assert.AreEqual("Dr. João Silva", funcionario.IdPessoaNavigation.Nome);
            Assert.AreEqual("V", funcionario.IdPessoaNavigation.Tipo);

            // Verificar navegação para Estabelecimento
            Assert.IsNotNull(funcionario.IdEstabelecimentoNavigation);
            Assert.AreEqual("Clínica Veterinária Feliz", funcionario.IdEstabelecimentoNavigation.Nome);
        }

        [TestMethod()]
        public void GetTest_FuncionarioInexistente()
        {
            // Act
            var funcionario = funcionarioService.Get(999);

            // Assert
            Assert.IsNull(funcionario);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaFuncionarios = funcionarioService.GetAll(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(listaFuncionarios, typeof(IEnumerable<Funcionario>));
            Assert.IsNotNull(listaFuncionarios);
            Assert.AreEqual(3, listaFuncionarios.Count());

            var primeiroFuncionario = listaFuncionarios.First();
            Assert.AreEqual((uint)1, primeiroFuncionario.Id);
            Assert.AreEqual("CRMV12345", primeiroFuncionario.Crmv);

            // Verificar se as navegações estão carregadas
            Assert.IsNotNull(primeiroFuncionario.IdPessoaNavigation);
            Assert.IsNotNull(primeiroFuncionario.IdEstabelecimentoNavigation);
        }

        [TestMethod()]
        public void GetAllTest_Paginacao()
        {
            // Act - Testar primeira página com pageSize = 2
            var funcionariosPagina1 = funcionarioService.GetAll(1, 2);
            var funcionariosPagina2 = funcionarioService.GetAll(2, 2);

            // Assert
            Assert.AreEqual(2, funcionariosPagina1.Count());
            Assert.AreEqual(1, funcionariosPagina2.Count());

            // Verificar ordenação por ID
            Assert.AreEqual((uint)1, funcionariosPagina1.First().Id);
            Assert.AreEqual((uint)2, funcionariosPagina1.ElementAt(1).Id);
            Assert.AreEqual((uint)3, funcionariosPagina2.First().Id);
        }

        [TestMethod()]
        public void GetCountTest()
        {
            // Act
            var count = funcionarioService.GetCount();

            // Assert
            Assert.AreEqual(3, count);
        }

        [TestMethod()]
        public void GetCountTest_AposCreate()
        {
            // Arrange - Criar uma nova pessoa primeiro
            var novaPessoa = new Pessoa()
            {
                Nome = "Dr. Pedro Silva",
                Senha = "senha123",
                Email = "pedro@gmail.com",
                Telefone = "57916591642",
                Cpf = "31156880408",
                Tipo = "V",
                Status = "A",
                Logradouro = "Rua E",
                Numero = "500",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP"
            };
            context.Pessoas.Add(novaPessoa);
            context.SaveChanges();

            funcionarioService.Create(new Funcionario()
            {
                Crmv = null,
                IdPessoa = novaPessoa.Id,
                IdEstabelecimento = 1
            });

            // Act
            var count = funcionarioService.GetCount();

            // Assert
            Assert.AreEqual(4, count);
        }

        [TestMethod()]
        public void GetCountTest_AposDelete()
        {
            // Arrange
            funcionarioService.Delete(1);

            // Act
            var count = funcionarioService.GetCount();

            // Assert
            Assert.AreEqual(2, count);
        }
    }
}