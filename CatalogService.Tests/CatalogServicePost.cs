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
        private Mock<ICatalogRepository> _itemServiceMock;

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
            var newItem = new Catalog { Id = Guid.NewGuid(), Title = "Valid Item", Price = 10 };

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
            var invalidItem = new Catalog { Id = Guid.NewGuid(), Title = "", Price = (float)-1.50m };

            // Act
            var result = await _controller.createItem(invalidItem) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid item data", result.Value);
        }

        //Bruger ikke længere
        // [TestMethod]
        // public async Task CreateItem_WithExistingItem_ReturnsConflict()
        // {
        //     // Arrange
        //     var existingItem = new Catalog { Id = Guid.NewGuid(), Title = "Existing Item", Price = 20};
        //     _itemServiceMock.Setup(service => service.getSpecificItem(existingItem.Id)).ReturnsAsync(existingItem);

        //     // Act
        //     var result = await _controller.createItem(existingItem) as ConflictObjectResult;

        //     // Assert
        //     Assert.IsNotNull(result);
        //     Assert.AreEqual("Item already exists", result.Value);
        // }


    }
}