using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShipTracker.Server.Controllers;
using ShipTracker.Server.Database;
using ShipTracker.Server.Models;
using ShipTracker.Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShipTracker.Server.Tests
{
    [TestFixture]
    public class CountriesControllerTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private CountriesController _controller;
        private Mock<DbSet<Country>> _mockCountriesDbSet;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockCountriesDbSet = new Mock<DbSet<Country>>();
            _controller = new CountriesController(_mockDbContext.Object);
        }

        #region GetAllCountries Tests

        [Test]
        public async Task GetAllCountries_ReturnsAllCountriesOrderedByName()
        {
            // Arrange
            var testData = new List<Country>
            {
                new Country { Id = 2, Name = "Brazil" },
                new Country { Id = 1, Name = "Argentina" },
                new Country { Id = 3, Name = "Canada" }
            }.AsQueryable();

            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(testData.Provider);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(testData.Expression);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            _mockDbContext.Setup(db => db.Countries).Returns(_mockCountriesDbSet.Object);

            // Act
            var result = await _controller.GetAllCountries();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var countries = okResult.Value as List<Country>;

            Assert.That(countries, Has.Count.EqualTo(3));
            Assert.That(countries[0].Name, Is.EqualTo("Argentina"));
            Assert.That(countries[1].Name, Is.EqualTo("Brazil"));
            Assert.That(countries[2].Name, Is.EqualTo("Canada"));
        }

        [Test]
        public async Task GetAllCountries_WhenNoCountries_ReturnsEmptyList()
        {
            // Arrange
            var testData = new List<Country>().AsQueryable();

            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(testData.Provider);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(testData.Expression);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            _mockCountriesDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            _mockDbContext.Setup(db => db.Countries).Returns(_mockCountriesDbSet.Object);

            // Act
            var result = await _controller.GetAllCountries();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var countries = okResult.Value as List<Country>;

            Assert.That(countries, Is.Empty);
        }

        #endregion

        #region AddCountry Tests

        [Test]
        public async Task AddCountry_ValidDto_AddsCountryAndReturnsCreated()
        {
            // Arrange
            var newCountryDto = new AddCountryDto { Name = "Test Country" };
            var countries = new List<Country>();

            _mockCountriesDbSet.Setup(m => m.Add(It.IsAny<Country>()))
                .Callback<Country>(c => countries.Add(c));

            _mockDbContext.Setup(db => db.Countries).Returns(_mockCountriesDbSet.Object);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            // Act
            var result = await _controller.AddCountry(newCountryDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedResult>());
            Assert.That(countries, Has.Count.EqualTo(1));
            Assert.That(countries[0].Name, Is.EqualTo("Test Country"));
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AddCountry_NullDto_ReturnsBadRequest()
        {
            // Arrange
            AddCountryDto nullDto = null;

            // Act
            var result = await _controller.AddCountry(nullDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion

        #region UpdateCountry Tests

        [Test]
        public async Task UpdateCountry_ValidIdAndDto_UpdatesCountryAndReturnsOk()
        {
            // Arrange
            var existingCountry = new Country { Id = 1, Name = "Old Name" };
            var updateDto = new UpdateCountryDto { Name = "New Name" };

            _mockDbContext.Setup(db => db.Countries.FindAsync(1))
                .ReturnsAsync(existingCountry);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            // Act
            var result = await _controller.UpdateCountry(1, updateDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var updatedCountry = okResult.Value as Country;

            Assert.That(updatedCountry.Name, Is.EqualTo("New Name"));
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateCountry_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateCountryDto { Name = "New Name" };

            _mockDbContext.Setup(db => db.Countries.FindAsync(1))
                .ReturnsAsync((Country)null);

            // Act
            var result = await _controller.UpdateCountry(1, updateDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateCountry_NullDto_ReturnsBadRequest()
        {
            // Arrange
            UpdateCountryDto nullDto = null;

            // Act
            var result = await _controller.UpdateCountry(1, nullDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion

        #region DeleteCountry Tests

        [Test]
        public async Task DeleteCountry_ValidId_DeletesCountryAndReturnsOk()
        {
            // Arrange
            var existingCountry = new Country { Id = 1, Name = "Test Country" };

            _mockDbContext.Setup(db => db.Countries.FindAsync(1))
                .ReturnsAsync(existingCountry);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            // Act
            var result = await _controller.DeleteCountry(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            _mockDbContext.Verify(db => db.Remove(existingCountry), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteCountry_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockDbContext.Setup(db => db.Countries.FindAsync(1))
                .ReturnsAsync((Country)null);

            // Act
            var result = await _controller.DeleteCountry(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        #endregion
    }
}