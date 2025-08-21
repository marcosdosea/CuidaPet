using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class RacaServiceTests
    {
        private CuidaPetContext context = null!;
        private IRacaService racaService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<CuidaPetContext>();
            builder.UseInMemoryDatabase("cuidapetdb");
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

            var racas = new List<Raca>
            {
                new() { Id = 1, Nome = "Labrador", IdEspecie = 1 },
                new() { Id = 2, Nome = "Persa", IdEspecie = 2 },
                new() { Id = 3, Nome = "Poodle", IdEspecie = 1 }
            };

            context.AddRange(racas);
            context.SaveChanges();

            racaService = new RacaService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaRaca = new Raca()
            {
                Id = 4,
                Nome = "Siamês",
                IdEspecie = 2
            };

            var novoId = racaService.Create(novaRaca);

            Assert.AreEqual((uint)4, novoId);
            Assert.AreEqual(4, racaService.GetAll().Count());
            var raca = racaService.Get(4);
            Assert.IsNotNull(raca);
            Assert.AreEqual("Siamês", raca.Nome);
            Assert.AreEqual((uint)2, raca.IdEspecie);
        }

        [TestMethod()]
        public void EditTest()
        {
            var raca = racaService.Get(3);
            Assert.IsNotNull(raca);
            raca.Nome = "Poodle Toy";
            racaService.Edit(raca);

            var racaEditada = racaService.Get(3);
            Assert.IsNotNull(racaEditada);
            Assert.AreEqual("Poodle Toy", racaEditada.Nome);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            racaService.Delete(2);

            Assert.AreEqual(2, racaService.GetAll().Count());
            var raca = racaService.Get(2);
            Assert.IsNull(raca);
        }

        [TestMethod()]
        public void GetTest()
        {
            var raca = racaService.Get(1);

            Assert.IsNotNull(raca);
            Assert.AreEqual("Labrador", raca.Nome);
            Assert.AreEqual((uint)1, raca.IdEspecie);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaRacas = racaService.GetAll();

            Assert.IsInstanceOfType(listaRacas, typeof(IEnumerable<Raca>));
            Assert.IsNotNull(listaRacas);
            Assert.AreEqual(3, listaRacas.Count());
            Assert.AreEqual((uint)1, listaRacas.First().Id);
            Assert.AreEqual("Labrador", listaRacas.First().Nome);
        }
    }
}