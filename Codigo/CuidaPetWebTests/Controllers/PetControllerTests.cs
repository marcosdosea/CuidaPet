using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Mappers;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CuidaPetWeb.Controllers.Tests
{
    [TestClass()]
    public class PetControllerTests
    {
        private static PetController controller = null!;
        private readonly int page = 1;
        private readonly int pageSize = 10;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IPetService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PetProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll(page, pageSize))
                .Returns(GetTestPets());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetPet());
            mockService.Setup(service => service.Edit(It.IsAny<Pet>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Pet>()))
                .Returns(4);
            mockService.Setup(service => service.Delete(It.IsAny<uint>()))
                .Verifiable();

            controller = new PetController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<PetViewModel>));

            var listaPets = (IEnumerable<PetViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, listaPets.Count());
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PetViewModel));
            PetViewModel petModel = (PetViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Rex", petModel.Nome);
            Assert.AreEqual("M", petModel.Sexo);
            Assert.AreEqual(1u, petModel.IdRaca);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            var result = controller.Create(GetNewPet());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewPet());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PetViewModel));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PetViewModel));
            PetViewModel petModel = (PetViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Rex", petModel.Nome);
            Assert.AreEqual("M", petModel.Sexo);
            Assert.AreEqual(1u, petModel.IdRaca);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(GetTargetPetModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PetViewModel));
            PetViewModel petModel = (PetViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Rex", petModel.Nome);
            Assert.AreEqual("M", petModel.Sexo);
            Assert.AreEqual(1u, petModel.IdRaca);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetPetModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private PetViewModel GetNewPet()
        {
            return new PetViewModel
            {
                Id = 4,
                Nome = "Toby",
                Sexo = "M",
                DataNascimento = new DateTime(2022, 1, 1),
                IdRaca = 2
            };
        }

        private static Pet GetTargetPet()
        {
            return new Pet
            {
                Id = 1,
                Nome = "Rex",
                Sexo = "M",
                DataNascimento = new DateTime(2020, 5, 10),
                IdRaca = 1
            };
        }

        private PetViewModel GetTargetPetModel()
        {
            return new PetViewModel
            {
                Id = 1,
                Nome = "Rex",
                Sexo = "M",
                DataNascimento = new DateTime(2020, 5, 10),
                IdRaca = 1
            };
        }

        private static IEnumerable<Pet> GetTestPets()
        {
            return new List<Pet>
            {
                new Pet { Id = 1, Nome = "Rex", Sexo = "M", DataNascimento = new DateTime(2020, 5, 10), IdRaca = 1 },
                new Pet { Id = 2, Nome = "Luna", Sexo = "F", DataNascimento = new DateTime(2021, 3, 15), IdRaca = 2 },
                new Pet { Id = 3, Nome = "Thor", Sexo = "M", DataNascimento = new DateTime(2019, 8, 20), IdRaca = 1 }
            };
        }
    }
}