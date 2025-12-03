using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class DoencaServiceTests
    {
        private CuidaPetContext context = null!;
        private IDoencaService doencaService = null!;

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

            var especies = new List<Especie>
            {
                new() { Id = 1, Nome = "Cachorro" },
                new() { Id = 2, Nome = "Gato" }
            };
            context.AddRange(especies);

            var doencas = new List<Doenca>
            {
                new() { Id = 1, Nome = "Cinomose", IdEspecie = 1 },
                new() { Id = 2, Nome = "Leucemia Felina", IdEspecie = 2 },
                new() { Id = 3, Nome = "Parvovirose", IdEspecie = 1 }
            };

            context.AddRange(doencas);
            context.SaveChanges();

            doencaService = new DoencaService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaDoencaId = doencaService.Create(new Doenca()
            {
                Id = 4,
                Nome = "Dermatofitose",
                IdEspecie = 2
            });

            Assert.AreEqual((uint)4, novaDoencaId);
            Assert.AreEqual(4, doencaService.GetAll(page, pageSize).Count());
            var doenca = doencaService.Get(4);
            Assert.IsNotNull(doenca);
            Assert.AreEqual("Dermatofitose", doenca.Nome);
            Assert.AreEqual((uint)2, doenca.IdEspecie);
        }

        [TestMethod]
        public void Criar_DeveAumentarQuantidade()
        {
            doencaService.Create(new Doenca
            {
                Nome = "Raiva",
                IdEspecie = 1
            });

            Assert.AreEqual(4, doencaService.GetAll(page, pageSize).Count());
        }

        [TestMethod()]
        public void DeleteTest()
        {
            doencaService.Delete(2);

            Assert.AreEqual(2, doencaService.GetAll(page, pageSize).Count());
            var doenca = doencaService.Get(2);
            Assert.IsNull(doenca);
        }

        [TestMethod]
        public void Deletar_IdInexistente_NaoDeveGerarExcecao()
        {
            doencaService.Delete(999);
        }

        [TestMethod()]
        public void EditTest()
        {
            var doenca = doencaService.Get(3);
            Assert.IsNotNull(doenca);
            doenca.Nome = "Parvovirose Canina";
            doenca.IdEspecie = 1;
            doencaService.Edit(doenca);

            doenca = doencaService.Get(3);
            Assert.IsNotNull(doenca);
            Assert.AreEqual("Parvovirose Canina", doenca.Nome);
            Assert.AreEqual((uint)1, doenca.IdEspecie);
        }

        [TestMethod]
        public void Editar_IdInexistente_NaoDeveAlterarQuantidade()
        {
            var doenca = new Doenca
            {
                Id = 999,
                Nome = "Nada",
                IdEspecie = 1
            };

            doencaService.Edit(doenca);

            Assert.AreEqual(3, doencaService.GetAll(page, pageSize).Count());
        }

        [TestMethod()]
        public void GetTest()
        {
            var doenca = doencaService.Get(1);

            Assert.IsNotNull(doenca);
            Assert.AreEqual("Cinomose", doenca.Nome);
            Assert.AreEqual((uint)1, doenca.IdEspecie);
        }

        [TestMethod]
        public void Obter_IdInexistente_DeveRetornarNull()
        {
            var doenca = doencaService.Get(999);

            Assert.IsNull(doenca);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaDoencas = doencaService.GetAll(page, pageSize);

            Assert.IsInstanceOfType(listaDoencas, typeof(IEnumerable<Doenca>));
            Assert.IsNotNull(listaDoencas);
            Assert.AreEqual(3, listaDoencas.Count());
            Assert.AreEqual((uint)1, listaDoencas.First().Id);
            Assert.AreEqual("Cinomose", listaDoencas.First().Nome);
        }

        [TestMethod]
        public void ObterTodos_Paginado_DeveRetornarQuantidadeCorreta()
        {
            var lista = doencaService.GetAll(1, 2);

            Assert.AreEqual(2, lista.Count());
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            var doencas = doencaService.GetByNome("Cinomose");

            Assert.IsInstanceOfType(doencas, typeof(IEnumerable<DoencaDTO>));
            Assert.IsNotNull(doencas);
            Assert.AreEqual(1, doencas.Count());
            var doenca = doencas.First();
            Assert.AreEqual("Cinomose", doenca.Nome);
            Assert.AreEqual((uint)1, doenca.Id);
        }

        [TestMethod]
        public void ObterPorNome_Parcial_DeveRetornarMultiplos()
        {
            context.Doencas.Add(new Doenca
            {
                Nome = "Cinomose Nervosa",
                IdEspecie = 1
            });
            context.SaveChanges();

            var resultado = doencaService.GetByNome("Cino");

            Assert.AreEqual(2, resultado.Count());
        }

        [TestMethod]
        public void ObterPorNome_SemCorrespondencia_DeveRetornarVazio()
        {
            var resultado = doencaService.GetByNome("XYZ");

            Assert.AreEqual(0, resultado.Count());
        }
    }
}
