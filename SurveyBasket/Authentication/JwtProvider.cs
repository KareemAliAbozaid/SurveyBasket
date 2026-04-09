using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authentication
{
    public class JwtProvider(IOptions<JwtOptions> jwtoptions) : IJwtProvider
    {
        private readonly JwtOptions _jwtoptions = jwtoptions.Value;

        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new (JwtRegisteredClaimNames.Sub,user.Id),
                new (JwtRegisteredClaimNames.Email,user.Email!),
                new (JwtRegisteredClaimNames.GivenName,user.FirstName),
                new (JwtRegisteredClaimNames.FamilyName,user.LastName),
                new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                ];

            var symmetricsecuretykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB"));
            var signingcredintial = new SigningCredentials(symmetricsecuretykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtoptions.Issuer,
                audience: _jwtoptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtoptions.ExpiryMinutes),
                signingCredentials: signingcredintial
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn:_jwtoptions.ExpiryMinutes*60);
        }
    }
}
