using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class EspecieServiceTests
    {
        private CuidaPetContext context = null!;
        private IEspecieService especieService = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

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

            var especies = new List<Especie>
            {
                new() { Id = 1, Nome = "Cachorro"},
                new() { Id = 2, Nome = "Gato"},
                new() { Id = 3, Nome = "Peixe"}
            };

            context.AddRange(especies);
            context.SaveChanges();

            especieService = new EspecieService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novaEspecie = especieService.Create(new Especie()
            {
                Id = 4,
                Nome = "Ave"
            });

            // Assert
            Assert.AreEqual((uint)4, novaEspecie);
            var especie = especieService.Get(4);
            Assert.IsNotNull(especie);
            Assert.IsTrue(4 == especie.Id);
            Assert.AreEqual("Ave", especie.Nome);
        }

        [TestMethod]
        public void Create_DeveAumentarQuantidade()
        {
            // Act
            especieService.Create(new Especie { Nome = "Ave" });

            // Assert
            Assert.AreEqual(4, especieService.GetCount());
        }


        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            especieService.Delete(2);

            // Assert
            Assert.AreEqual(2, especieService.GetAll(page, pageSize).Count());
            var especie = especieService.Get(2);
            Assert.IsNull(especie);
        }

        [TestMethod]
        public void Delete_IdInexistente_ShouldNotThrow()
        {
            especieService.Delete(999);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Act 
            var especie = especieService.Get(3);
            Assert.IsNotNull(especie);
            especie.Nome = "Roedor";
            especieService.Edit(especie);

            //Assert
            especie = especieService.Get(3);
            Assert.IsNotNull(especie);
            Assert.AreEqual("Roedor", especie.Nome);
            Assert.IsTrue(3 == especie.Id);
        }

        [TestMethod]
        public void Edit_ItemInexistente_ShouldNotThrow()
        {
            // Arrange
            var especie = new Especie { Id = 999, Nome = "Nada" };

            // Act
            especieService.Edit(especie);

            // Assert
            Assert.AreEqual(3, especieService.GetCount());
        }


        [TestMethod()]
        public void GetTest()
        {
            // Act
            var especie = especieService.Get(1);

            // Assert
            Assert.IsNotNull(especie);
            Assert.AreEqual("Cachorro", especie.Nome);
            Assert.AreEqual((uint)1, especie.Id);
        }

        [TestMethod]
        public void Get_IdInexistente_RetornaNulo()
        {
            var especie = especieService.Get(999);

            Assert.IsNull(especie);
        }


        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaEspecies = especieService.GetAll(page, pageSize);

            // Assert
            Assert.IsInstanceOfType(listaEspecies, typeof(IEnumerable<Especie>));
            Assert.IsNotNull(listaEspecies);
            Assert.AreEqual(3, listaEspecies.Count());
            Assert.AreEqual((uint)1, listaEspecies.First().Id);
            Assert.AreEqual("Cachorro", listaEspecies.First().Nome);
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            //Act
            var especies = especieService.GetByNome("Cachorro");

            //Assert
            Assert.IsInstanceOfType(especies, typeof(IEnumerable<EspecieDto>));
            Assert.IsNotNull(especies);
            Assert.AreEqual(1, especies.Count());
            var produto = especies.First();
            Assert.AreEqual("Cachorro", produto.Nome);
            Assert.AreEqual((uint)1, produto.Id);
        }

        [TestMethod]
        public void GetAll_Paginado_DeveRetornarQuantidadeCorreta()
        {
            var lista = especieService.GetAll(1, 2);

            Assert.AreEqual(2, lista.Count());
        }

        [TestMethod]
        public void GetByNome_Parcial_DeveRetornarMultiplos()
        {
            context.Especies.Add(new Especie { Nome = "Cachorrão" });
            context.SaveChanges();

            var resultado = especieService.GetByNome("Cacho");

            Assert.AreEqual(2, resultado.Count());
        }
    }
}