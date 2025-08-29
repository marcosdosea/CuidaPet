using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass]
    public class PessoaServiceTests
    {
        private CuidaPetContext context = null!;
        private IPessoaService pessoaService = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<CuidaPetContext>()
                .UseInMemoryDatabase("cuidapetdb");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var tutores = new List<Pessoa>
            {
                new() {
                    Id = 1,
                    Nome = "João Silva",
                    Senha = "senha123",
                    Email = "joao@gmail.com",
                    Telefone = "11999999999",
                    Cpf = "12345678900",
                    Tipo = "T",
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
                    Cpf = "98765432100",
                    Tipo = "T",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Complemento = "Apto 101",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
            };

            context.AddRange(tutores);
            context.SaveChanges();

            pessoaService = new PessoaService(context);
        }

        [TestMethod]
        public void CreateTest()
        {
            var pessoaId = pessoaService.Create(new()
            {
                Id = 3,
                Nome = "Ana Bia",
                Email = "anab@biana.com",
                Telefone = "11977777777",
                Cpf = "11122233344",
                Senha = "123",
                Tipo = "T",
                Status = "A",
                Logradouro = "Rua 1",
                Numero = "10",
                Bairro = "Centro",
                Cidade = "Aracaju",
                Estado = "SE"
            });

            Assert.AreEqual((uint)3, pessoaId);
            Assert.AreEqual(3, pessoaService.GetAll(page, pageSize).Count());
            var pessoa = pessoaService.Get(3);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("Ana Bia", pessoa.Nome);
            Assert.AreEqual("T", pessoa.Tipo);
            Assert.AreEqual("A", pessoa.Status);
            Assert.AreEqual("anab@biana.com", pessoa.Email);
            Assert.AreEqual("11977777777", pessoa.Telefone);
            Assert.AreEqual("11122233344", pessoa.Cpf);
            Assert.AreEqual("123", pessoa.Senha);
            Assert.AreEqual("Rua 1", pessoa.Logradouro);
            Assert.AreEqual("10", pessoa.Numero);
            Assert.AreEqual("Centro", pessoa.Bairro);
            Assert.AreEqual("Aracaju", pessoa.Cidade);
            Assert.AreEqual("SE", pessoa.Estado);
        }

        [TestMethod]
        public void DeleteTest()
        {
            pessoaService.Delete(2);

            var deleted = pessoaService.Get(2);
            Assert.IsNotNull(deleted);
            Assert.AreEqual("I", deleted.Status); // Status deve ser "I" (inativo)
        }

        [TestMethod]
        public void EditTest()
        {
            var pessoa = pessoaService.Get(1);
            Assert.IsNotNull(pessoa);
            pessoa.Nome = "João Alterado";
            pessoa.Email = "joao@ggmail.com";
            pessoa.Senha = "1234";
            pessoa.Telefone = "11988888888";
            pessoa.Logradouro = "Rua 2";
            pessoa.Numero = "20";
            pessoa.Complemento = null;
            pessoa.Bairro = "Bairro 2";
            pessoa.Cidade = "Rio de Janeiro";
            pessoa.Estado = "RJ";

            pessoaService.Edit(pessoa);

            pessoa = pessoaService.Get(1);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("João Alterado", pessoa.Nome);
            Assert.AreEqual("joao@ggmail.com", pessoa.Email);
            Assert.AreEqual("1234", pessoa.Senha);
            Assert.AreEqual("11988888888", pessoa.Telefone);
            Assert.AreEqual("Rua 2", pessoa.Logradouro);
            Assert.AreEqual("20", pessoa.Numero);
            Assert.IsNull(pessoa.Complemento);
            Assert.AreEqual("Bairro 2", pessoa.Bairro);
            Assert.AreEqual("Rio de Janeiro", pessoa.Cidade);
            Assert.AreEqual("RJ", pessoa.Estado);
        }

        [TestMethod]
        public void GetTest()
        {
            var pessoa = pessoaService.Get(2);

            Assert.IsNotNull(pessoa);
            Assert.AreEqual("José Silva", pessoa.Nome);
            Assert.AreEqual("T", pessoa.Tipo);
            Assert.AreEqual("A", pessoa.Status);
            Assert.AreEqual("jose@gmail.com", pessoa.Email);
            Assert.AreEqual("senha123", pessoa.Senha);
            Assert.AreEqual("11999999999", pessoa.Telefone);
            Assert.AreEqual("98765432100", pessoa.Cpf);
            Assert.AreEqual("Rua A", pessoa.Logradouro);
            Assert.AreEqual("100", pessoa.Numero);
            Assert.AreEqual("Apto 101", pessoa.Complemento);
            Assert.AreEqual("Centro", pessoa.Bairro);
            Assert.AreEqual("São Paulo", pessoa.Cidade);
            Assert.AreEqual("SP", pessoa.Estado);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var listaTutores = pessoaService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(listaTutores, typeof(IEnumerable<Pessoa>));
            Assert.IsNotNull(listaTutores);
            Assert.AreEqual(2, listaTutores.Count());
            Assert.AreEqual((uint)1, listaTutores.First().Id);
            Assert.AreEqual("João Silva", listaTutores.First().Nome);
        }

        [TestMethod]
        public void GetCountTest()
        {
            var quantidade = pessoaService.GetCount();

            Assert.AreEqual(2, quantidade);
        }

        [TestMethod]
        public void GetByCpfTest_Existente_DeveRetornarPessoa()
        {
            // Act
            var pessoa = pessoaService.GetByCpf("12345678900");

            // Assert
            Assert.IsNotNull(pessoa);
            Assert.AreEqual<uint>(1, pessoa!.Id);
            Assert.AreEqual("João Silva", pessoa.Nome);
            Assert.IsInstanceOfType(pessoa, typeof(Pessoa));
        }

        [TestMethod]
        public void GetByCpfTest_Inexistente_DeveRetornarNull()
        {
            // Act
            var pessoa = pessoaService.GetByCpf("00000000000");

            // Assert
            Assert.IsNull(pessoa);
        }

        [TestMethod]
        public void GetByCpfTest_PessoaInativa_DeveRetornarPessoa()
        {            
            context.Pessoas.Add(new Pessoa
            {
                Id = 10,
                Nome = "Inativo",
                Senha = "123",
                Email = "inativo@email.com",
                Telefone = "11900000000",
                Cpf = "55544433322",
                Tipo = "T",
                Status = "I", // Inativa
                Logradouro = "Rua X",
                Numero = "1",
                Bairro = "Bairro X",
                Cidade = "Cidade X",
                Estado = "SP"
            });
            context.SaveChanges();

            // Act
            var pessoa = pessoaService.GetByCpf("55544433322");

            // Assert
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("I", pessoa!.Status);
            Assert.AreEqual("Inativo", pessoa.Nome);
            Assert.IsInstanceOfType(pessoa, typeof(Pessoa));
        }
    }
}