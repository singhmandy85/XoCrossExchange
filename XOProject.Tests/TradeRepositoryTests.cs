using System;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace XOProject.Tests
{
    public class TradeRepositoryTests
    {
        private ExchangeContext _testDBContext;
        private readonly ITradeRepository _tradeRepository;

        public TradeRepositoryTests()
        {
            _testDBContext = InMemoryContext();
            _tradeRepository = new TradeRepository(_testDBContext);
        }

        private ExchangeContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ExchangeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExchangeContext(options);

            return context;
        }

        [Test]
        public async Task Test_Post_NewTrade()
        {
            //Arrange
            var trade = new Trade
            {
                Symbol = "REL",
                NoOfShares = 12,
                Price = 98.76m,
                PortfolioId = 1,
                Action = "Buy"
            };

            //Act
            await _tradeRepository.InsertAsync(trade);
            var result = await _tradeRepository.Query().ToListAsync();

            // Assert 
            Assert.True(result.Count > 0);
        }

        [Test]
        public async Task Test_Query_Trades()
        {
            //Arrange
            var trade = new Trade
            {
                Symbol = "REL",
                NoOfShares = 12,
                Price = 98.76m,
                PortfolioId = 1,
                Action = "Buy"
            };

            //Act
            _testDBContext.Add(trade);
            await _testDBContext.SaveChangesAsync();
            var result = await _tradeRepository.Query().ToListAsync();

            // Assert 
            Assert.True(result.Count > 0);
        }

        public void Dispose()
        {
            if (_testDBContext != null)
            {
                _testDBContext.Dispose();
            }
        }
    }
}
