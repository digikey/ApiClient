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

        public static DisplayInformationViewModel CreateFrom(DigiKeyAppCredentials appCredentials,
                                                             OAuth2AccessTokenResponse oAuth2AccessTokenResponse)
        {
            var displayInformationViewModel = new DisplayInformationViewModel
            {
                ClientId = appCredentials.ClientId,
                ClientSecret = appCredentials.ClientSecret,
                AuthorizationEndpoint = DigiKeyUriConstants.AuthorizationEndpoint.ToString(),
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint.ToString(),
                AccessToken = oAuth2AccessTokenResponse.AccessToken,
                Error = oAuth2AccessTokenResponse.Error,
                ErrorDescription = oAuth2AccessTokenResponse.ErrorDescription,
                IdToken = oAuth2AccessTokenResponse.IdToken,
                RefreshToken = oAuth2AccessTokenResponse.RefreshToken,
                TokenType = oAuth2AccessTokenResponse.TokenType,
                ExpiresIn = DateTime.Now.AddSeconds(oAuth2AccessTokenResponse.ExpiresIn)
            };
            return displayInformationViewModel;
        }
    }
}
