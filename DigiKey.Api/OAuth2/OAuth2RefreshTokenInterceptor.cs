using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using DigiKey.Api.Core;
using DigiKey.Api.Core.Interfaces;
using DigiKey.Api.Exception;

namespace DigiKey.Api.OAuth2
{
    public class OAuth2RefreshTokenInterceptor : IDigiKeyInterceptor
    {
        private const string _CustomHeader = "DigiKey.NET-StaleTokenRetry";

        public Task<HttpResponseMessage> OnRequest(HttpRequestMessage request,
                                                          CancellationToken cancellationToken,
                                                          DigiKeyClient client)
        {
            return null;
        }

        public async Task<HttpResponseMessage> OnResponse(Task<HttpResponseMessage> response,
                                                                 CancellationToken cancellationToken,
                                                                 DigiKeyClient invokingClient)
        {
            //Unauthorized, then there is a chance token is stale
            if (response.Result.StatusCode == HttpStatusCode.Unauthorized)
            {
                var responseBody = await response.Result.Content.ReadAsStringAsync();

                if (OAuth2Helpers.IsTokenStale(responseBody))
                {
                    Debug.WriteLine(
                        "Stale token detected. Invoking registered tokenManager.RefreskToken to refresh it");
                    Console.WriteLine($"invokingClient.Settings.AccessToken is {invokingClient.Settings.AccessToken}");
                    var oAuth2AccessToken = await OAuth2Service.RefreshTokenAsync(invokingClient.Settings);
                    Console.WriteLine($"invokingClient.Settings.AccessToken is {invokingClient.Settings.AccessToken}");
                    Console.WriteLine($"oAuth2AccessToken is {oAuth2AccessToken.AccessToken}");

                    //Only retry the first time.
                    if (!response.Result.RequestMessage.Headers.Contains(_CustomHeader))
                    {
                        var clonedRequest = await response.Result.RequestMessage.CloneAsync();
                        clonedRequest.Headers.Add(_CustomHeader, _CustomHeader);
                        clonedRequest.Headers.Authorization =
                            new AuthenticationHeaderValue("Authorization", invokingClient.Settings.AccessToken);
                        return await invokingClient.HttpClient.SendAsync(clonedRequest, cancellationToken);
                    }
                    else if (response.Result.RequestMessage.Headers.Contains(_CustomHeader))
                    {
                        throw new DigikeyApiUnauthorizedException(response.Result,
                                                                  message:
                                                                  $"In interceptor {nameof(OAuth2RefreshTokenInterceptor)} inside method {nameof(OnResponse)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.Result.StatusCode}");
                    }
                }
            }

            //let the pipeline continue
            return null;
        }
    }
}
