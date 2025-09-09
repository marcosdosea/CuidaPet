using Core;
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
    }
}
