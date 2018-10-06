using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Controller
{
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }
        
        [HttpGet("{portfolioId}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portfolioId)
        {
            var trade = await _tradeRepository.Query()
                                                .Where(x => x.PortfolioId.Equals(portfolioId))
                                                .ToListAsync();
            return Ok(trade);
        }
        
        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, average and Sum of all the trades that happened for that share. 
        /// Group statistics individually for all BUY trades and SELL trades separately.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([Required][FromRoute]string symbol)
        { 
            var trades = await _tradeRepository.Query()
                                                .Where(x => x.Symbol.Equals(symbol))
                                                .ToListAsync();
            var list = (from tr in trades
                          group tr by tr.Action into dayGrp
                          select new TradeAnalysis()
                          {
                              Action = dayGrp.Key,
                              Sum = dayGrp.Sum(x => x.Price),
                              Average = Math.Round(dayGrp.Average(x => x.Price), 2),
                              Minimum = dayGrp.Min(x => x.Price),
                              Maximum = dayGrp.Max(x => x.Price)
                          }).ToList();
            
            return Ok(list);
        }
    }
}
