using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopmentController : ControllerBase
    {
        private readonly IOS _ios;
        public DevelopmentController(IOS ios)
        {
            _ios = ios;
        }

        [HttpGet]
        public IActionResult Run()
        {
            var message = _ios.RunApp();
            return Ok(message);
        }
    }
}
