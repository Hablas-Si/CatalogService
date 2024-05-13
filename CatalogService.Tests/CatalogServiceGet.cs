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
            var loggerMock = new Mock<ILogger<CatalogController>>();
            _itemServiceMock.Setup(x => x.getItem(-1))
                .Returns(() => throw new CatalogNotFoundException());
            _controller = new CatalogController(loggerMock.Object, _itemServiceMock.Object);
            _items = new List<Catalog> {
                new Catalog { Id = Guid.NewGuid(), Name = "Item 1", Price = 10 },
                new Catalog { Id = Guid.NewGuid(), Name = "Item 2", Price = 20 },
                new Catalog { Id = Guid.NewGuid(), Name = "Item 3", Price = 30 }
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
        public void GetAll_ReturnsOkObjectResult()
        {
            // Arrange
            var itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            var controller = new CatalogController(loggerMock.Object, itemServiceMock.Object);

            // Act
            var result = controller.getAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void ReturnCollectionOfItems()
        {
            // Act
            var result = _controller.getAll() as OkObjectResult;

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
            _itemServiceMock.Verify(mock=>mock.getAll(), Times.Once );

        }

          [TestMethod]    
        public  void GivenCatalogNotFoundExceptionThenNotFoundObjectReuslt()
        {

            //var controller = new SpeakerController(_speakerServiceMock.Object);

            var result = _controller.getItem(-1) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Catalog Not Found", result.Value);
        }
    }
}