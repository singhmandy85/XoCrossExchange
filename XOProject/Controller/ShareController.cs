using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Controller
{
    [Route("api/[controller]")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository { get; set; }

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpPut("{symbol}")]
        public async Task<IActionResult> UpdateLastPrice([Required][FromRoute]string symbol)
        {
            var share = await _shareRepository.Query()
                                            .Where(x => x.Symbol.Equals(symbol))
                                            .OrderByDescending(x => x.Rate)
                                            .FirstOrDefaultAsync();

            share.Rate =+ 10;
            await _shareRepository.UpdateAsync(share);
            return Ok();
        }
        
        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([Required][FromRoute]string symbol)
        {
            var shares = await _shareRepository.Query()
                                                .Where(x => x.Symbol.Equals(symbol))
                                                .ToListAsync();

            if (shares.Count >= 0)
            {
                return Ok(shares);
            }
            else
                return BadRequest();
        }


        [HttpGet("{symbol}/Latest")]
        public async Task<IActionResult> GetLatestPrice([Required][FromRoute]string symbol)
        {
            var share = await _shareRepository.Query()
                                            .Where(x => x.Symbol.Equals(symbol))
                                            .FirstOrDefaultAsync();
            return Ok(share?.Rate);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HourlyShareRate value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _shareRepository.InsertAsync(value);

            return Created($"Share/{value.Id}", value);
        }
        
    }
}
