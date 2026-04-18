
using SurveyBasket.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly int _jwtRefreshTokenExpiryDays = 14;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;
            var validatPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validatPassword)
                return null;

            var (token,expiresIn)=_jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration=DateTime.UtcNow.AddDays(_jwtRefreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token,expiresIn*60,refreshToken,refreshTokenExpiration);

        }
        private static string GenerateRefreshToken()
        { 
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId=_jwtProvider.ValidateTken(token);
            if (userId is null)
                return null;

            var user=await _userManager.FindByIdAsync(userId);
            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn= DateTime.UtcNow;

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtRefreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn * 60, newRefreshToken, refreshTokenExpiration);

        }
    }
}
