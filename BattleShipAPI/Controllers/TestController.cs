using Microsoft.AspNetCore.Mvc;

namespace BattleShipAPI.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("Hello")]
        public ActionResult Index()
        {
            return Ok("HelloWorld");
        }
    }
}