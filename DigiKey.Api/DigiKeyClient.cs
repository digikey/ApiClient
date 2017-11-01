using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Logging;
using DigiKey.Api.Core;
using DigiKey.Api.Extensions;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;

namespace DigiKey.Api
{
    public class DigiKeyClient
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DigiKeyClient));

        private WebApiSettings _settings;

        public WebApiSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public List<IDigiKeyInterceptor> DigiKeyInterceptorPipline { get; set; }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the DigiKeyClient instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public DigiKeyClient(WebApiSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Initialize();
        }

        private void Initialize()
        {
            DigiKeyInterceptorPipline = new List<IDigiKeyInterceptor> {new OAuth2RefreshTokenInterceptor()};

            var pipeline = this.CreatePipeline(DigiKeyInterceptorPipline);
            HttpClient = pipeline != null ? new HttpClient(pipeline) : new HttpClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var authenticationHeaderValue = new AuthenticationHeaderValue("Authorization", Settings.AccessToken);
            HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            HttpClient.DefaultRequestHeaders.Add("X-IBM-Client-ID", Settings.ClientId);
            HttpClient.BaseAddress = new Uri("https://api.digikey.com");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> KeywordSearch(string keyword)
        {
            var resourcePath = "/services/partsearch/v2/keywordsearch";

            var request = new KeywordSearchRequest
            {
                Keywords = "P5555-ND",
                RecordCount = 25
            };

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
                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
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
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
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
