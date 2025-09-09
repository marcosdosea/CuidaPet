using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass]
    public class NotificacaoServiceTests
    {
        private CuidaPetContext context = null!;
        private INotificacaoService notificacaoService = null!;
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

            // Dados de teste
            var pessoas = new List<Pessoa>
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
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP"
                },
                new() {
                    Id = 2,
                    Nome = "Maria Santos",
                    Senha = "senha456",
                    Email = "maria@gmail.com",
                    Telefone = "11888888888",
                    Cpf = "98765432100",
                    Tipo = "T",
                    Status = "A",
                    Logradouro = "Rua B",
                    Numero = "200",
                    Bairro = "Jardim",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ"
                }
            };

            var notificacoes = new List<Notificacao>
            {
                new() {
                    Id = 1,
                    Titulo = "Bem-vindo",
                    Descricao = "Seja bem-vindo ao CuidaPet!",
                    DataEnvio = DateTime.Now.AddDays(-2)
                },
                new() {
                    Id = 2,
                    Titulo = "Consulta Agendada",
                    Descricao = "Sua consulta foi agendada com sucesso.",
                    DataEnvio = DateTime.Now.AddDays(-1)
                }
            };

            context.AddRange(pessoas);
            context.AddRange(notificacoes);
            context.SaveChanges();

            // Relacionamentos pessoa-notificação
            var pessoaNotificacoes = new List<Pessoanotificacao>
            {
                new() {
                    Id = 1,
                    IdPessoa = 1,
                    IdNotificacao = 1,
                    StatusLida = 0
                },
                new() {
                    Id = 2,
                    IdPessoa = 1,
                    IdNotificacao = 2,
                    StatusLida = 1
                }
            };

            context.AddRange(pessoaNotificacoes);
            context.SaveChanges();

            notificacaoService = new NotificacaoService(context);
        }

        [TestMethod]
        public void CreateTest()
        {
            var novaNotificacao = new Notificacao
            {
                Titulo = "Nova Notificação",
                Descricao = "Descrição da nova notificação",
                DataEnvio = DateTime.Now
            };

            var notificacaoId = notificacaoService.Create(novaNotificacao);

            Assert.AreEqual((uint)3, notificacaoId);
            var notificacao = notificacaoService.Get(notificacaoId);
            Assert.IsNotNull(notificacao);
            Assert.AreEqual("Nova Notificação", notificacao.Titulo);
            Assert.AreEqual("Descrição da nova notificação", notificacao.Descricao);
        }

        [TestMethod]
        public void EditTest()
        {
            var notificacao = notificacaoService.Get(1);
            Assert.IsNotNull(notificacao);

            notificacao.Titulo = "Título Alterado";
            notificacao.Descricao = "Descrição Alterada";

            notificacaoService.Edit(notificacao);

            var notificacaoEditada = notificacaoService.Get(1);
            Assert.IsNotNull(notificacaoEditada);
            Assert.AreEqual("Título Alterado", notificacaoEditada.Titulo);
            Assert.AreEqual("Descrição Alterada", notificacaoEditada.Descricao);
        }

        [TestMethod]
        public void DeleteTest()
        {
            notificacaoService.Delete(2);

            var notificacaoRemovida = notificacaoService.Get(2);
            Assert.IsNull(notificacaoRemovida);
        }

        [TestMethod]
        public void GetTest()
        {
            var notificacao = notificacaoService.Get(1);

            Assert.IsNotNull(notificacao);
            Assert.AreEqual("Bem-vindo", notificacao.Titulo);
            Assert.AreEqual("Seja bem-vindo ao CuidaPet!", notificacao.Descricao);
            Assert.IsInstanceOfType(notificacao, typeof(Notificacao));
        }

        [TestMethod]
        public void GetTest_NotificacaoInexistente()
        {
            var notificacao = notificacaoService.Get(999);

            Assert.IsNull(notificacao);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var notificacoes = notificacaoService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(notificacoes, typeof(IEnumerable<Notificacao>));
            Assert.IsNotNull(notificacoes);
            Assert.AreEqual(2, notificacoes.Count());

            var primeiraNotificacao = notificacoes.First();
            Assert.AreEqual("Consulta Agendada", primeiraNotificacao.Titulo); // Ordenado por data desc
        }

        [TestMethod]
        public void GetCountTest()
        {
            var quantidade = notificacaoService.GetCount();

            Assert.AreEqual(2, quantidade);
        }

        [TestMethod]
        public void EnviarNotificacaoTest()
        {
            var titulo = "Teste Envio";
            var mensagem = "Mensagem de teste";
            uint idPessoa = 2;

            notificacaoService.EnviarNotificacao(titulo, mensagem, idPessoa);

            // Verificar se a notificação foi criada
            var todasNotificacoes = notificacaoService.GetAll(1, 100);
            var novaNotificacao = todasNotificacoes.FirstOrDefault(n => n.Titulo == titulo);
            Assert.IsNotNull(novaNotificacao);
            Assert.AreEqual(mensagem, novaNotificacao.Descricao);

            // Verificar se o relacionamento pessoa-notificação foi criado
            var notificacoesPessoa = notificacaoService.ObterNotificacoesPorPessoa(idPessoa);
            Assert.IsTrue(notificacoesPessoa.Any(n => n.Titulo == titulo));
        }

        [TestMethod]
        public void ObterNotificacoesPorPessoaTest()
        {
            var notificacoesPessoa1 = notificacaoService.ObterNotificacoesPorPessoa(1);

            Assert.IsNotNull(notificacoesPessoa1);
            Assert.AreEqual(2, notificacoesPessoa1.Count);
            Assert.IsTrue(notificacoesPessoa1.Any(n => n.Titulo == "Bem-vindo"));
            Assert.IsTrue(notificacoesPessoa1.Any(n => n.Titulo == "Consulta Agendada"));
        }

        [TestMethod]
        public void ObterNotificacoesPorPessoaTest_PessoaSemNotificacoes()
        {
            var notificacoesPessoa2 = notificacaoService.ObterNotificacoesPorPessoa(2);

            Assert.IsNotNull(notificacoesPessoa2);
            Assert.AreEqual(0, notificacoesPessoa2.Count);
        }

        [TestMethod]
        public void MarcarComoLidaTest()
        {
            // Verificar status inicial (não lida)
            var pessoaNotificacao = context.Pessoanotificacaos
                .FirstOrDefault(pn => pn.IdNotificacao == 1 && pn.IdPessoa == 1);
            Assert.IsNotNull(pessoaNotificacao);
            Assert.AreEqual((sbyte)0, pessoaNotificacao.StatusLida);

            // Marcar como lida
            notificacaoService.MarcarComoLida(1, 1);

            // Verificar se foi marcada como lida
            var pessoaNotificacaoAtualizada = context.Pessoanotificacaos
                .FirstOrDefault(pn => pn.IdNotificacao == 1 && pn.IdPessoa == 1);
            Assert.IsNotNull(pessoaNotificacaoAtualizada);
            Assert.AreEqual((sbyte)1, pessoaNotificacaoAtualizada.StatusLida);
        }

        [TestMethod]
        public void MarcarComoLidaTest_NotificacaoInexistente()
        {
            // Tentar marcar como lida uma notificação inexistente
            notificacaoService.MarcarComoLida(999, 1);

            // Não deve gerar erro e não deve alterar nada
            var count = context.Pessoanotificacaos.Count();
            Assert.AreEqual(2, count); // Deve manter os 2 registros originais
        }

        [TestMethod]
        public void NotificarAprovacaoPedidoTest()
        {
            // Criar um pedido de teste
            var pedido = new Pedido
            {
                Id = 1,
                IdTutor = 1,
                Status = "A",
                RealizadoEm = DateTime.Now
            };
            context.Pedidos.Add(pedido);
            context.SaveChanges();

            var countInicial = notificacaoService.GetCount();

            notificacaoService.NotificarAprovacaoPedido(1);

            var countFinal = notificacaoService.GetCount();
            Assert.AreEqual(countInicial + 1, countFinal);

            var notificacoesPessoa = notificacaoService.ObterNotificacoesPorPessoa(1);
            Assert.IsTrue(notificacoesPessoa.Any(n => n.Titulo == "Pedido Aprovado"));
        }

        [TestMethod]
        public void NotificarAprovacaoPedidoTest_PedidoInexistente()
        {
            var countInicial = notificacaoService.GetCount();

            notificacaoService.NotificarAprovacaoPedido(999);

            var countFinal = notificacaoService.GetCount();
            Assert.AreEqual(countInicial, countFinal); // Não deve criar notificação
        }

        [TestMethod]
        public void VerificarVacinasVencendoTest()
        {
            // Este teste seria mais complexo pois requer dados de Pets, Vacinas, etc.
            // Por simplicidade, vamos testar que o método executa sem erro
            notificacaoService.VerificarVacinasVencendo();

            // Se chegou até aqui, o método executou sem exceções
            Assert.IsTrue(true);
        }
    }
}