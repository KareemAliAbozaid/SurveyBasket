using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authentication
{
    public class JwtProvider : IJwtProvider
    {
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

            var expiresIn = 30;

            var token = new JwtSecurityToken(
                issuer: "SurveyBasket",
                audience: "SurveyBasketApp",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresIn),
                signingCredentials: signingcredintial
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn);
        }
    }
}
