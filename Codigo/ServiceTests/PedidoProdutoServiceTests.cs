using Core;
using Core.Context;
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

        #region Testes CRUD Básicos

        [TestMethod()]
        public void Create_QuandoDadosValidos_DevePersistirNoBancoDeDados()
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
        public void Delete_QuandoRegistroExiste_DeveRemoverDoBancoDeDados()
        {
            // Act
            pedidoProdutoService.Delete(2);

            // Assert
            var pedidoProduto = pedidoProdutoService.Get(2);
            Assert.IsNull(pedidoProduto);
        }

        [TestMethod()]
        public void Edit_QuandoDadosAlterados_DeveAtualizarRegistroNoBanco()
        {
            // Arrange
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoProduto);
            
            // Act
            pedidoProduto.Quantidade = 5;
            pedidoProduto.Preco = 140.00m;
            pedidoProdutoService.Edit(pedidoProduto);

            // Assert - Validar persistência dos dados
            var pedidoAtualizado = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoAtualizado);
            Assert.AreEqual(5, pedidoAtualizado.Quantidade);
            Assert.AreEqual(140.00m, pedidoAtualizado.Preco);
        }

        [TestMethod()]
        public void Get_QuandoRegistroExiste_DeveRetornarComNavegacaoCarregada()
        {
            // Act
            var pedidoProduto = pedidoProdutoService.Get(1);

            // Assert - Validar integridade dos dados e navegação
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual((uint)1, pedidoProduto.Id);
            Assert.AreEqual(2, pedidoProduto.Quantidade);
            Assert.AreEqual(150.00m, pedidoProduto.Preco);
            Assert.IsNotNull(pedidoProduto.IdProdutoNavigation);
            Assert.AreEqual("Ração Premium", pedidoProduto.IdProdutoNavigation.Nome);
            Assert.IsNotNull(pedidoProduto.IdPedidoNavigation);
            Assert.IsNotNull(pedidoProduto.IdPedidoNavigation.IdTutorNavigation);
        }

        [TestMethod()]
        public void Get_QuandoRegistroNaoExiste_DeveRetornarNull()
        {
            // Act
            var pedidoProduto = pedidoProdutoService.Get(999);

            // Assert
            Assert.IsNull(pedidoProduto);
        }

        #endregion

        #region Testes de Filtros e Consultas

        [TestMethod()]
        public void GetAll_QuandoChamado_DeveRetornarApenasPedidosAtivosEFinalizados()
        {
            // Act
            var listaPedidoProdutos = pedidoProdutoService.GetAll(page, pageSize);

            // Assert - Validar regra de negócio de filtro por status
            Assert.IsInstanceOfType(listaPedidoProdutos, typeof(IEnumerable<Pedidoproduto>));
            Assert.IsNotNull(listaPedidoProdutos);
            Assert.AreEqual(2, listaPedidoProdutos.Count());
            
            foreach (var item in listaPedidoProdutos)
            {
                Assert.IsTrue(item.IdPedidoNavigation.Status == "A" || item.IdPedidoNavigation.Status == "F",
                    "Deve retornar apenas pedidos com status A ou F");
            }
        }

        [TestMethod()]
        public void GetByStatus_QuandoStatusAndamento_DeveRetornarDadosCompletosMapeados()
        {
            // Act
            var pedidosAndamento = pedidoProdutoService.GetByStatus("A");

            // Assert - Validar transformação de dados e cálculos
            Assert.IsInstanceOfType(pedidosAndamento, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidosAndamento);
            Assert.AreEqual(1, pedidosAndamento.Count());
            
            var pedido = pedidosAndamento.First();
            Assert.AreEqual("A", pedido.Status);
            Assert.AreEqual("João Silva", pedido.TutorNome);
            Assert.AreEqual(2, pedido.Quantidade);
            Assert.AreEqual(150.00m, pedido.PrecoUnitario);
            Assert.AreEqual(300.00m, pedido.PrecoTotal, "PrecoTotal deve ser PrecoUnitario * Quantidade");
        }

        [TestMethod()]
        public void GetByStatus_QuandoStatusFinalizado_DeveRetornarApenasFinalizados()
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
        public void GetByStatus_QuandoStatusCancelado_DeveRetornarApenasCancelados()
        {
            // Act
            var pedidosCancelados = pedidoProdutoService.GetByStatus("C");

            // Assert - Validar filtro de status
            Assert.IsNotNull(pedidosCancelados);
            Assert.AreEqual(1, pedidosCancelados.Count());
            
            var pedido = pedidosCancelados.First();
            Assert.AreEqual("C", pedido.Status);
            Assert.AreEqual((uint)3, pedido.Id);
        }

        [TestMethod()]
        public void GetByTutor_QuandoTutorPossuiPedidos_DeveRetornarApenasPedidosAtivos()
        {
            // Act
            var pedidosTutor1 = pedidoProdutoService.GetByTutor(1);

            // Assert - Validar filtro por tutor e status
            Assert.IsInstanceOfType(pedidosTutor1, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidosTutor1);
            Assert.AreEqual(1, pedidosTutor1.Count(), "Tutor 1 tem apenas 1 pedido ativo");
            
            var pedido = pedidosTutor1.First();
            Assert.AreEqual((uint)1, pedido.TutorId);
            Assert.AreEqual("João Silva", pedido.TutorNome);
            Assert.AreEqual("5527999999999", pedido.TutorTelefone);
        }

        [TestMethod()]
        public void GetByTutor_QuandoTutorNaoPossuiPedidos_DeveRetornarListaVazia()
        {
            // Act
            var pedidosTutor999 = pedidoProdutoService.GetByTutor(999);

            // Assert
            Assert.IsNotNull(pedidosTutor999);
            Assert.AreEqual(0, pedidosTutor999.Count());
        }

        #endregion

        #region Testes de Regras de Negócio

        [TestMethod()]
        public void AlterarStatus_QuandoPedidoEmAndamento_DeveAlterarStatusParaFinalizado()
        {
            // Act
            pedidoProdutoService.AlterarStatus(1, "F");

            // Assert - Validar alteração de estado
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNotNull(pedidoProduto);
            Assert.AreEqual("F", pedidoProduto.IdPedidoNavigation.Status);
        }

        [TestMethod()]
        public void AlterarStatus_QuandoPedidoJaFinalizado_NaoDeveAlterarStatus()
        {
            // Arrange
            var pedidoAntes = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("F", pedidoAntes.IdPedidoNavigation.Status);

            // Act - Tentar alterar status de um pedido já finalizado
            pedidoProdutoService.AlterarStatus(2, "C");

            // Assert - Validar regra: apenas pedidos "A" podem ter status alterado
            var pedidoDepois = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("F", pedidoDepois.IdPedidoNavigation.Status, "Status não deve mudar pois pedido não está em andamento");
        }

        [TestMethod()]
        public void AlterarStatus_QuandoPedidoCancelado_NaoDeveAlterarStatus()
        {
            // Arrange
            var pedidoAntes = pedidoProdutoService.Get(3);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("C", pedidoAntes.IdPedidoNavigation.Status);

            // Act
            pedidoProdutoService.AlterarStatus(3, "F");

            // Assert - Validar integridade da regra de negócio
            var pedidoDepois = pedidoProdutoService.Get(3);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("C", pedidoDepois.IdPedidoNavigation.Status, "Pedido cancelado não pode ter status alterado");
        }

        [TestMethod()]
        public void RecusarPedido_QuandoPedidoEmAndamento_DeveDeletarItensECancelarPedido()
        {
            // Arrange
            var pedidoAntes = context.Pedidos.Find((uint)1);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("A", pedidoAntes.Status);

            // Act
            pedidoProdutoService.RecusarPedido(1);

            // Assert - Validar transformação de dados: deleção e mudança de status
            var pedidoProduto = pedidoProdutoService.Get(1);
            Assert.IsNull(pedidoProduto, "Item deve ser deletado");

            var pedidoDepois = context.Pedidos.Find((uint)1);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("C", pedidoDepois.Status, "Pedido deve ser marcado como cancelado");
        }

        [TestMethod()]
        public void RecusarPedido_QuandoPedidoJaFinalizado_NaoDeveAlterarDados()
        {
            // Arrange
            var pedidoAntes = context.Pedidos.Find((uint)2);
            Assert.IsNotNull(pedidoAntes);
            Assert.AreEqual("F", pedidoAntes.Status);

            // Act
            pedidoProdutoService.RecusarPedido(2);

            // Assert - Validar regra: apenas pedidos "A" podem ser recusados
            var pedidoProduto = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoProduto, "Item não deve ser deletado pois pedido não está em andamento");
            
            var pedidoDepois = context.Pedidos.Find((uint)2);
            Assert.IsNotNull(pedidoDepois);
            Assert.AreEqual("F", pedidoDepois.Status, "Status não deve mudar");
        }

        #endregion

        #region Testes de Transformação de Dados (DTO)

        [TestMethod()]
        public void GetDetalhes_QuandoRegistroExiste_DeveRetornarDtoComCalculoCorreto()
        {
            // Act
            var detalhes = pedidoProdutoService.GetDetalhes(1);

            // Assert - Validar mapeamento e cálculo de PrecoTotal
            Assert.IsNotNull(detalhes);
            Assert.AreEqual((uint)1, detalhes.Id);
            Assert.AreEqual("Ração Premium", detalhes.ProdutoNome);
            Assert.AreEqual(2, detalhes.Quantidade);
            Assert.AreEqual(150.00m, detalhes.PrecoUnitario);
            Assert.AreEqual(300.00m, detalhes.PrecoTotal, "PrecoTotal = PrecoUnitario * Quantidade");
            Assert.AreEqual("João Silva", detalhes.TutorNome);
            Assert.AreEqual("5527999999999", detalhes.TutorTelefone);
            Assert.AreEqual("A", detalhes.Status);
        }

        [TestMethod()]
        public void GetDetalhes_QuandoRegistroNaoExiste_DeveRetornarNull()
        {
            // Act
            var detalhes = pedidoProdutoService.GetDetalhes(999);

            // Assert
            Assert.IsNull(detalhes);
        }

        [TestMethod()]
        public void GetItensByPedidoId_QuandoPedidoPossuiItens_DeveRetornarDtosComDadosCompletos()
        {
            // Act
            var itens = pedidoProdutoService.GetItensByPedidoId(1);

            // Assert - Validar transformação e integridade dos dados
            Assert.IsInstanceOfType(itens, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(itens);
            Assert.AreEqual(1, itens.Count());
            
            var item = itens.First();
            Assert.AreEqual((uint)1, item.PedidoId);
            Assert.AreEqual("Ração Premium", item.ProdutoNome);
            Assert.AreEqual(2, item.Quantidade);
            Assert.AreEqual(150.00m, item.PrecoUnitario);
            Assert.AreEqual(300.00m, item.PrecoTotal);
            Assert.AreEqual("João Silva", item.TutorNome);
        }

        [TestMethod()]
        public void GetItensByPedidoId_QuandoPedidoNaoPossuiItens_DeveRetornarListaVazia()
        {
            // Act
            var itens = pedidoProdutoService.GetItensByPedidoId(999);

            // Assert
            Assert.IsNotNull(itens);
            Assert.AreEqual(0, itens.Count());
        }

        #endregion

        #region Testes de Ordenação e Paginação

        [TestMethod()]
        public void GetPedidosAtivos_QuandoChamado_DeveRetornarApenasPedidosAtivosEFinalizados()
        {
            // Act
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize);

            // Assert - Validar filtro de status
            Assert.IsInstanceOfType(pedidos, typeof(IEnumerable<PedidoProdutoDto>));
            Assert.IsNotNull(pedidos);
            Assert.AreEqual(2, pedidos.Count());
            
            foreach (var pedido in pedidos)
            {
                Assert.IsTrue(pedido.Status == "A" || pedido.Status == "F",
                    "Deve retornar apenas pedidos com status A ou F");
            }
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoOrdenadoPorDataCrescente_DeveRetornarEmOrdemCorreta()
        {
            // Act
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "data", false);

            // Assert - Validar ordenação de dados
            Assert.IsNotNull(pedidos);
            var lista = pedidos.ToList();
            Assert.AreEqual(2, lista.Count);
            Assert.IsTrue(lista[0].RealizadoEm <= lista[1].RealizadoEm, "Primeiro item deve ser o mais antigo");
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoOrdenadoPorDataDecrescente_DeveRetornarEmOrdemCorreta()
        {
            // Act
            var pedidosDesc = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "data", true);

            // Assert - Validar ordenação decrescente
            Assert.IsNotNull(pedidosDesc);
            var lista = pedidosDesc.ToList();
            Assert.AreEqual(2, lista.Count);
            Assert.IsTrue(lista[0].RealizadoEm >= lista[1].RealizadoEm, "Primeiro item deve ser o mais recente");
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoOrdenadoPorTutor_DeveRetornarEmOrdemAlfabetica()
        {
            // Act
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize, "tutor", false);

            // Assert - Validar ordenação alfabética
            Assert.IsNotNull(pedidos);
            var lista = pedidos.ToList();
            Assert.AreEqual(2, lista.Count);
            
            for (int i = 0; i < lista.Count - 1; i++)
            {
                Assert.IsTrue(string.Compare(lista[i].TutorNome, lista[i + 1].TutorNome, StringComparison.Ordinal) <= 0,
                    "Nomes devem estar em ordem alfabética");
            }
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoPaginado_DeveRetornarPaginasCorretas()
        {
            // Act
            var pedidosPagina1 = pedidoProdutoService.GetPedidosAtivos(1, 1);
            var pedidosPagina2 = pedidoProdutoService.GetPedidosAtivos(2, 1);

            // Assert - Validar paginação
            Assert.AreEqual(1, pedidosPagina1.Count());
            Assert.AreEqual(1, pedidosPagina2.Count());
            Assert.AreNotEqual(pedidosPagina1.First().Id, pedidosPagina2.First().Id, "Páginas devem conter registros diferentes");
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoPaginaExcedeLimite_DeveRetornarListaVazia()
        {
            // Act
            var pedidosPagina99 = pedidoProdutoService.GetPedidosAtivos(99, pageSize);

            // Assert
            Assert.IsNotNull(pedidosPagina99);
            Assert.AreEqual(0, pedidosPagina99.Count(), "Página inexistente deve retornar lista vazia");
        }

        #endregion

        #region Testes de Contagem

        [TestMethod()]
        public void GetCountPedidosAtivos_QuandoChamado_DeveContarApenasPedidosAtivosEFinalizados()
        {
            // Act
            var count = pedidoProdutoService.GetCountPedidosAtivos();

            // Assert - Validar cálculo de contagem
            Assert.AreEqual(2, count, "Deve contar apenas pedidos com status A ou F");
        }

        [TestMethod()]
        public void GetCountPedidosAtivos_AposRecusarPedido_DeveDecrementarContagem()
        {
            // Arrange
            var countAntes = pedidoProdutoService.GetCountPedidosAtivos();

            // Act
            pedidoProdutoService.RecusarPedido(1);
            var countDepois = pedidoProdutoService.GetCountPedidosAtivos();

            // Assert - Validar integridade da contagem após alteração
            Assert.AreEqual(countAntes - 1, countDepois, "Contagem deve diminuir após recusar pedido");
        }

        [TestMethod()]
        public void GetCountPedidosAtivos_AposFinalizarPedido_DeveManterContagem()
        {
            // Arrange
            var countAntes = pedidoProdutoService.GetCountPedidosAtivos();

            // Act - Finalizar pedido em andamento
            pedidoProdutoService.AlterarStatus(1, "F");
            var countDepois = pedidoProdutoService.GetCountPedidosAtivos();

            // Assert - Pedido continua ativo, apenas mudou de A para F
            Assert.AreEqual(countAntes, countDepois, "Contagem deve permanecer a mesma pois F também é considerado ativo");
        }

        #endregion

        #region Testes de Casos de Borda e Validação

        [TestMethod()]
        public void GetByStatus_QuandoStatusInexistente_DeveRetornarListaVazia()
        {
            // Act
            var pedidos = pedidoProdutoService.GetByStatus("X");

            // Assert
            Assert.IsNotNull(pedidos);
            Assert.AreEqual(0, pedidos.Count());
        }

        [TestMethod()]
        public void GetPedidosAtivos_QuandoNaoHaPedidos_DeveRetornarListaVazia()
        {
            // Arrange - Remover todos os pedidos ativos
            pedidoProdutoService.RecusarPedido(1);
            context.Pedidos.Find((uint)2)!.Status = "C";
            context.SaveChanges();

            // Act
            var pedidos = pedidoProdutoService.GetPedidosAtivos(page, pageSize);

            // Assert
            Assert.IsNotNull(pedidos);
            Assert.AreEqual(0, pedidos.Count());
        }

        [TestMethod()]
        public void GetDetalhes_DeveCalcularPrecoTotalCorretamente()
        {
            // Arrange
            var pedidoProduto = pedidoProdutoService.Get(2);
            Assert.IsNotNull(pedidoProduto);
            int quantidade = pedidoProduto.Quantidade;
            decimal precoUnitario = pedidoProduto.Preco;

            // Act
            var detalhes = pedidoProdutoService.GetDetalhes(2);

            // Assert - Validar fórmula de cálculo
            Assert.IsNotNull(detalhes);
            Assert.AreEqual(quantidade * precoUnitario, detalhes.PrecoTotal, 
                "PrecoTotal deve ser igual a Quantidade * PrecoUnitario");
        }

        #endregion
    }
}
