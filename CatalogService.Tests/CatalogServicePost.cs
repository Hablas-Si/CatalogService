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
    public class CatalogServicePostTests
    {
        private CatalogController _controller;
        private static Mock<ICatalogRepository> _itemServiceMock;
        private List<Catalog> _items;

        [TestInitialize]
        public void Setup()
        {
            _itemServiceMock = new Mock<ICatalogRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            _controller = new CatalogController(loggerMock.Object, _itemServiceMock.Object);
        }

        [TestMethod]
        public async Task CreateItem_WithValidItem_ReturnsOkResult()
        {
            // Arrange
            var newItem = new Catalog { Id = Guid.NewGuid(), ItemId = 123, Name = "Valid Item", Price = 10.5m };

            // Act
            var result = await _controller.createItem(newItem) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newItem, result.Value);
        }

        

       
    }
}