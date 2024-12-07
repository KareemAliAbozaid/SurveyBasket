using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authServices.GetTokenAsync(request.Email, request.Password);
            return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
        }
    }
}
