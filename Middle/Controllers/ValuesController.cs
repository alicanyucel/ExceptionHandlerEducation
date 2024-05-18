using Microsoft.AspNetCore.Mvc;

namespace Middle.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetException()
        {
            try
            {
                throw new WeCannotFindYourUserException();
            }
            catch(Exception ex) 
            {

                return StatusCode(500,ex.Message);

            }
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "api is working" });
        }
    }
}
