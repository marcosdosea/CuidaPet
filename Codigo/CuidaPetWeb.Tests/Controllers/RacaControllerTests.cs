using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Controllers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWeb.Tests.Controllers
{
    [TestClass]
    public class RacaControllerTests
    {
        private Mock<IRacaService> mockRacaService = null!;
        private Mock<IMapper> mockMapper = null!;
        private RacaController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockRacaService = new Mock<IRacaService>();
            mockMapper = new Mock<IMapper>();
            controller = new RacaController(mockRacaService.Object, mockMapper.Object);
        }

        /// <summary>
        /// Teste de Cobertura de Decisão - Caminho Verdadeiro (ModelState.IsValid = true)
        /// Testa o fluxo onde os dados são válidos e a criação é bem-sucedida
        /// Cobre: Decisão TRUE no if(ModelState.IsValid)
        /// </summary>
        [TestMethod]
        public void Create_Post_ModelStateValido_DeveRedirecionarParaIndex()
        {
            // Arrange - Define o caminho de execução
            var racaViewModel = new RacaViewModel
            {
                Id = 1,
                Nome = "Golden Retriever",
                IdEspecie = 1
            };

            var raca = new Raca
            {
                Id = 1,
                Nome = "Golden Retriever",
                IdEspecie = 1
            };

            mockMapper.Setup(m => m.Map<Raca>(racaViewModel)).Returns(raca);
            mockRacaService.Setup(s => s.Create(raca)).Returns(1);

            // Act - Executa o caminho TRUE
            var result = controller.Create(racaViewModel) as RedirectToActionResult;

            // Assert - Verifica o resultado do caminho
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockRacaService.Verify(s => s.Create(It.IsAny<Raca>()), Times.Once);
        }

        /// <summary>
        /// Teste de Cobertura de Decisão - Caminho Falso (ModelState.IsValid = false)
        /// Testa o fluxo onde os dados são inválidos e retorna a view com erros
        /// Cobre: Decisão FALSE no if(ModelState.IsValid)
        /// Fluxo de Dados: racaViewModel é usado sem ser mapeado ou persistido
        /// </summary>
        [TestMethod]
        public void Create_Post_ModelStateInvalido_DeveRetornarViewComErros()
        {
            // Arrange - Simula estado inválido
            var racaViewModel = new RacaViewModel
            {
                Nome = "", // Nome vazio - inválido
                IdEspecie = 0
            };

            controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

            // Act - Executa o caminho FALSE
            var result = controller.Create(racaViewModel) as ViewResult;

            // Assert - Verifica que não houve persistência
            Assert.IsNotNull(result);
            Assert.AreEqual(racaViewModel, result.Model);
            mockRacaService.Verify(s => s.Create(It.IsAny<Raca>()), Times.Never);
        }

        /// <summary>
        /// Teste de Cobertura de Decisão - Caminho TRUE no Edit
        /// Testa o fluxo de edição com dados válidos
        /// Cobre: Decisão TRUE no if(ModelState.IsValid) do método Edit
        /// Fluxo de Dados: Def(racaViewModel) -> Use(mapper) -> Def(raca) -> Use(service.Edit)
        /// </summary>
        [TestMethod]
        public void Edit_Post_ModelStateValido_DeveAtualizarERedirecionarParaIndex()
        {
            // Arrange - Define o fluxo de dados completo
            var racaViewModel = new RacaViewModel
            {
                Id = 2,
                Nome = "Pastor Alemão Atualizado",
                IdEspecie = 1
            };

            var racaAtualizada = new Raca
            {
                Id = 2,
                Nome = "Pastor Alemão Atualizado",
                IdEspecie = 1
            };

            mockMapper.Setup(m => m.Map<Raca>(racaViewModel)).Returns(racaAtualizada);
            mockRacaService.Setup(s => s.Edit(racaAtualizada));

            // Act - Executa o caminho de atualização
            var result = controller.Edit(racaViewModel) as RedirectToActionResult;

            // Assert - Verifica o fluxo completo
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockMapper.Verify(m => m.Map<Raca>(racaViewModel), Times.Once);
            mockRacaService.Verify(s => s.Edit(It.Is<Raca>(r => r.Nome == "Pastor Alemão Atualizado")), Times.Once);
        }
    }
}