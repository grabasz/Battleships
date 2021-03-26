using Microsoft.AspNetCore.Mvc;

namespace BattleShipAPI.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("Hello")]
        public ActionResult ISApiWorking()
        {
            return Ok("API is working, connection hub: https://localhost:44365/hub");
        }
    }
}