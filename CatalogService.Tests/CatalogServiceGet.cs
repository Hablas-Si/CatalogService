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
        private List<Catalog> _items;
        public CatalogServiceGetTests()
        {
            _itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>(); // Add this line
            _itemServiceMock.Setup(x => x.getSpecificItem(-1))
             .Returns(() => throw new CatalogNotFoundException());
            _controller = new CatalogController(loggerMock.Object, _itemServiceMock.Object); // Update this line
            _items = new List<Catalog> {
                new Catalog { Id = Guid.NewGuid(), Name = "Item 1", Price = 10, Description = "Description 1"},
                new Catalog { Id = Guid.NewGuid(), Name = "Item 2", Price = 20, Description = "Description 2"},
                new Catalog { Id = Guid.NewGuid(), Name = "Item 3", Price = 30, Description = "Description 3"}
            };
        }

        [TestMethod]
        public void ItExist()
        {
            // Arrange
            var itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            var controller = new CatalogController(loggerMock.Object, itemServiceMock.Object);

            // Act
            var result = controller.getAll();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkObjectResult()
        {
            // Arrange
            var itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            var controller = new CatalogController(loggerMock.Object, itemServiceMock.Object);

            // Act
            var result = await controller.getAll(); // Afvent resultatet

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult)); // Kontroller typen af resultatet
        }


        [TestMethod]
        public async Task ReturnCollectionOfItems()
        {
            // Act
            var result = await _controller.getAll() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType<IEnumerable<Catalog>>(result.Value);
        }

        [TestMethod]
        public void ItCallGetSummariesServiceOnce()
        {
            //_speakerServiceMock = new Mock<ISpeakerService>();
            //var controller = new SpeakerController(_speakerServiceMock.Object);

            _controller.getAll();
            _itemServiceMock.Verify(mock => mock.getAll(), Times.Once);

        }

        [TestMethod]
        public async Task GivenCatalogNotFoundExceptionThenNotFoundObjectReuslt()
        {
            var result = await _controller.getSpecificItem(-1) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Catalog Not Found", result.Value);
        }
    }
}