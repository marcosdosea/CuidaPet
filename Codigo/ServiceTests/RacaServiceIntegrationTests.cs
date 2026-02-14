using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass]
    public class RacaServiceIntegrationTests
    {
        private CuidaPetContext context = null!;
        private IRacaService racaService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            
            builder.UseInMemoryDatabase("RacaServiceIntegrationTests");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var especies = new List<Especie>
            {
                new() { Id = 1, Nome = "Canina" },
                new() { Id = 2, Nome = "Felina" }
            };

            context.AddRange(especies);
            context.SaveChanges();

            racaService = new RacaService(context);
        }

        /// <summary>
        /// Teste de Integração - Fluxo de Dados: Create -> Get
        /// Testa o caminho completo de definição e uso de dados
        /// Def(novaRaca) -> Use(Create) -> Def(id) -> Use(Get) -> Def(racaBuscada)
        /// Verifica persistência e recuperação dos dados no contexto
        /// </summary>
        [TestMethod]
        public void Create_DeveInserirERecuperarRacaComSucesso()
        {
            // Arrange - Define os dados iniciais
            var novaRaca = new Raca
            {
                Id = 10,
                Nome = "Bulldog Francês",
                IdEspecie = 1
            };

            // Act - Executa o fluxo de criação
            var idCriado = racaService.Create(novaRaca);

            // Assert - Verifica o fluxo de uso dos dados
            Assert.AreEqual((uint)10, idCriado);

            // Verifica integração com banco
            var racaBuscada = racaService.Get(idCriado);
            Assert.IsNotNull(racaBuscada);
            Assert.AreEqual("Bulldog Francês", racaBuscada.Nome);
            Assert.AreEqual((uint)1, racaBuscada.IdEspecie);

            // Verifica que o dado foi persistido no contexto
            var racaNoContexto = context.Racas.Find(idCriado);
            Assert.IsNotNull(racaNoContexto);
            Assert.AreEqual(novaRaca.Nome, racaNoContexto.Nome);
        }

        /// <summary>
        /// Teste de Integração - Fluxo de Controle com Decisão
        /// Testa o caminho onde o Delete encontra a raça (if != null TRUE)
        /// Fluxo: Def(raca) -> Use(Delete) -> Decisão(raca != null) -> Use(Remove)
        /// Cobre o cenário de deleção bem-sucedida com verificação de existência
        /// </summary>
        [TestMethod]
        public void Delete_RacaExistente_DeveRemoverDoContextoEPersistir()
        {
            // Arrange - Cria uma raça para ser deletada
            var racaParaDeletar = new Raca
            {
                Id = 20,
                Nome = "Husky Siberiano",
                IdEspecie = 1
            };
            context.Racas.Add(racaParaDeletar);
            context.SaveChanges();

            // Verifica que a raça existe antes da deleção
            var racaAntesDeletar = racaService.Get(20);
            Assert.IsNotNull(racaAntesDeletar);

            // Act - Executa o caminho de deleção (caminho TRUE do if)
            racaService.Delete(20);

            // Assert - Verifica que o fluxo de remoção foi executado
            var racaDepoisDeletar = racaService.Get(20);
            Assert.IsNull(racaDepoisDeletar);

            // Verifica integração - confirma remoção no contexto
            var racaNoContexto = context.Racas.Find((uint)20);
            Assert.IsNull(racaNoContexto);

            // Verifica que outras raças não foram afetadas
            var totalRacas = context.Racas.Count();
            Assert.AreEqual(0, totalRacas);
        }

        /// <summary>
        /// Teste de Integração - Fluxo de Dados Complexo: Create -> Edit -> Get
        /// Testa múltiplos caminhos de definição e uso de variáveis
        /// Fluxo: Def(raca) -> Use(Create) -> Def(id) -> Use(Get) -> Def(racaBuscada) ->
        ///        Redef(racaBuscada.Nome) -> Use(Edit) -> Use(Get) -> Def(racaEditada)
        /// Verifica transações e atualizações no contexto
        /// </summary>
        [TestMethod]
        public void Edit_DeveAtualizarRacaExistenteEPersistirMudancas()
        {
            // Arrange - Cria uma raça inicial
            var racaInicial = new Raca
            {
                Id = 30,
                Nome = "Beagle",
                IdEspecie = 1
            };

            var idCriado = racaService.Create(racaInicial);
            Assert.AreEqual((uint)30, idCriado);

            // Busca a raça criada para edição
            var racaParaEditar = racaService.Get(idCriado);
            Assert.IsNotNull(racaParaEditar);
            Assert.AreEqual("Beagle", racaParaEditar.Nome);

            // Act - Redefine o valor e executa a edição
            racaParaEditar.Nome = "Beagle Inglês";
            racaParaEditar.IdEspecie = 2; // Muda espécie também

            racaService.Edit(racaParaEditar);

            // Assert - Verifica o fluxo completo de atualização
            var racaEditada = racaService.Get(idCriado);
            Assert.IsNotNull(racaEditada);
            Assert.AreEqual("Beagle Inglês", racaEditada.Nome);
            Assert.AreEqual((uint)2, racaEditada.IdEspecie);

            // Verifica integração - confirma persistência no contexto
            context.Entry(racaEditada).Reload(); // Força reload do contexto
            var racaNoContexto = context.Racas.Find(idCriado);
            Assert.IsNotNull(racaNoContexto);
            Assert.AreEqual("Beagle Inglês", racaNoContexto.Nome);
            Assert.AreEqual((uint)2, racaNoContexto.IdEspecie);
        }
    }
}