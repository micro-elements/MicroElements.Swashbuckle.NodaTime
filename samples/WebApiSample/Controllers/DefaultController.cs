using Microsoft.AspNetCore.Mvc;

namespace WebApiSample.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true), Route("")]
    public class DefaultController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Redirect("swagger");
    }
}