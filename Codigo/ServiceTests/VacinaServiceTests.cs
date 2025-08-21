using Core;
using Core.Service;
using Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class VacinaServiceTests
    {
        private CuidaPetContext context = null!;
        private IVacinaService vacinaService = null!;

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

            var doencas = new List<Doenca>
            {
                new() { Id = 1, Nome = "Raiva", IdEspecie = 1 },
                new() { Id = 2, Nome = "Cinomose", IdEspecie = 2 },
                new() { Id = 3, Nome = "Sarna", IdEspecie = 3 }
            };

            context.AddRange(doencas);

            var especies = new List<Especie>
            {
                new() { Id = 1, Nome = "Cachorro" },
                new() { Id = 2, Nome = "Gato" }
            };

            context.AddRange(especies);

            var vacinas = new List<Vacina>
            {
                new() {
                    Id = 1,
                    Nome = "Antirrábica",
                    PeriodoEmDias = 365,
                    IdDoenca = 1,
                    IdEspecie = 1
                },

                new() {
                    Id = 2,
                    Nome = "Polivalente V10",
                    PeriodoEmDias = 365,
                    IdDoenca = 2,
                    IdEspecie = 1
                },

                new() {
                    Id = 3,
                    Nome = "Quádrupla Felina",
                    PeriodoEmDias = 365,
                    IdDoenca = 3,
                    IdEspecie = 2
                },
            };

            context.AddRange(vacinas);
            context.SaveChanges();

            vacinaService = new VacinaService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novaVacinaId = vacinaService.Create(new() {
                Id = 4,
                Nome = "Giárdia Canina - 1ª Dose",
                PeriodoEmDias = 21,
                IdDoenca = 3,
                IdEspecie = 1
            });

            // Assert
            Assert.AreEqual((uint)4, novaVacinaId);
            Assert.AreEqual(4, vacinaService.GetAll().Count());
            var vacina = vacinaService.Get(4);
            Assert.IsNotNull(vacina);
            Assert.AreEqual("Giárdia Canina - 1ª Dose", vacina.Nome);
            Assert.AreEqual<uint?>(21, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(3, vacina.IdDoenca);
            Assert.AreEqual<uint>(1, vacina.IdEspecie);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            vacinaService.Delete(2);

            // Assert
            Assert.AreEqual(2, vacinaService.GetAll().Count());
            var vacina = vacinaService.Get(2);
            Assert.IsNull(vacina);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Act 
            var vacina = vacinaService.Get(3);
            Assert.IsNotNull(vacina);
            vacina.Nome = "Vacina contra Mixomatose";
            vacina.PeriodoEmDias = 365;
            vacina.IdDoenca = 1;
            vacina.IdEspecie = 2;
            vacinaService.Edit(vacina);

            //Assert
            vacina = vacinaService.Get(3);
            Assert.IsNotNull(vacina);
            Assert.AreEqual("Vacina contra Mixomatose", vacina.Nome);
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.IdDoenca);
            Assert.AreEqual<uint>(2, vacina.IdEspecie);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var vacina = vacinaService.Get(1);

            // Assert
            Assert.IsNotNull(vacina);
            Assert.AreEqual("Antirrábica", vacina.Nome);
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.IdDoenca);
            Assert.AreEqual<uint>(1, vacina.IdEspecie);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaVacinas = vacinaService.GetAll();

            // Assert
            Assert.IsInstanceOfType(listaVacinas, typeof(IEnumerable<Vacina>));
            Assert.IsNotNull(listaVacinas);
            Assert.AreEqual(3, listaVacinas.Count());
            Assert.AreEqual((uint)1, listaVacinas.First().Id);
            Assert.AreEqual("Antirrábica", listaVacinas.First().Nome);
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            //Act
            var vacinas = vacinaService.GetByNome("Antirrábica");

            //Assert
            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDTO>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            var vacina = vacinas.First();
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.Doenca.Id);
            Assert.AreEqual<uint>(1, vacina.Especie.Id);
        }

        [TestMethod()]
        public void GetByDoencaTest()
        {
            //Act
            var vacinas = vacinaService.GetByDoenca(3);

            //Assert
            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDTO>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            Assert.IsTrue(vacinas.Any(p => p.Nome == "Quádrupla Felina"));
        }

        [TestMethod()]
        public void GetByEspecieTest()
        {
            //Act
            var vacinas = vacinaService.GetByEspecie(2);

            //Assert
            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDTO>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            var vacina = vacinas.First();
            Assert.AreEqual("Quádrupla Felina", vacina.Nome);
            Assert.IsNotNull(vacina.Especie);
            Assert.IsNotNull(vacina.Doenca);
        }
    }
}