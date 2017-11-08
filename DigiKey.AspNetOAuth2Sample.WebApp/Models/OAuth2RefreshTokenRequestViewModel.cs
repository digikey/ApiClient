namespace DigiKey.AspNetOAuth2Sample.WebApp.Models
{
    public class OAuth2RefreshTokenRequestViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
        public string RefreshToken { get; set; }
    }
}
