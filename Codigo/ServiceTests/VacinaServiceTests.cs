using Core;
using Core.Context;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class VacinaServiceTests
    {
        private CuidaPetContext context = null!;
        private IVacinaService vacinaService = null!;
        private int page = 1;
        private int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
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
                new()
                {
                    Id = 1,
                    Nome = "Antirrábica",
                    PeriodoEmDias = 365,
                    IdDoenca = 1,
                    IdEspecie = 1
                },

                new()
                {
                    Id = 2,
                    Nome = "Polivalente V10",
                    PeriodoEmDias = 365,
                    IdDoenca = 2,
                    IdEspecie = 1
                },

                new()
                {
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
            var novaVacinaId = vacinaService.Create(new()
            {
                Id = 4,
                Nome = "Giárdia Canina - 1ª Dose",
                PeriodoEmDias = 21,
                IdDoenca = 3,
                IdEspecie = 1
            });

            Assert.AreEqual((uint)4, novaVacinaId);
            Assert.AreEqual(4, vacinaService.GetAll(page, pageSize).Count());
            var vacina = vacinaService.Get(4);
            Assert.IsNotNull(vacina);
            Assert.AreEqual("Giárdia Canina - 1ª Dose", vacina.Nome);
            Assert.AreEqual<uint?>(21, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(3, vacina.IdDoenca);
            Assert.AreEqual<uint>(1, vacina.IdEspecie);
        }

        [TestMethod]
        public void Criar_DeveAumentarQuantidade()
        {
            vacinaService.Create(new Vacina
            {
                Nome = "Vacina Teste",
                PeriodoEmDias = 30,
                IdDoenca = 1,
                IdEspecie = 1
            });

            Assert.AreEqual(4, vacinaService.GetAll(page, pageSize).Count());
        }

        [TestMethod()]
        public void DeleteTest()
        {
            vacinaService.Delete(2);

            Assert.AreEqual(2, vacinaService.GetAll(page, pageSize).Count());
            var vacina = vacinaService.Get(2);
            Assert.IsNull(vacina);
        }

        [TestMethod]
        public void Deletar_IdInexistente_NaoDeveGerarExcecao()
        {
            vacinaService.Delete(999);
        }

        [TestMethod()]
        public void EditTest()
        {
            var vacina = vacinaService.Get(3);
            Assert.IsNotNull(vacina);

            vacina.Nome = "Vacina contra Mixomatose";
            vacina.PeriodoEmDias = 365;
            vacina.IdDoenca = 1;
            vacina.IdEspecie = 2;

            vacinaService.Edit(vacina);

            vacina = vacinaService.Get(3);
            Assert.IsNotNull(vacina);
            Assert.AreEqual("Vacina contra Mixomatose", vacina.Nome);
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.IdDoenca);
            Assert.AreEqual<uint>(2, vacina.IdEspecie);
        }

        [TestMethod]
        public void Editar_IdInexistente_NaoDeveAlterarQuantidade()
        {
            var vacina = new Vacina
            {
                Id = 999,
                Nome = "Inexistente",
                PeriodoEmDias = 100,
                IdDoenca = 1,
                IdEspecie = 1
            };

            vacinaService.Edit(vacina);

            Assert.AreEqual(3, vacinaService.GetAll(page, pageSize).Count());
        }

        [TestMethod()]
        public void GetTest()
        {
            var vacina = vacinaService.Get(1);

            Assert.IsNotNull(vacina);
            Assert.AreEqual("Antirrábica", vacina.Nome);
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.IdDoenca);
            Assert.AreEqual<uint>(1, vacina.IdEspecie);
        }

        [TestMethod]
        public void Obter_IdInexistente_DeveRetornarNull()
        {
            var vacina = vacinaService.Get(999);

            Assert.IsNull(vacina);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaVacinas = vacinaService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(listaVacinas, typeof(IEnumerable<Vacina>));
            Assert.IsNotNull(listaVacinas);
            Assert.AreEqual(3, listaVacinas.Count());
            Assert.AreEqual((uint)1, listaVacinas.First().Id);
            Assert.AreEqual("Antirrábica", listaVacinas.First().Nome);
        }

        [TestMethod]
        public void ObterTodos_Paginado_DeveRetornarQuantidadeCorreta()
        {
            var lista = vacinaService.GetAll(1, 2);

            Assert.AreEqual(2, lista.Count());
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            var vacinas = vacinaService.GetByNome("Antirrábica");

            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDto>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            var vacina = vacinas.First();
            Assert.AreEqual<uint?>(365, vacina.PeriodoEmDias);
            Assert.AreEqual<uint>(1, vacina.Doenca.Id);
            Assert.AreEqual<uint>(1, vacina.Especie.Id);
        }

        [TestMethod]
        public void ObterPorNome_Parcial_DeveRetornarMultiplos()
        {
            context.Vacinas.Add(new Vacina
            {
                Nome = "Antirrábica Premium",
                PeriodoEmDias = 365,
                IdDoenca = 1,
                IdEspecie = 1
            });
            context.SaveChanges();

            var resultado = vacinaService.GetByNome("Antir");

            Assert.AreEqual(2, resultado.Count());
        }

        [TestMethod]
        public void ObterPorNome_SemCorrespondencia_DeveRetornarVazio()
        {
            var resultado = vacinaService.GetByNome("XYZ");

            Assert.AreEqual(0, resultado.Count());
        }

        [TestMethod()]
        public void GetByDoencaTest()
        {
            var vacinas = vacinaService.GetByDoenca(3);

            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDto>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            Assert.IsTrue(vacinas.Any(p => p.Nome == "Quádrupla Felina"));
        }

        [TestMethod]
        public void ObterPorDoenca_Inexistente_DeveRetornarVazio()
        {
            var vacinas = vacinaService.GetByDoenca(999);

            Assert.AreEqual(0, vacinas.Count());
        }

        [TestMethod()]
        public void GetByEspecieTest()
        {
            var vacinas = vacinaService.GetByEspecie(2);

            Assert.IsInstanceOfType(vacinas, typeof(IEnumerable<VacinaDto>));
            Assert.IsNotNull(vacinas);
            Assert.AreEqual(1, vacinas.Count());
            var vacina = vacinas.First();
            Assert.AreEqual("Quádrupla Felina", vacina.Nome);
            Assert.IsNotNull(vacina.Especie);
            Assert.IsNotNull(vacina.Doenca);
        }

        [TestMethod]
        public void ObterPorEspecie_Inexistente_DeveRetornarVazio()
        {
            var vacinas = vacinaService.GetByEspecie(999);

            Assert.AreEqual(0, vacinas.Count());
        }
    }
}
