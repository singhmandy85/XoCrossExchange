using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using FizzWare.NBuilder;
using XOProject.Tests.Controller;
using System.Collections.Generic;

namespace XOProject.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            // Arrange
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Search_Returns_Shares_List()
        {
            // Arrange
            var shareDbSetMock = Builder<HourlyShareRate>.CreateListOfSize(3)
                .All()
                .With(c => c.Rate = Faker.RandomNumber.Next(20, 100))
                .With(c => c.Symbol = "REL")
                .Build().ToAsyncDbSetMock();
            _shareRepositoryMock.Setup(m => m.Query()).Returns(shareDbSetMock.Object);
                         
            // Act
            var result = await _shareController.Get("REL");

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as List<HourlyShareRate>;
            Assert.NotNull(content);

            Assert.AreEqual(3, content.Count);
        }

        [Test]
        public async Task Search_Returns_Shares_BySymbol()
        {
            // Arrange
            var shareDbSetMock = Builder<HourlyShareRate>.CreateListOfSize(3)
                .All()
                .With(c => c.Rate = Faker.RandomNumber.Next(20, 100))
                .With(c => c.Symbol = "REL")
                .Build().ToAsyncDbSetMock();
            _shareRepositoryMock.Setup(m => m.Query()).Returns(shareDbSetMock.Object);

            // Act
            var result = await _shareController.GetLatestPrice("REL");

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = Convert.ToDecimal(objectResult.Value);

            Assert.NotNull(content);
        }

        [Test]
        public async Task Put_ShouldUpdateLastPrice()
        {
            // Arrange
            var shareDbSetMock = Builder<HourlyShareRate>.CreateListOfSize(3)
                .All()
                .With(c => c.Rate = Faker.RandomNumber.Next(20, 100))
                .With(c => c.Symbol = "REL")
                .Build().ToAsyncDbSetMock();
            _shareRepositoryMock.Setup(m => m.Query()).Returns(shareDbSetMock.Object);

            // Act
            await _shareController.UpdateLastPrice("REL");

            // Assert 
        }

    }
}
