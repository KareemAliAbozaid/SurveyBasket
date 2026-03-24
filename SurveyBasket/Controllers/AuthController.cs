using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Authentication;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices,IOptions<JwtOptions> jwtoptions) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        private readonly JwtOptions _jwtoptions = jwtoptions.Value;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authServices.GetTokenAsync(request.Email, request.Password);
            return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_jwtoptions.Audience);
        }
    }
}
