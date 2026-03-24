namespace SurveyBasket.Authentication
{
    public class JwtOptions
    {
        public string Jwt { get; init; }=string.Empty;
        public string Issuer { get; init; }=string.Empty;
        public string Audience { get; init; }=string.Empty;
        public int ExpiryMinutes { get; init; }
    
    }
}
