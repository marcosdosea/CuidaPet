using Core;
using Core.Context;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class PetServiceTests
    {
        private CuidaPetContext context = null!;
        private IPetService petService = null!;

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

            var racas = new List<Raca>
            {
                new() { Id = 1, Nome = "Labrador", IdEspecie = 1 },
                new() { Id = 2, Nome = "Persa", IdEspecie = 2 }
            };
            context.AddRange(racas);

            var pets = new List<Pet>
            {
                new() { Id = 1, Nome = "Rex", Sexo = "M", DataNascimento = new DateTime(2020, 5, 10), IdRaca = 1 },
                new() { Id = 2, Nome = "Luna", Sexo = "F", DataNascimento = new DateTime(2021, 3, 15), IdRaca = 2 },
                new() { Id = 3, Nome = "Thor", Sexo = "M", DataNascimento = new DateTime(2019, 8, 20), IdRaca = 1 }
            };
            context.AddRange(pets);
            context.SaveChanges();

            petService = new PetService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novoPet = new Pet()
            {
                Id = 4,
                Nome = "Toby",
                Sexo = "M",
                DataNascimento = new DateTime(2022, 1, 1),
                IdRaca = 2
            };

            var novoId = petService.Create(novoPet);

            Assert.AreEqual((uint)4, novoId);
            Assert.AreEqual(4, petService.GetAll(page, pageSize).Count());
            var pet = petService.Get(4);
            Assert.IsNotNull(pet);
            Assert.AreEqual("Toby", pet.Nome);
            Assert.AreEqual("M", pet.Sexo);
            Assert.AreEqual((uint)2, pet.IdRaca);
        }

        [TestMethod()]
        public void EditTest()
        {
            var pet = petService.Get(3);
            Assert.IsNotNull(pet);
            pet.Nome = "Thor Jr.";
            petService.Edit(pet);

            var petEditado = petService.Get(3);
            Assert.IsNotNull(petEditado);
            Assert.AreEqual("Thor Jr.", petEditado.Nome);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            petService.Delete(2);

            Assert.AreEqual(2, petService.GetAll(page, pageSize).Count());
            var pet = petService.Get(2);
            Assert.IsNull(pet);
        }

        [TestMethod()]
        public void GetTest()
        {
            var pet = petService.Get(1);

            Assert.IsNotNull(pet);
            Assert.AreEqual("Rex", pet.Nome);
            Assert.AreEqual("M", pet.Sexo);
            Assert.AreEqual((uint)1, pet.IdRaca);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaPets = petService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(listaPets, typeof(IEnumerable<Pet>));
            Assert.IsNotNull(listaPets);
            Assert.AreEqual(3, listaPets.Count());
            Assert.AreEqual((uint)1, listaPets.First().Id);
            Assert.AreEqual("Rex", listaPets.First().Nome);
        }
    }
}