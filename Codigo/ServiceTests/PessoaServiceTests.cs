using Core;
using Core.Context;
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
                    Telefone = "12979696546",
                    Cpf = "41693720779",
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
                    Telefone = "12979696546",
                    Cpf = "22771042568",
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
                Telefone = "11989794106",
                Cpf = "26347877902",
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
            Assert.AreEqual("11989794106", pessoa.Telefone);
            Assert.AreEqual("26347877902", pessoa.Cpf);
            Assert.AreEqual("123", pessoa.Senha);
            Assert.AreEqual("Rua 1", pessoa.Logradouro);
            Assert.AreEqual("10", pessoa.Numero);
            Assert.AreEqual("Centro", pessoa.Bairro);
            Assert.AreEqual("Aracaju", pessoa.Cidade);
            Assert.AreEqual("SE", pessoa.Estado);
        }

        [TestMethod]
        public void Delete_IdExistente_AlterarStatusParaInativo()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Id = 2,
                Nome = "José Carlos de Alencar Neto III",
                Status = "A",
                Tipo = "T",
                Cpf = "75399735954",
                Senha = "XyZ123!@#_ComplexPassword",
                Email = "jose.carlos@ufs.ac.br",
                Telefone = "41955476567",
                Logradouro = "R",
                Numero = "1",
                Bairro = "Atalaia",
                Cidade = "Aracaju",
                Estado = "SE"
            };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            // Act
            pessoaService.Delete(2);

            // Assert
            var deleted = context.Pessoas.Find((uint)2);
            Assert.IsNotNull(deleted);
            Assert.AreEqual("I", deleted.Status);
        }

        [TestMethod]
        public void Edit_PessoaExistente_AtualizarDados()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Alice Soares Pereira",
                Status = "A",
                Tipo = "T",
                Cpf = "43487935228",
                Senha = "=BM9-Fgo5rZ4,YpfUCT~TBXKI$",
                Email = "alice.sp@gmail.com",
                Telefone = "21991379225",
                Logradouro = "R",
                Numero = "1",
                Bairro = "Jardim América",
                Cidade = "São Paulo",
                Estado = "SP"
            };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();
            context.Entry(pessoa).State = EntityState.Detached;

            var pessoaEditada = new Pessoa { Id = 1, Nome = "Alterado", Status = "A", Tipo = "T", Cpf = "43487935228", Senha = "123", Email = "o@o.com", Telefone = "21991379225", Logradouro = "R", Numero = "1", Bairro = "B", Cidade = "C", Estado = "UF" };

            // Act
            pessoaService.Edit(pessoaEditada);

            // Assert
            var atualizado = context.Pessoas.Find((uint)1);
            Assert.IsNotNull(atualizado);
            Assert.AreEqual("Alterado", atualizado.Nome);
        }

        [TestMethod]
        public void Get_IdExistente_RetornarPessoaCorreta()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Id = 10,
                Nome = "Bruno Oliveira Costa",
                Tipo = "T",
                Status = "A",
                Cpf = "36342800260",
                Senha = "B[Xlwuk{5nD6OPM!hf]9K^[~a]",
                Email = "bruno.oc@gmail.net",
                Telefone = "11946769344",
                Logradouro = "R",
                Numero = "1",
                Bairro = "Copacabana",
                Cidade = "Rio de Janeiro",
                Estado = "RJ"
            };
            context.Pessoas.Add(pessoa);
            context.SaveChanges();

            // Act
            var resultado = pessoaService.Get(10);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Busca ID", resultado.Nome);
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
            var pessoa = pessoaService.GetByCpf("41693720779");

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
            var pessoa = pessoaService.GetByCpf("75399735954");

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
                Telefone = "19927046394",
                Cpf = "90005940818",
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
            var pessoa = pessoaService.GetByCpf("90005940818");

            // Assert
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("I", pessoa!.Status);
            Assert.AreEqual("Inativo", pessoa.Nome);
            Assert.IsInstanceOfType(pessoa, typeof(Pessoa));
        }
    }
}