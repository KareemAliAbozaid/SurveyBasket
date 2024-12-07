
using SurveyBasket.Authentication;

namespace SurveyBasket.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;
            var validatPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validatPassword)
                return null;

            var (token,expiresIn)=_jwtProvider.GenerateToken(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token,expiresIn*60);

        }
    }
}
