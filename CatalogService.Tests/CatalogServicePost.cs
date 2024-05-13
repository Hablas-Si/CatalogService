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

        [TestMethod]
        public async Task CreateItem_WithNullItem_ReturnsBadRequest()
        {
            // Arrange
            Catalog newItem = null;

            // Act
            var result = await _controller.createItem(newItem) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Item cannot be null", result.Value);
        }

        [TestMethod]
        public async Task CreateItem_WithInvalidItem_ReturnsBadRequest()
        {
            // Arrange
            var invalidItem = new Catalog { Id = Guid.NewGuid(), ItemId = 7, Name = "", Price = -1.0m };

            // Act
            var result = await _controller.createItem(invalidItem) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid item data", result.Value);
        }

        [TestMethod]
        public async Task CreateItem_WithExistingItem_ReturnsConflict()
        {
            // Arrange
            var existingItem = new Catalog { Id = Guid.NewGuid(), ItemId = 123, Name = "Existing Item", Price = 20.0m };
            _itemServiceMock.Setup(service => service.getSpecificItem(existingItem.ItemId)).ReturnsAsync(existingItem);

            // Act
            var result = await _controller.createItem(existingItem) as ConflictObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Item already exists", result.Value);
        }


    }
}