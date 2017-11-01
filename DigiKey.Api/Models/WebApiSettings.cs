using System;
using DigiKey.Api.Core.Configuration;

namespace DigiKey.Api.Models
{
    public class WebApiSettings
    {
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
        public String RedirectUri { get; set; }
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }

        public void Save()
        {
            WebApiConfigHelper.Instance().ClientId = ClientId;
            WebApiConfigHelper.Instance().ClientSecret = ClientSecret;
            WebApiConfigHelper.Instance().RedirectUri = RedirectUri;
            WebApiConfigHelper.Instance().AccessToken = AccessToken;
            WebApiConfigHelper.Instance().RefreshToken = RefreshToken;
            WebApiConfigHelper.Instance().ExpirationDateTime = ExpirationDateTime;
            WebApiConfigHelper.Instance().Save();
        }

        public static WebApiSettings CreateFromConfigFile()
        {
            return new WebApiSettings()
            {
                ClientId = WebApiConfigHelper.Instance().ClientId,
                ClientSecret = WebApiConfigHelper.Instance().ClientSecret,
                RedirectUri = WebApiConfigHelper.Instance().RedirectUri,
                AccessToken = WebApiConfigHelper.Instance().AccessToken,
                RefreshToken = WebApiConfigHelper.Instance().RefreshToken,
                ExpirationDateTime = WebApiConfigHelper.Instance().ExpirationDateTime,
            };
        }
    }
}
