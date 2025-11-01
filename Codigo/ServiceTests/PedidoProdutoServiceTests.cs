using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class PedidoProdutoServiceTests
    {
        private CuidaPetContext context = null!;
        private IPedidoProdutoService pedidoProdutoService = null!;

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

            // Dados de teste - Pessoas (Tutores)
            var pessoas = new List<Pessoa>
            {
                new() {
                    Id = 1,
                    Nome = "João Silva",
                    Senha = "senha123",
                    Email = "joao@gmail.com",
                    Telefone = "5527999999999",
                    Cpf = "12345678900",
                    Tipo = "T",
                    Status = "A",
                    Logradouro = "Rua A",
                    Numero = "100",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() {
                    Id = 2,
                    Nome = "Maria Santos",
                    Senha = "senha456",
                    Email = "maria@gmail.com",
                    Telefone = "5527988888888",
                    Cpf = "98765432100",
                    Tipo = "T",
                    Status = "A",
                    Logradouro = "Rua B",
                    Numero = "200",
                    Bairro = "Jardim",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ"
                },
                new() {
                    Id = 3,
                    Nome = "Dr. Carlos",
                    Senha = "senha789",
                    Email = "carlos@gmail.com",
                    Telefone = "5527977777777",
                    Cpf = "11122233344",
                    Tipo = "F",
                    Status = "A",
                    Logradouro = "Rua C",
                    Numero = "300",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                }
            };

            context.AddRange(pessoas);

            // Funcionários
            var funcionarios = new List<Funcionario>
            {
                new() { Id = 1, IdPessoa = 3, Crmv = "12345", IdEstabelecimento = 1 }
            };

            context.AddRange(funcionarios);

            // Estabelecimento
            var estabelecimentos = new List<Estabelecimento>
            {
                new() {
                    Id = 1,
                    Nome = "Pet Shop Central",
                    Cnpj = "12345678000100",
                    Tipo = "P",
                    IdGerente = 3,
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Logradouro = "Rua A",
                    Numero = "123",
                    Bairro = "Centro",
                    Telefone = "11999999999"
                }
            };

            context.AddRange(estabelecimentos);

            // Pets
            var pets = new List<Pet>
            {
                new() {
                    Id = 1,
                    Nome = "Rex",
                    DataNascimento = DateTime.Now.AddYears(-3),
                    Sexo = "M",
                    IdRaca = 1
                }
            };

            context.AddRange(pets);

            // Agendamentos
            var agendamentos = new List<Agendamento>
            {
                new() {
                    Id = 1,
                    DataSolicitacao = DateTime.Now.AddDays(-3),
                    Horario = new TimeSpan(10, 0, 0),
                    Status = "A",
                    IdPet = 1,
                    IdFuncionario = 1,
                    IdTutor = 1
                },
                new() {
                    Id = 2,
                    DataSolicitacao = DateTime.Now.AddDays(-2),
                    Horario = new TimeSpan(14, 0, 0),
                    Status = "A",
                    IdPet = 1,
                    IdFuncionario = 1,
                    IdTutor = 2
                }
            };

            context.AddRange(agendamentos);

            // Categorias
            var categorias = new List<Categoria>
            {
                new() { Id = 1, Nome = "Ração", Descricao = "Alimentos para pets" },
                new() { Id = 2, Nome = "Brinquedos", Descricao = "Brinquedos para pets" }
            };

            context.AddRange(categorias);

            // Produtos
            var produtos = new List<Produto>
            {
                new() {
                    Id = 1,
                    Nome = "Ração Premium",
                    Preco = 150.00m,
                    Status = "D",
                    Descricao = "Ração de alta qualidade",
                    IdCategoria = 1,
                    IdEstabelecimento = 1
                },
                new() {
                    Id = 2,
                    Nome = "Brinquedo Interativo",
                    Preco = 45.00m,
                    Status = "D",
                    Descricao = "Brinquedo que estimula a mente",
                    IdCategoria = 2,
                    IdEstabelecimento = 1
                }
            };

            context.AddRange(produtos);

            // Pedidos
            var pedidos = new List<Pedido>
            {
                new() {
                    Id = 1,
                    Status = "A", // Em andamento
                    RealizadoEm = DateTime.Now.AddDays(-2),
                    IdTutor = 1,
                    IdFuncionario = 1,
                    IdAgendamento = 1
                },
                new() {
                    Id = 2,
                    Status = "F", // Finalizado
                    RealizadoEm = DateTime.Now.AddDays(-1),
                    IdTutor = 2,
                    IdFuncionario = 1,
                    IdAgendamento = 2
                },
                new() {
                    Id = 3,
                    Status = "C", // Cancelado
                    RealizadoEm = DateTime.Now.AddDays(-3),
                    IdTutor = 1,
                    IdFuncionario = 1,
                    IdAgendamento = 1
                }
            };

            context.AddRange(pedidos);

            // Pedido Produtos
            var pedidoProdutos = new List<Pedidoproduto>
            {
                new() {
                    Id = 1,
                    Quantidade = 2,
                    Preco = 150.00m,
                    IdProduto = 1,
                    IdPedido = 1
                },
                new() {
                    Id = 2,
                    Quantidade = 3,
                    Preco = 45.00m,
                    IdProduto = 2,
                    IdPedido = 2
                },
                new() {
                    Id = 3,
                    Quantidade = 1,
                    Preco = 150.00m,
                    IdProduto = 1,
                    IdPedido = 3
                }
            };

            context.AddRange(pedidoProdutos);
            context.SaveChanges();

            pedidoProdutoService = new PedidoProdutoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novoPedidoProdutoId = pedidoProdutoService.Create(new Pedidoproduto()
            {
                Id = 4,
                Quantidade = 1,
                Preco = 45.00m,
                IdProduto = 2,
                IdPedido = 1
            });

            // Assert
            Assert.AreEqual((uint)4, novoPedidoProdutoId);
            var pedidoProduto = pedidoProdutoService.Get(4);
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual(1, pedidoProduto.Quantidade);
            Assert.AreEqual(45.00m, pedidoProduto.Preco);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            pedidoProdutoService.Delete(2);

            // Assert
            var pedidoProduto = pedidoProdutoService.Get(2);
            Assert.IsNull(pedidoProduto);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Act
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoProduto);
            pedidoProduto.Quantidade = 5;
            pedidoProduto.Preco = 140.00m;
            pedidoProdutoService.Edit(pedidoProduto);

            // Assert
            pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual(5, pedidoProduto.Quantidade);
            Assert.AreEqual(140.00m, pedidoProduto.Preco);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var pedidoProduto = pedidoProdutoService.Get(1);

            // Assert
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual((uint)1, pedidoProduto.Id);
            Assert.AreEqual(2, pedidoProduto.Quantidade);
            Assert.AreEqual(150.00m, pedidoProduto.Preco);
            Assert.IsNotNull(pedidoProduto.IdProdutoNavigation);
            Assert.AreEqual("Ração Premium", pedidoProduto.IdProdutoNavigation.Nome);
        }

        [TestMethod()]
        public void GetTest_Inexistente()
        {
            // Act
            var pedidoProduto = pedidoProdutoService.Get(999);

            // Assert
            Assert.IsNull(pedidoProduto);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaPedidoProdutos = pedidoProdutoService.GetAll(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(listaPedidoProdutos, typeof(IEnumerable<Pedidoproduto>));
            Assert.IsNotNull(listaPedidoProdutos);
            // Deve retornar apenas pedidos com status "A" ou "F" (2 registros)
            Assert.AreEqual(2, listaPedidoProdutos.Count());
        }

        [TestMethod()]
        public void GetByStatusTest()
        {
            // Act
            var pedidosAndamento = pedidoProdutoService.GetByStatus("A");

            // Assert
            Assert.IsInstanceOfType(pedidosAndamento, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidosAndamento);
            Assert.AreEqual(1, pedidosAndamento.Count());
            
            var pedido = pedidosAndamento.First();
            Assert.AreEqual("A", pedido.Status);
            Assert.AreEqual("João Silva", pedido.TutorNome);
            Assert.AreEqual(2, pedido.Quantidade);
            Assert.AreEqual(150.00m, pedido.PrecoUnitario);
            Assert.AreEqual(300.00m, pedido.PrecoTotal);
        }

        [TestMethod()]
        public void GetByStatusTest_Finalizado()
        {
            // Act
            var pedidosFinalizados = pedidoProdutoService.GetByStatus("F");

            // Assert
            Assert.IsNotNull(pedidosFinalizados);
            Assert.AreEqual(1, pedidosFinalizados.Count());
            
            var pedido = pedidosFinalizados.First();
            Assert.AreEqual("F", pedido.Status);
            Assert.AreEqual("Maria Santos", pedido.TutorNome);
        }

        [TestMethod()]
        public void GetByTutorTest()
        {
            // Act
            var pedidosTutor1 = pedidoProdutoService.GetByTutor(1);

            // Assert
            Assert.IsInstanceOfType(pedidosTutor1, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidosTutor1);
            // Tutor 1 tem 1 pedido ativo (status A)
            Assert.AreEqual(1, pedidosTutor1.Count());
            
            var pedido = pedidosTutor1.First();
            Assert.AreEqual((uint)1, pedido.TutorId);
            Assert.AreEqual("João Silva", pedido.TutorNome);
        }

        [TestMethod()]
        public void GetByTutorTest_SemPedidos()
        {
            // Act
            var pedidosTutor999 = pedidoProdutoService.GetByTutor(999);

            // Assert
            Assert.IsNotNull(pedidosTutor999);
            Assert.AreEqual(0, pedidosTutor999.Count());
        }

        [TestMethod()]
        public void AlterarStatusTest()
        {
            // Act - Alterar de "A" (Andamento) para "F" (Finalizado)
            pedidoProdutoService.AlterarStatus(1, "F");

            // Assert
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual("F", pedidoProduto.IdPedidoNavigation.Status);
        }

        [TestMethod()]
        public void AlterarStatusTest_PedidoJaFinalizado()
        {
            // Arrange - Pedido 2 já está finalizado
            var pedidoAntes = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("F", pedidoAntes.IdPedidoNavigation.Status);

            // Act - Tentar alterar status de um pedido já finalizado
            pedidoProdutoService.AlterarStatus(2, "C");

            // Assert - Status não deve mudar pois não está mais em "A"
            var pedidoDepois = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("F", pedidoDepois.IdPedidoNavigation.Status);
        }

        [TestMethod()]
        public void GetDetalhesTest()
        {
            // Act
            var detalhes = pedidoProdutoService.GetDetalhes(1);

            // Assert
            Assert.IsNotNull(detalhes);
            Assert.AreEqual((uint)1, detalhes.Id);
            Assert.AreEqual("Ração Premium", detalhes.ProdutoNome);
            Assert.AreEqual(2, detalhes.Quantidade);
            Assert.AreEqual(150.00m, detalhes.PrecoUnitario);
            Assert.AreEqual(300.00m, detalhes.PrecoTotal);
            Assert.AreEqual("João Silva", detalhes.TutorNome);
            Assert.AreEqual("5527999999999", detalhes.TutorTelefone);
            Assert.AreEqual("A", detalhes.Status);
        }

        [TestMethod()]
        public void GetDetalhesTest_Inexistente()
        {
            // Act
            var detalhes = pedidoProdutoService.GetDetalhes(999);

            // Assert
            Assert.IsNull(detalhes);
        }

        [TestMethod()]
        public void GetItensByPedidoIdTest()
        {
            // Act
            var itens = pedidoProdutoService.GetItensByPedidoId(1);

            // Assert
            Assert.IsInstanceOfType(itens, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(itens);
            Assert.AreEqual(1, itens.Count());
            
            var item = itens.First();
            Assert.AreEqual((uint)1, item.PedidoId);
            Assert.AreEqual("Ração Premium", item.ProdutoNome);
        }

        [TestMethod()]
        public void GetItensByPedidoIdTest_PedidoSemItens()
        {
            // Act
            var itens = pedidoProdutoService.GetItensByPedidoId(999);

            // Assert
            Assert.IsNotNull(itens);
            Assert.AreEqual(0, itens.Count());
        }

        [TestMethod()]
        public void RecusarPedidoTest()
        {
            // Arrange
            var pedidoAntes = context.Pedidos.Find((uint)1);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("A", pedidoAntes.Status);

            // Act
            pedidoProdutoService.RecusarPedido(1);

            // Assert
            // Verificar se o item foi deletado
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNull(pedidoProduto);

            // Verificar se o pedido foi marcado como cancelado
            var pedidoDepois = context.Pedidos.Find((uint)1);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("C", pedidoDepois.Status);
        }

        [TestMethod()]
        public void RecusarPedidoTest_PedidoJaFinalizado()
        {
            // Arrange - Pedido 2 já está finalizado
            var pedidoAntes = context.Pedidos.Find((uint)2);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("F", pedidoAntes.Status);

            // Act - Tentar recusar um pedido já finalizado
            pedidoProdutoService.RecusarPedido(2);

            // Assert - Pedido não deve ser alterado
            var pedidoProduto = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoProduto, "Item não deve ser deletado pois pedido não está em andamento");
            
            var pedidoDepois = context.Pedidos.Find((uint)2);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("F", pedidoDepois.Status, "Status não deve mudar");
        }

        [TestMethod()]
        public void GetPedidosAtivosTest()
        {
            // Act
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(pedidos, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidos);
            // Deve retornar apenas pedidos com status "A" ou "F"
            Assert.AreEqual(2, pedidos.Count());
        }

        [TestMethod()]
        public void GetPedidosAtivosTest_ComOrdenacaoPorData()
        {
            // Act - Ordenar por data crescente
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "data", false);

            // Assert
            Assert.IsNotNull(pedidos);
            var lista = pedidos.ToList();
            Assert.AreEqual(2, lista.Count);
            // Primeiro deve ser o mais antigo
            Assert.IsTrue(lista[0].RealizadoEm <= lista[1].RealizadoEm);
        }

        [TestMethod()]
        public void GetPedidosAtivosTest_ComOrdenacaoPorTutor()
        {
            // Act - Ordenar por tutor
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "tutor", false);

            // Assert
            Assert.IsNotNull(pedidos);
            var lista = pedidos.ToList();
            Assert.AreEqual(2, lista.Count);
            // Verificar ordem alfabética
            for (int i = 0; i < lista.Count - 1; i++)
            {
                Assert.IsTrue(string.Compare(lista[i].TutorNome, lista[i + 1].TutorNome, StringComparison.Ordinal) <= 0);
            }
        }

        [TestMethod()]
        public void GetPedidosAtivosTest_ComPaginacao()
        {
            // Act - Pegar apenas 1 item por página
            var pedidosPagina1 = pedidoProdutoService.GetPedidosAtivos(1, 1);
            var pedidosPagina2 = pedidoProdutoService.GetPedidosAtivos(2, 1);

            // Assert
            Assert.AreEqual(1, pedidosPagina1.Count());
            Assert.AreEqual(1, pedidosPagina2.Count());
            // Devem ser diferentes
            Assert.AreNotEqual(pedidosPagina1.First().Id, pedidosPagina2.First().Id);
        }

        [TestMethod()]
        public void GetCountPedidosAtivosTest()
        {
            // Act
            var count = pedidoProdutoService.GetCountPedidosAtivos();

            // Assert
            // Deve contar apenas pedidos com status "A" ou "F" (2 no total)
            Assert.AreEqual(2, count);
        }

        [TestMethod()]
        public void GetCountPedidosAtivosTest_AposRecusar()
        {
            // Arrange
            var countAntes = pedidoProdutoService.GetCountPedidosAtivos();

            // Act - Recusar um pedido ativo
            pedidoProdutoService.RecusarPedido(1);
            var countDepois = pedidoProdutoService.GetCountPedidosAtivos();

            // Assert
            Assert.AreEqual(countAntes - 1, countDepois);
        }

        [TestMethod()]
        public void GetPedidosAtivosTest_OrdenacaoDescendente()
        {
            // Act
            var pedidosDesc = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "data", true);

            // Assert
            Assert.IsNotNull(pedidosDesc);
            var lista = pedidosDesc.ToList();
            Assert.AreEqual(2, lista.Count);
            // Verificar ordem decrescente
            Assert.IsTrue(lista[0].RealizadoEm >= lista[1].RealizadoEm);
        }
    }
}
