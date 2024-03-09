using ABPTest.Data;
using ABPTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABPTest.Controllers
{
    public class StatisticsController : ControllerBase
    {
        private readonly ExperimentContext _context;

        public StatisticsController(ExperimentContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStatistics()
        {
            var buttonColorStats = await _context.ExperimentResults
                .Where(e => e.ExperimentType == ExperimentType.ButtonColor)
                .GroupBy(e => e.Value)
                .Select(g => new { Color = g.Key, Count = g.Count() })
                .ToListAsync();

            var priceStats = await _context.ExperimentResults
                .Where(e => e.ExperimentType == ExperimentType.Price)
                .GroupBy(e => e.Value)
                .Select(g => new { Price = g.Key, Count = g.Count() })
                .ToListAsync();

            var statistics = new
            {
                ButtonColorStatistics = new { ParticipansCount = buttonColorStats.Sum(x => x.Count), Items = buttonColorStats },
                PriceStatistics = new { ParticipansCount = priceStats.Sum(x => x.Count), Items = priceStats },

            };
            return Ok(statistics);
        }
    }
}
