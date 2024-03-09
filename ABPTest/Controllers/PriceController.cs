using Microsoft.AspNetCore.Mvc;
using ABPTest.Services;

namespace ABPTest.Controllers
{
    [Route("api/price")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IExperimentService _experimentService;

        public PriceController(IExperimentService experimentService)
        {
            _experimentService = experimentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice([FromQuery(Name = "device-token")] string deviceToken)
        {
            try
            {
                var result = await _experimentService.GetPrice(deviceToken);
                return Ok(new { key = "price", value = result });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }    
    }
}
