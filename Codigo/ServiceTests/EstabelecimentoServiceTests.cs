using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class EstabelecimentoServiceTests
    {
        private CuidaPetContext context = null!;
        private IEstabelecimentoService estabelecimentoService = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            builder.UseInMemoryDatabase("cuidapetdb");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var gerentes = new List<Pessoa>
            {
                new() {
                    Id = 1,
                    Nome = "João Silva",
                    Senha = "senha123",
                    Email = "joao@gmail.com",
                    Telefone = "11999999999",
                    Cpf = "12345678900",
                    Tipo = "G",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Complemento = "Apto 101",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() {
                    Id = 2,
                    Nome = "José Silva",
                    Senha = "senha123",
                    Email = "jose@gmail.com",
                    Telefone = "11999999999",
                    Cpf = "12345678901",
                    Tipo = "G",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Complemento = "Apto 101",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
            };

            context.AddRange(gerentes);

            var estabelecimentos = new List<Estabelecimento>
            {
                new() {
                    Id = 1,
                    Nome = "Salão Pet Feliz",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 1
                },

                new() {
                    Id = 2,
                    Nome = "Salão Pet Top",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua B",
                    Numero = "123",
                    IdGerente = 2
                },

                new() {
                    Id = 3,
                    Nome = "Clínica Veterinária Bicho Feliz",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 2
                },

                new() {
                    Id = 4,
                    Nome = "Pet Shop Amor de Bicho",
                    Tipo = "V",
                    Cnpj = "00.000.000/0001-00",
                    Telefone = "(00) 0000-0000",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua A",
                    Numero = "123",
                    IdGerente = 1
                }
            };

            context.AddRange(estabelecimentos);
            context.SaveChanges();

            estabelecimentoService = new EstabelecimentoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novaEstabelecimentoId = estabelecimentoService.Create(new()
            {
                Id = 5,
                Nome = "Pet Shop Novo",
                Tipo = "C",
                Cnpj = "00.000.000/0001-00",
                Telefone = "(00) 0000-0000",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Logradouro = "Rua A",
                Numero = "123",
                IdGerente = 1
            });

            // Assert
            Assert.AreEqual((uint)5, novaEstabelecimentoId);
            Assert.AreEqual(5, estabelecimentoService.GetAll(page, pageSize).Count());
            var estabelecimento = estabelecimentoService.Get(5);
            Assert.IsNotNull(estabelecimento);
            Assert.AreEqual("Pet Shop Novo", estabelecimento.Nome);
            Assert.AreEqual<uint>(1, estabelecimento.IdGerente);
            Assert.AreEqual("C", estabelecimento.Tipo);
            Assert.AreEqual("00.000.000/0001-00", estabelecimento.Cnpj);
            Assert.AreEqual("(00) 0000-0000", estabelecimento.Telefone);
            Assert.AreEqual("Rua A", estabelecimento.Logradouro);
            Assert.AreEqual("123", estabelecimento.Numero);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            estabelecimentoService.Delete(2);

            // Assert
            Assert.AreEqual(3, estabelecimentoService.GetAll(page, pageSize).Count());
            var estabelecimento = estabelecimentoService.Get(2);
            Assert.IsNull(estabelecimento);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Act 
            var estabelecimento = estabelecimentoService.Get(3);
            Assert.IsNotNull(estabelecimento);
            estabelecimento.Nome = "Pet Shop Editado";
            estabelecimento.Tipo = "C";
            estabelecimento.Cnpj = "00.000.000/0001-10";
            estabelecimento.Telefone = "(00) 0000-3213";
            estabelecimento.Logradouro = "Rua AB";
            estabelecimento.Numero = "14";
            estabelecimentoService.Edit(estabelecimento);

            //Assert
            estabelecimento = estabelecimentoService.Get(3);
            Assert.IsNotNull(estabelecimento);
            Assert.AreEqual("Pet Shop Editado", estabelecimento.Nome);
            Assert.AreEqual("C", estabelecimento.Tipo);
            Assert.AreEqual("00.000.000/0001-10", estabelecimento.Cnpj);
            Assert.AreEqual("(00) 0000-3213", estabelecimento.Telefone);
            Assert.AreEqual("Rua AB", estabelecimento.Logradouro);
            Assert.AreEqual("14", estabelecimento.Numero);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var estabelecimento = estabelecimentoService.Get(1);

            // Assert
            Assert.IsNotNull(estabelecimento);
            Assert.AreEqual("Salão Pet Feliz", estabelecimento.Nome);
            Assert.AreEqual<uint>(1, estabelecimento.IdGerente);
            Assert.AreEqual("V", estabelecimento.Tipo);
            Assert.AreEqual("00.000.000/0001-00", estabelecimento.Cnpj);
            Assert.AreEqual("(00) 0000-0000", estabelecimento.Telefone);
            Assert.AreEqual("Rua A", estabelecimento.Logradouro);
            Assert.AreEqual("123", estabelecimento.Numero);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaEstabelecimentos = estabelecimentoService.GetAll(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(listaEstabelecimentos, typeof(IEnumerable<Estabelecimento>));
            Assert.IsNotNull(listaEstabelecimentos);
            Assert.AreEqual(4, listaEstabelecimentos.Count());
            Assert.AreEqual((uint)1, listaEstabelecimentos.First().Id);
            Assert.AreEqual("Salão Pet Feliz", listaEstabelecimentos.First().Nome);
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            //Act
            var estabelecimentos = estabelecimentoService.GetByNome("Salão Pet Feliz");

            //Assert
            Assert.IsInstanceOfType(estabelecimentos, typeof(IEnumerable<EstabelecimentoDto>));
            Assert.IsNotNull(estabelecimentos);
            Assert.AreEqual(1, estabelecimentos.Count());
            var estabelecimento = estabelecimentos.First();
            Assert.AreEqual("Salão Pet Feliz", estabelecimento.Nome);
            Assert.IsNotNull(estabelecimento.Gerente);
            Assert.AreEqual("João Silva", estabelecimento.Gerente?.Nome);
        }

        [TestMethod()]
        public void GetByGerenteTest()
        {
            //Act
            var estabelecimentos = estabelecimentoService.GetByGerente(2);

            //Assert
            Assert.IsInstanceOfType(estabelecimentos, typeof(IEnumerable<EstabelecimentoDto>));
            Assert.IsNotNull(estabelecimentos);
            Assert.AreEqual(2, estabelecimentos.Count());
            Assert.IsTrue(estabelecimentos.Any(p => p.Nome == "Clínica Veterinária Bicho Feliz"));
        }
    }
}