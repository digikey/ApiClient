using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using DigiKey.Api.Constants;
using DigiKey.Api.Core;
using DigiKey.Api.Exception;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2.Models;
using Newtonsoft.Json;

namespace DigiKey.Api.OAuth2
{
    public class OAuth2RefreshTokenInterceptor : IDigiKeyInterceptor
    {
        private const string _CustomHeader = "DigiKey.NET-StaleTokenRetry";

        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request,
                                                          CancellationToken cancellationToken,
                                                          DigiKeyClient client)
        {
            return null;
        }

        public async Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response,
                                                                 CancellationToken cancellationToken,
                                                                 DigiKeyClient invokingClient)
        {
            if (response.Result.StatusCode == HttpStatusCode.Unauthorized) //Unauthorized, then there is a chance token is stale
            {
                var responseBody = await response.Result.Content.ReadAsStringAsync();

                if (IsTokenStale(responseBody))
                {
                    Debug.WriteLine(
                        "Stale token detected. Invoking registered tokenManager.RefreskToken to refresh it");
                    Console.WriteLine($"invokingClient.Settings.AccessToken is {invokingClient.Settings.AccessToken}");
                    var oAuth2AccessToken = await RefreshTokenAsync(invokingClient);
                    Console.WriteLine($"invokingClient.Settings.AccessToken is {invokingClient.Settings.AccessToken}");
                    Console.WriteLine($"oAuth2AccessToken is {oAuth2AccessToken.AccessToken}");

                    //Only retry the first time.
                    if (!response.Result.RequestMessage.Headers.Contains(_CustomHeader))
                    {
                        var clonedRequest = await response.Result.RequestMessage.CloneAsync();
                        clonedRequest.Headers.Add(_CustomHeader, _CustomHeader);
                        clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Authorization", invokingClient.Settings.AccessToken);
                        return await invokingClient.HttpClient.SendAsync(clonedRequest, cancellationToken);
                    }
                    else if (response.Result.RequestMessage.Headers.Contains(_CustomHeader))
                    {
                        throw new DigikeyApiUnauthorizedException(response.Result, message: $"In interceptor {nameof(OAuth2RefreshTokenInterceptor)} inside method {nameof(InterceptResponse)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.Result.StatusCode}");
                    }
                }
            }

            //let the pipeline continue
            return null;
        }

        private bool IsTokenStale(string responseBody)
        {
            var errors = JsonConvert.DeserializeObject<OAuth2Error>(responseBody);
            return errors.HttpMessage.ToLower().Contains("unauthorized");
        }

        public async Task<OAuth2AccessToken> RefreshTokenAsync(DigiKeyClient client)
        {
            var postUrl = DigiKeyUriConstants.TokenEndpoint.ToString();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", client.Settings.RefreshToken),
            });

            var httpClient = new HttpClient();

            var clientIdConcatSecret = OAuth2Service.Base64Encode(client.Settings.ClientId + ":" + client.Settings.ClientSecret);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            var response = await httpClient.PostAsync(postUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var oAuth2AccessToken = JsonConvert.DeserializeObject<OAuth2AccessToken>(responseString);

            client.Settings.AccessToken = oAuth2AccessToken.AccessToken;
            client.Settings.RefreshToken = oAuth2AccessToken.RefreshToken;

            return oAuth2AccessToken;
        }
    }
}
