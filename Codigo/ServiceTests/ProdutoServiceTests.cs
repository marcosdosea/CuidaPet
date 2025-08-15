using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class ProdutoServiceTests
    {
        private CuidaPetContext context;
        private IProdutoService produtoService;

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

            // Criar dados de teste para Categoria
            var categorias = new List<Categoria>
            {
                new() { Id = 1, Nome = "Ração", Descricao = "Alimentos para pets" },
                new() { Id = 2, Nome = "Brinquedos", Descricao = "Brinquedos para pets" },
                new() { Id = 3, Nome = "Medicamentos", Descricao = "Medicamentos veterinários" }
            };

            context.AddRange(categorias);

            // Criar dados de teste para Estabelecimento
            var estabelecimentos = new List<Estabelecimento>
            {
                new() { Id = 1, Nome = "Pet Shop Central", Cnpj = "12345678000100", Tipo = "P", IdGerente = 1, Cidade = "São Paulo", Estado = "SP", Logradouro = "Rua A", Numero = "123", Bairro = "Centro", Telefone = "11999999999" },
                new() { Id = 2, Nome = "Clínica Veterinária São Paulo", Cnpj = "98765432000100", Tipo = "C", IdGerente = 2, Cidade = "São Paulo", Estado = "SP", Logradouro = "Rua B", Numero = "456", Bairro = "Vila Madalena", Telefone = "11888888888" }
            };

            context.AddRange(estabelecimentos);

            // Criar dados de teste para Produto
            var produtos = new List<Produto>
            {
                new() { Id = 1, Nome = "Ração Premium Cães", Preco = 45.90m, Status = "D", Descricao = "Ração premium para cães adultos", IdCategoria = 1, IdEstabelecimento = 1 },
                new() { Id = 2, Nome = "Bola de Borracha", Preco = 15.50m, Status = "P", PrecoPromocao = 12.40m, Descricao = "Bola de borracha para cães", IdCategoria = 2, IdEstabelecimento = 1 },
                new() { Id = 3, Nome = "Vermífugo", Preco = 25.00m, Status = "D", Descricao = "Vermífugo para cães e gatos", IdCategoria = 3, IdEstabelecimento = 2 }
            };

            context.AddRange(produtos);
            context.SaveChanges();

            produtoService = new ProdutoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novoProdutoId = produtoService.Create(new Produto()
            {
                Id = 4,
                Nome = "Shampoo Pet",
                Preco = 18.90m,
                Status = "D",
                Descricao = "Shampoo para pets",
                IdCategoria = 1,
                IdEstabelecimento = 1
            });

            // Assert
            Assert.AreEqual((uint)4, novoProdutoId);
            Assert.AreEqual(4, produtoService.GetAll().Count());
            var produto = produtoService.Get(4);
            Assert.IsNotNull(produto);
            Assert.AreEqual("Shampoo Pet", produto.Nome);
            Assert.AreEqual(18.90m, produto.Preco);
            Assert.AreEqual("D", produto.Status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            produtoService.Delete(2);

            // Assert
            Assert.AreEqual(2, produtoService.GetAll().Count());
            var produto = produtoService.Get(2);
            Assert.IsNull(produto);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Act 
            var produto = produtoService.Get(3);
            Assert.IsNotNull(produto);
            produto.Nome = "Vermífugo Premium";
            produto.Preco = 35.00m;
            produto.Status = "P";
            produto.PrecoPromocao = 28.00m;
            produtoService.Edit(produto);

            //Assert
            produto = produtoService.Get(3);
            Assert.IsNotNull(produto);
            Assert.AreEqual("Vermífugo Premium", produto.Nome);
            Assert.AreEqual(35.00m, produto.Preco);
            Assert.AreEqual("P", produto.Status);
            Assert.AreEqual(28.00m, produto.PrecoPromocao);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var produto = produtoService.Get(1);

            // Assert
            Assert.IsNotNull(produto);
            Assert.AreEqual("Ração Premium Cães", produto.Nome);
            Assert.AreEqual(45.90m, produto.Preco);
            Assert.AreEqual("D", produto.Status);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaProdutos = produtoService.GetAll();

            // Assert
            Assert.IsInstanceOfType(listaProdutos, typeof(IEnumerable<Produto>));
            Assert.IsNotNull(listaProdutos);
            Assert.AreEqual(3, listaProdutos.Count());
            Assert.AreEqual((uint)1, listaProdutos.First().Id);
            Assert.AreEqual("Ração Premium Cães", listaProdutos.First().Nome);
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            //Act
            var produtos = produtoService.GetByNome("Ração");

            //Assert
            Assert.IsNotNull(produtos);
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual("Ração Premium Cães", produtos.First().Nome);
        }

        [TestMethod()]
        public void GetByEstabelecimentoTest()
        {
            //Act
            var produtos = produtoService.GetByEstabelecimento(1);

            //Assert
            Assert.IsNotNull(produtos);
            Assert.AreEqual(2, produtos.Count());
            Assert.IsTrue(produtos.Any(p => p.Nome == "Ração Premium Cães"));
            Assert.IsTrue(produtos.Any(p => p.Nome == "Bola de Borracha"));
        }

        [TestMethod()]
        public void GetByNomeAndEstabelecimentoTest()
        {
            //Act
            var produtos = produtoService.GetByNomeAndEstabelecimento("Bola", 1);

            //Assert
            Assert.IsNotNull(produtos);
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual("Bola de Borracha", produtos.First().Nome);
            Assert.AreEqual("Pet Shop Central", produtos.First().Estabelecimento);
        }

        [TestMethod()]
        public void GetByCategoriaTest()
        {
            //Act
            var produtos = produtoService.GetByCategoria(1);

            //Assert
            Assert.IsNotNull(produtos);
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual("Ração Premium Cães", produtos.First().Nome);
            Assert.AreEqual("Ração", produtos.First().Categoria);
        }

        [TestMethod()]
        public void GetProdutosPromocaoTest()
        {
            //Act
            var produtos = produtoService.GetProdutosPromocao();

            //Assert
            Assert.IsNotNull(produtos);
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual("Bola de Borracha", produtos.First().Nome);
            Assert.AreEqual("P", produtos.First().Status);
            Assert.IsNotNull(produtos.First().PrecoPromocao);
            Assert.AreEqual(12.40m, produtos.First().PrecoPromocao);
        }
    }
}