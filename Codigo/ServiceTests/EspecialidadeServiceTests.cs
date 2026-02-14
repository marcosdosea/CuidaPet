using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class EspecialidadeServiceTests
    {
        private CuidaPetContext context = null!;
        private IEspecialidadeService especialidadeService = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            builder.UseInMemoryDatabase("cuidapetdb");
            var options = builder.Options;

            context = new CuidaPetContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var especialidades = new List<Especialidade>
            {
                new() { Id = 1, Nome = "Cardiologia", Descricao = "Especialista em coração" },
                new() { Id = 2, Nome = "Dermatologia", Descricao = "Especialista em pele" },
                new() { Id = 3, Nome = "Ortopedia", Descricao = "Especialista em ossos" }
            };

            context.AddRange(especialidades);
            context.SaveChanges();

            especialidadeService = new EspecialidadeService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaEspecialidade = new Especialidade()
            {
                Id = 4,
                Nome = "Oftalmologia",
                Descricao = "Especialista em olhos"
            };

            var novoId = especialidadeService.Create(novaEspecialidade);

            Assert.AreEqual((uint)4, novoId);
            Assert.AreEqual(4, especialidadeService.GetAll(page, pageSize).Count());
            var especialidade = especialidadeService.Get(4);
            Assert.IsNotNull(especialidade);
            Assert.AreEqual("Oftalmologia", especialidade.Nome);
            Assert.AreEqual("Especialista em olhos", especialidade.Descricao);
        }

        [TestMethod()]
        public void EditTest()
        {
            var especialidade = especialidadeService.Get(3);
            Assert.IsNotNull(especialidade);
            especialidade.Nome = "Ortopedia Avançada";
            especialidade.Descricao = "Especialista em ossos e articulações";
            especialidadeService.Edit(especialidade);

            var especialidadeEditada = especialidadeService.Get(3);
            Assert.IsNotNull(especialidadeEditada);
            Assert.AreEqual("Ortopedia Avançada", especialidadeEditada.Nome);
            Assert.AreEqual("Especialista em ossos e articulações", especialidadeEditada.Descricao);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            especialidadeService.Delete(2);

            Assert.AreEqual(2, especialidadeService.GetAll(page, pageSize).Count());
            var especialidade = especialidadeService.Get(2);
            Assert.IsNull(especialidade);
        }

        [TestMethod()]
        public void GetTest()
        {
            var especialidade = especialidadeService.Get(1);

            Assert.IsNotNull(especialidade);
            Assert.AreEqual("Cardiologia", especialidade.Nome);
            Assert.AreEqual("Especialista em coração", especialidade.Descricao);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaEspecialidades = especialidadeService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(listaEspecialidades, typeof(IEnumerable<Especialidade>));
            Assert.IsNotNull(listaEspecialidades);
            Assert.AreEqual(3, listaEspecialidades.Count());
            Assert.AreEqual((uint)1, listaEspecialidades.First().Id);
            Assert.AreEqual("Cardiologia", listaEspecialidades.First().Nome);
        }

        [TestMethod()]
        public void GetByNomeTest_DeveRetornarEspecialidade_QuandoNomeExiste()
        {
            // Arrange
            string nomeBusca = "Cardiologia";

            // Act
            var especialidades = especialidadeService.GetByNome(nomeBusca);

            // Assert
            Assert.IsNotNull(especialidades, "A lista de especialidades não deve ser nula");
            Assert.IsInstanceOfType(especialidades, typeof(IEnumerable<Especialidade>), "Deve retornar uma coleção de Especialidade");
            Assert.AreEqual(1, especialidades.Count(), "Deve retornar exatamente uma especialidade");
            
            var especialidade = especialidades.First();
            Assert.AreEqual("Cardiologia", especialidade.Nome, "O nome da especialidade deve ser 'Cardiologia'");
            Assert.AreEqual("Especialista em coração", especialidade.Descricao, "A descrição deve estar correta");
        }

        [TestMethod()]
        public void GetCountTest_DeveRetornarQuantidadeCorreta_DeEspecialidades()
        {
            // Arrange
            int quantidadeEsperada = 3;

            // Act
            int quantidadeAtual = especialidadeService.GetCount();

            // Assert
            Assert.AreEqual(quantidadeEsperada, quantidadeAtual, "A quantidade de especialidades deve ser 3");
            Assert.IsTrue(quantidadeAtual > 0, "Deve haver pelo menos uma especialidade cadastrada");
        }

        [TestMethod()]
        public void CreateAndDeleteTest_DeveManterIntegridade_AposOperacoesCRUD()
        {
            // Arrange
            int quantidadeInicial = especialidadeService.GetCount();
            var novaEspecialidade = new Especialidade()
            {
                Id = 5,
                Nome = "Neurologia",
                Descricao = "Especialista em sistema nervoso"
            };

            // Act - Criar
            var novoId = especialidadeService.Create(novaEspecialidade);
            int quantidadeAposCreate = especialidadeService.GetCount();

            // Assert - Verificar criação
            Assert.AreEqual((uint)5, novoId, "O ID retornado deve ser 5");
            Assert.AreEqual(quantidadeInicial + 1, quantidadeAposCreate, "Quantidade deve aumentar em 1 após criar");

            // Act - Deletar
            especialidadeService.Delete(novoId);
            int quantidadeAposDelete = especialidadeService.GetCount();
            var especialidadeRemovida = especialidadeService.Get(novoId);

            // Assert - Verificar remoção
            Assert.AreEqual(quantidadeInicial, quantidadeAposDelete, "Quantidade deve voltar ao valor inicial após deletar");
            Assert.IsNull(especialidadeRemovida, "A especialidade deletada não deve mais existir");
        }
    }
}
