using Microsoft.AspNetCore.Mvc;

namespace Middle.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                throw new Exception("we can not find your user");
            }
            catch(Exception ex) 
            {

                return StatusCode(500,ex.Message);

            }
        }
    }
}
public class WeCannotFindYourUserException : Exception
{
    public WeCannotFindYourUserException():base("we cannot find your user") 
    {
        
    }
}