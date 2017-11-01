using System;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2.Models;

namespace DigiKey.AspNetOAuth2Sample.WebApp.ViewModels
{
    public class DisplayInformationViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string AccessToken { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string IdToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }

        public static DisplayInformationViewModel CreateFrom(WebApiSettings settings,
                                                             OAuth2AccessToken oAuth2AccessToken)
        {
            var displayInformationViewModel = new DisplayInformationViewModel
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                AuthorizationEndpoint = DigiKeyUriConstants.AuthorizationEndpoint.ToString(),
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint.ToString(),
                AccessToken = oAuth2AccessToken.AccessToken,
                Error = oAuth2AccessToken.Error,
                ErrorDescription = oAuth2AccessToken.ErrorDescription,
                IdToken = oAuth2AccessToken.IdToken,
                RefreshToken = oAuth2AccessToken.RefreshToken,
                TokenType = oAuth2AccessToken.TokenType,
                ExpiresIn = DateTime.Now.AddSeconds(oAuth2AccessToken.ExpiresIn)
            };
            return displayInformationViewModel;
        }
    }
}
