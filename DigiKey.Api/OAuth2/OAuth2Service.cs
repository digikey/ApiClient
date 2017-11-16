using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Logging;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2.Models;
using Newtonsoft.Json;

namespace DigiKey.Api.OAuth2
{
    /// <summary>
    /// OAuth2Service accepts WebApiSettings to use to initialize and finish an OAuth2 Authorization and 
    /// get and set the Access Token and Refresh Token for the given ClientId and Client Secret in the WebApiSettings
    /// </summary>
    public class OAuth2Service
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OAuth2Service));

        private WebApiSettings _settings;

        public WebApiSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public OAuth2Service(WebApiSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Generates the authentication URL based on WebApiSettings.
        /// </summary>
        /// <param name="scopes">This is current not used and should be "".</param>
        /// <param name="state">This is not currently used.</param>
        /// <returns>String which is the oauth2 authorization url.</returns>
        public string GenerateAuthUrl(string scopes = "", string state = null)
        {
            var url = string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&response_type={4}",
                                    DigiKeyUriConstants.AuthorizationEndpoint,
                                    Settings.ClientId,
                                    scopes,
                                    Settings.RedirectUri,
                                    OAuth2Constants.ResponseTypes.Code);

            if (!string.IsNullOrWhiteSpace(state))
            {
                url = string.Format("{0}&state={1}", url, state);
            }
            _log.DebugFormat($"Authorize Url is {url}");

            return url;
        }

        /// <summary>
        ///     Finishes authorization by passing the authorization code to the Token endpoint
        /// </summary>
        /// <param name="code">Code value returned by the RedirectUri callback</param>
        /// <returns>Returns OAuth2AccessToken</returns>
        public async Task<OAuth2AccessToken> FinishAuthorization(string code)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };

            // Build up the body for the token request
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(OAuth2Constants.Code, code),
                new KeyValuePair<string, string>(OAuth2Constants.RedirectUri, Settings.RedirectUri),
                new KeyValuePair<string, string>(OAuth2Constants.ClientId, Settings.ClientId),
                new KeyValuePair<string, string>(OAuth2Constants.ClientSecret, Settings.ClientSecret),
                new KeyValuePair<string, string>(OAuth2Constants.GrantType,
                                                 OAuth2Constants.GrantTypes.AuthorizationCode)
            };

            // Request the token
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, DigiKeyUriConstants.TokenEndpoint);

            var httpClient = new HttpClient {BaseAddress = DigiKeyUriConstants.BaseAddress};

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new FormUrlEncodedContent(body);
            Console.WriteLine("HttpRequestMessage {0}", requestMessage.RequestUri.AbsoluteUri);
            var tokenResponse = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            var text = await tokenResponse.Content.ReadAsStringAsync();

            // Check if there was an error in the response
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var status = tokenResponse.StatusCode;
                if (status == HttpStatusCode.BadRequest)
                {
                    // Deserialize and return model
                    var errorResponse = JsonConvert.DeserializeObject<OAuth2AccessToken>(text);
                    return errorResponse;
                }

                // Throw error
                tokenResponse.EnsureSuccessStatusCode();
            }

            // Deserializes the token response if successfull
            var oAuth2Token = OAuth2Helpers.ParseOAuth2AccessTokenResponse(text);

            _log.DebugFormat("FinishAuthorization: " + oAuth2Token);

            return oAuth2Token;
        }

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <returns>Returns OAuth2AccessToken</returns>
        public async Task<OAuth2AccessToken> RefreshTokenAsync()
        {
            return await OAuth2Helpers.RefreshTokenAsync(Settings);
        }

        
    }
}
