using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models;
using Moq;
using Microsoft.Extensions.Logging;
using CatalogService.Repository;
using MongoDB.Bson;
using CatalogService.Exceptions;

namespace CatalogService.Tests.CatalogGetService
{
    [TestClass]
    public class CatalogServiceGetTests
    {
        private CatalogController _controller;
        private static Mock<ICatalogRepository> _itemServiceMock;
        //private List<Catalog> _items;

        [TestInitialize]
        public void Setup()
        {
            _itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            _controller = new CatalogController(loggerMock.Object, _itemServiceMock.Object);
            //_items bruger vi vidst ikke længere, men er i tvivl om vi brugte det under TDD?
            //_items = new List<Catalog> {
            //    new Catalog { Id = Guid.NewGuid(), Name = "Item 1", Price = 10, Description = "Description 1"},
            //    new Catalog { Id = Guid.NewGuid(), Name = "Item 2", Price = 20, Description = "Description 2"},
            //    new Catalog { Id = Guid.NewGuid(), Name = "Item 3", Price = 30, Description = "Description 3"}
            //};
        }

        //Kalder getAll, og checker at det ikke er null - Failer indtil getAll implementeres
        [TestMethod]
        public void GetAll_NotNull()
        {
            
            // Act
            var result = _controller.getAll();

            // Assert
            Assert.IsNotNull(result);
        }
        // Tester om getAll returnerer et OkObjectResult
        [TestMethod]
        public async Task GetAll_ReturnsOkObjectResult()
        {
            
            // Act
            var result = await _controller.getAll(); // Afvent resultatet

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult)); // Kontroller typen af resultatet
        }

        // Tester om getAll returnerer en samling af varer
        [TestMethod]
        public async Task GetAll_ReturnsCollection()
        {
            // Act
            var result = await _controller.getAll() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType<IEnumerable<Catalog>>(result.Value);
        }

        // Tjekker om getAll-metoden på repositoryservicen kaldes en gang
        [TestMethod]
        public void ItCallGetSummariesServiceOnce()
        {
            //_speakerServiceMock = new Mock<ISpeakerService>();
            //var controller = new SpeakerController(_speakerServiceMock.Object);

            _controller.getAll();
            _itemServiceMock.Verify(mock => mock.getAll(), Times.Once);

        }
        //Test for at der er en exception hvis item ikke kan findes - Failer indtil denne fejlmeddelse implementeres
        // [TestMethod]
        // public async Task GivenCatalogNotFoundExceptionThenNotFoundObjectResult()
        // {
        //     var result = await _controller.getSpecificItem(-1) as NotFoundObjectResult;

        //     Assert.IsNotNull(result);
        //     Assert.AreEqual("Item not found", result.Value);
        // }
    }
}