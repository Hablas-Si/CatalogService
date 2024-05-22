using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models;
using Moq;
using Microsoft.Extensions.Logging;
using CatalogService.Repositories;
using MongoDB.Bson;
using CatalogService.Exceptions;

namespace CatalogService.Tests.CatalogGetService
{
    [TestClass]
    public class CatalogServiceDeleteTests
    {
        private CatalogController _controller;
        private Mock<ICatalogRepository> _itemServiceMock;

        private Mock<IVaultRepository> _vaultServiceMock;
        

        [TestInitialize]
        public void Setup()
        {
            _itemServiceMock = new Mock<ICatalogRepository>();
            _vaultServiceMock = new Mock<IVaultRepository>();
            var loggerMock = new Mock<ILogger<CatalogController>>();
            _controller = new CatalogController(loggerMock.Object, _itemServiceMock.Object, _vaultServiceMock.Object);
        }

        [TestMethod]
        public async Task DeleteCatalog_WithValidItemId_ReturnsOkResult()
        {
            // Arrange
            Guid ID = Guid.NewGuid(); // Valid item ID
            _itemServiceMock.Setup(service => service.getSpecificItem(ID)).ReturnsAsync(new Catalog {});

            // Act
            var result = await _controller.DeleteCatalog(ID) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Catalog and associated ExtendedCatalog deleted successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteCatalog_WithInvalidItemId_ReturnsNotFoundResult()
        {
            // Arrange
            Guid ID = Guid.NewGuid(); // Valid item ID
            _itemServiceMock.Setup(service => service.getSpecificItem(ID)).ReturnsAsync((Catalog)null);

            // Act
            var result = await _controller.DeleteCatalog(ID) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Catalog not found", result.Value);
        }



    }
}