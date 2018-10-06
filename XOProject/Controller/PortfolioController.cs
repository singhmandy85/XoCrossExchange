using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Controller
{
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(IShareRepository shareRepository,
            ITradeRepository tradeRepository,
            IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portfolioId}")]
        public async Task<IActionResult> GetPortfolioInfo([Required][FromRoute]int portfolioId)
        {
            var portfolios = await _portfolioRepository
                                                .GetAll()
                                                .Where(x => x.Id.Equals(portfolioId))
                                                .ToListAsync();
            
            return Ok(portfolios);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Portfolio value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _portfolioRepository.InsertAsync(value);

            return Created($"Portfolio/{value.Id}", value);
        }
    }
}
