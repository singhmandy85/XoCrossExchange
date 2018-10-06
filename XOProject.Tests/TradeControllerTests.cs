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
    public class TradeControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IPortfolioRepository> _portfolioRepository = new Mock<IPortfolioRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object,
                _tradeRepositoryMock.Object,
                _portfolioRepository.Object);
        }        

        [Test]
        public async Task Test_GetAllTradings_List()
        {
            // Arrange
            var tradeDbSetMock = Builder<Trade>.CreateListOfSize(3)
                .All()
                .With(c => c.NoOfShares = Faker.RandomNumber.Next(20, 100))
                .With(c => c.Price = Faker.RandomNumber.Next(20, 100))
                .With(c => c.PortfolioId = 1)
                .With(c => c.Symbol = "REL")
                .With(c => c.Action = "Buy")
                .Build().ToAsyncDbSetMock();
            _tradeRepositoryMock.Setup(m => m.Query()).Returns(tradeDbSetMock.Object);
                         
            // Act
            var result = await _tradeController.GetAllTradings(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as List<Trade>;
            Assert.NotNull(content);

            Assert.AreEqual(3, content.Count);
        }

        [Test]
        public async Task Test_GetAnalysis_List()
        {
            // Arrange
            var tradeDbSetMock = Builder<Trade>.CreateListOfSize(3)
                .All()
                .With(c => c.NoOfShares = Faker.RandomNumber.Next(20, 100))
                .With(c => c.Price = Faker.RandomNumber.Next(20, 100))
                .With(c => c.PortfolioId = 1)
                .With(c => c.Symbol = "REL")
                .With(c => c.Action = "Buy")
                .Build().ToAsyncDbSetMock();
            _tradeRepositoryMock.Setup(m => m.Query()).Returns(tradeDbSetMock.Object);

            // Act
            var result = await _tradeController.GetAnalysis("REL");

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as List<TradeAnalysis>;
            Assert.NotNull(content);

            Assert.AreEqual(1, content.Count);
        }
    }
}
