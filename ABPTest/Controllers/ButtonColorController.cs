using ABPTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABPTest.Controllers
{
    [Route("api/button_color")]
    [ApiController]
    public class ButtonColorController : ControllerBase
    {
        private readonly IExperimentService _experimentService;
        
        public ButtonColorController(IExperimentService experimentService)
        {
            _experimentService = experimentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetButtonColor([FromQuery(Name = "device-token")] string deviceToken)
        {
            try
            {
                var result = await _experimentService.GetButtonColor(deviceToken);
                return Ok(new { key = "button_color", value = result });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }    
    }
}
