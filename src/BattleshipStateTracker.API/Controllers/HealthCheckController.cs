using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.API.Controllers
{
    [Route("")]
    public class HealthCheckController : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            return new ContentResult
            {
                Content = "Battle State Tracker API v1 running successfully",
                StatusCode = HttpStatusCode.OK.GetHashCode()
            };
        }
    }
}