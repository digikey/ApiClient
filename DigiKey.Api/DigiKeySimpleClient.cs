﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Logging;
using DigiKey.Api.Exception;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;

namespace DigiKey.Api
{
    public class DigiKeySimpleClient
    {
        private const string _CustomHeader = "DigiKey.NET-StaleTokenRetry";
        private static readonly ILog _log = LogManager.GetLogger(typeof(DigiKeySimpleClient));

        private WebApiSettings _settings;

        public WebApiSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the DigiKeyClient instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public DigiKeySimpleClient(WebApiSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Initialize();
        }

        private void Initialize()
        {
            HttpClient = new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var authenticationHeaderValue = new AuthenticationHeaderValue("Authorization", Settings.AccessToken);
            HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            HttpClient.DefaultRequestHeaders.Add("X-IBM-Client-ID", Settings.ClientId);
            HttpClient.BaseAddress = new Uri("https://api.digikey.com");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void CheckIfAccessTokenIsExpired()
        {
            if (_settings.ExpirationDateTime < DateTime.Now)
            {
                // Let's refresh the token
                var oAuth2Service = new OAuth2Service(_settings);
                var oAuth2AccessToken = oAuth2Service.RefreshTokenAsync().Result;
                if (oAuth2AccessToken.IsError)
                {
                    // Current Refresh token is invalid or expired 
                    Console.WriteLine("Current Refresh token is invalid or expired ");
                    return;
                }

                _settings.UpdateAndSave(oAuth2AccessToken);
                Console.WriteLine("DigiKeySimpleClient::CheckifAccessTokenIsExpired() call to refresh");
                Console.WriteLine(_settings.ToString());
            }
        }

        public async Task<string> KeywordSearch(string keyword)
        {
            var resourcePath = "/services/partsearch/v2/keywordsearch";

            var request = new KeywordSearchRequest
            {
                Keywords = keyword ?? "P5555-ND",
                RecordCount = 25
            };

            CheckIfAccessTokenIsExpired();
            var postResponse = await PostAsJsonAsync(resourcePath, request);

            return GetServiceResponse(postResponse).Result;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string resourcePath, T postRequest)
        {
            _log.DebugFormat(">DigiKeyClient::PostAsJsonAsync()");
            try
            {
                var response = await HttpClient.PostAsJsonAsync(resourcePath, postRequest);
                _log.DebugFormat("<DigiKeyClient::PostAsJsonAsync()");

                //Unauthorized, then there is a chance token is stale
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (OAuth2Helpers.IsTokenStale(responseBody))
                    {
                        _log.DebugFormat(
                            $"Stale access token detected ({_settings.AccessToken}. Calling RefreshTokenAsync to refresh it");
                        await OAuth2Service.RefreshTokenAsync(_settings);
                        _log.DebugFormat($"New Access token is {_settings.AccessToken}");

                        //Only retry the first time.
                        if (!response.RequestMessage.Headers.Contains(_CustomHeader))
                        {
                            HttpClient.DefaultRequestHeaders.Add(_CustomHeader, _CustomHeader);
                            HttpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Authorization", _settings.AccessToken);
                            return await PostAsJsonAsync(resourcePath, postRequest);
                        }
                        else if (response.RequestMessage.Headers.Contains(_CustomHeader))
                        {
                            throw new DigikeyApiUnauthorizedException(response,
                                                                      message:
                                                                      $"In interceptor {nameof(OAuth2RefreshTokenInterceptor)} inside method {nameof(PostAsJsonAsync)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.StatusCode}");
                        }
                    }
                }

                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (DigikeyApiUnauthorizedException daue)
            {
                Console.WriteLine($"daue exception is {daue.Message}");
                throw;
            }
        }

        protected async Task<string> GetServiceResponse(HttpResponseMessage response)
        {
            _log.DebugFormat(">DigiKeyClient::GetServiceResponse()");
            var postResponse = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    postResponse = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Response");
                Console.WriteLine("  Status Code : {0}", response.StatusCode);
                Console.WriteLine("  Content     : {0}", errorMessage);
                Console.WriteLine("  Reason      : {0}", response.ReasonPhrase);
                var resp = new HttpResponseMessage(response.StatusCode)
                {
                    Content = response.Content,
                    ReasonPhrase = response.ReasonPhrase
                };
                throw new HttpResponseException(resp);
            }

            _log.DebugFormat("<DigiKeyClient::GetServiceResponse()");
            return postResponse;
        }
    }
}