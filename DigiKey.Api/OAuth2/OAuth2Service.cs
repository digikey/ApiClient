using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DigiKey.Api.OAuth2
{
    public class OAuth2Service
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly string _redirectUri;

        public OAuth2Service(DigiKeyAppCredentials credentials, string redirectUri)
        {
            _clientId = credentials.ClientId;
            _clientSecret = credentials.ClientSecret;
            _redirectUri = redirectUri;
        }

        public string GenerateAuthUrl(string scopes = "", string state = null)
        {
            var url = string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&response_type={4}",
                                    DigiKeyUriConstants.AuthorizationEndpoint,
                                    _clientId,
                                    scopes,
                                    _redirectUri,
                                    OAuth2Constants.ResponseTypes.Code);

            if (!string.IsNullOrWhiteSpace(state))
            {
                url = string.Format("{0}&state={1}", url, state);
            }

            return url;

        }

        public async Task<OAuth2AccessToken> ExchangeAuthCodeForAccessTokenAsync(string code)
        {
            var httpClient = new HttpClient();

            var postUrl = DigiKeyUriConstants.TokenEndpoint;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _clientId),

                //new KeyValuePair<string, string>("client_secret", AppSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri)
            });


            var clientIdConcatSecret = Base64Encode(_clientId + ":" + _clientSecret);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            var response = await httpClient.PostAsync(postUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var accessToken = ParseAccessTokenResponse(responseString);

            return accessToken;
        }

        /// <summary>
        ///     Finishes authorization by passing the authorization code to the Token endpoint
        /// </summary>
        /// <param name="code"></param>
        /// <param name="callback"></param>
        /// <returns>returns OAuth2AccessTokenResponse</returns>
        public async Task<OAuth2AccessTokenResponse> FinishAuthorization(string code, string callback)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };


            // Build up the body for the token request
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(OAuth2Constants.Code, code),
                new KeyValuePair<string, string>(OAuth2Constants.RedirectUri, callback),
                new KeyValuePair<string, string>(OAuth2Constants.ClientId, _clientId),
                new KeyValuePair<string, string>(OAuth2Constants.ClientSecret, _clientSecret),
                new KeyValuePair<string, string>(OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.AuthorizationCode)
            };

            // Request the token
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, DigiKeyUriConstants.TokenEndpoint);

            var httpClient = new HttpClient { BaseAddress = DigiKeyUriConstants.BaseAddress };

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new FormUrlEncodedContent(body);
            Console.WriteLine("HttpRequestMessage {0}", requestMessage.RequestUri.AbsoluteUri);
            HttpResponseMessage tokenResponse = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            string text = await tokenResponse.Content.ReadAsStringAsync();

            // Check if there was an error in the response
            if (!tokenResponse.IsSuccessStatusCode)
            {
                HttpStatusCode status = tokenResponse.StatusCode;
                if (status == HttpStatusCode.BadRequest)
                {
                    // Deserialize and return model
                    var errorResponse = JsonConvert.DeserializeObject<OAuth2AccessTokenResponse>(text);
                    return errorResponse;
                }
                // Throw error
                tokenResponse.EnsureSuccessStatusCode();
            }

            // Deserializes the token response if successfull
            var oAuth2TokenResponse = JsonConvert.DeserializeObject<OAuth2AccessTokenResponse>(text);

            return oAuth2TokenResponse;
        }

        public static OAuth2AccessToken ParseAccessTokenResponse(string responseString)
        {
            // assumption is the errors json will return in usual format eg. errors array
            var responseObject = JObject.Parse(responseString);

            var error = responseObject["errors"];
            if (error != null)
            {
                // var errors = new JsonDotNetSerializer().ParseErrors(responseString);
                throw new ArgumentException(
                    $"Unable to parse token response in method -- {nameof(ParseAccessTokenResponse)}.");
            }

            return JsonConvert.DeserializeObject<OAuth2AccessToken>(responseString);
        }

        /// <summary>
        ///     Convert plain text to a base 64 encoded string - http://stackoverflow.com/a/11743162
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
