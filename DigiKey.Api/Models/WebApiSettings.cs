using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using DigiKey.Api.Core.Configuration;
using DigiKey.Api.OAuth2.Models;

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

        public void UpdateAndSave(OAuth2AccessToken oAuth2AccessToken)
        {
            AccessToken = oAuth2AccessToken.AccessToken;
            RefreshToken = oAuth2AccessToken.RefreshToken;
            ExpirationDateTime = DateTime.Now.AddSeconds(oAuth2AccessToken.ExpiresIn);
            Save();
        }
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"   ------------ [ WebApiSettings ] -------------");
            sb.AppendLine(@"     ClientId            : " + ClientId);
            sb.AppendLine(@"     ClientSecret        : " + ClientSecret);
            sb.AppendLine(@"     RedirectUri         : " + RedirectUri);
            sb.AppendLine(@"     AccessToken         : " + AccessToken);
            sb.AppendLine(@"     RefreshToken        : " + RefreshToken);
            sb.AppendLine(@"     ExpirationDateTime  : " + ExpirationDateTime);
            sb.AppendLine(@"   ---------------------------------------------");

            return sb.ToString();
        }
    }
}
