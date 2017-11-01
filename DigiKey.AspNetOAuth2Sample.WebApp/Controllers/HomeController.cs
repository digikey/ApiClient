using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using DigiKey.Api;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using DigiKey.Api.OAuth2.Models;
using DigiKey.AspNetOAuth2Sample.WebApp.ViewModels;
using Newtonsoft.Json;

namespace DigiKey.AspNetOAuth2Sample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public WebApiSettings SessionWebApiSettings
        {
            get => Session["WebApiSettings"] as WebApiSettings ?? WebApiSettings.CreateFromConfigFile();
            set => Session["WebApiSettings"] = value;
        }

        public ActionResult Index()
        {
            var viewModel = new OAuth2ConfigurationViewModel
            {
                ClientId = SessionWebApiSettings.ClientId,
                ClientSecret = SessionWebApiSettings.ClientSecret,
                Callback = SessionWebApiSettings.RedirectUri,
                AuthorizationEndpoint = DigiKeyUriConstants.AuthorizationEndpoint,
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(OAuth2ConfigurationViewModel authTokenModel)
        {
            SessionWebApiSettings.ClientId = authTokenModel.ClientId;
            SessionWebApiSettings.ClientSecret = authTokenModel.ClientSecret;
            SessionWebApiSettings.RedirectUri = authTokenModel.Callback;

            var authenticator = new OAuth2Service(SessionWebApiSettings);
            var scopes = "";
            var authUrl = authenticator.GenerateAuthUrl(scopes);

            return Redirect(authUrl);
        }

        public ActionResult FinishAuth()
        {
            var authenticator = new OAuth2Service(SessionWebApiSettings);
            var code = Request.Params["code"];

            var oAuth2AccessTokenResponse = authenticator.FinishAuthorization(code).Result;

            // For this Demo we'll just put the needed values in the ASP.NET session

            SessionWebApiSettings.AccessToken = oAuth2AccessTokenResponse.AccessToken;
            SessionWebApiSettings.RefreshToken = oAuth2AccessTokenResponse.RefreshToken;
            SessionWebApiSettings.ExpirationDateTime = DateTime.Now.AddSeconds(oAuth2AccessTokenResponse.ExpiresIn);
            SessionWebApiSettings.Save();

            return RedirectToAction("DisplayInformation", new RouteValueDictionary(oAuth2AccessTokenResponse));
        }

        public ActionResult DisplayInformation(OAuth2AccessToken oAuth2AccessToken)
        {
            var displayInformationViewModel =
                DisplayInformationViewModel.CreateFrom(SessionWebApiSettings, oAuth2AccessToken);

            return View(displayInformationViewModel);
        }

        public ActionResult RefreshAuthorization()
        {
            var viewModel = new OAuth2RefreshTokenRequest
            {
                RefreshToken = SessionWebApiSettings.RefreshToken,
                ClientId = SessionWebApiSettings.ClientId,
                ClientSecret = SessionWebApiSettings.ClientSecret,
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint.ToString()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            var postUrl = DigiKeyUriConstants.TokenEndpoint;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshTokenViewModel.RefreshToken),
            });

            var httpClient = new HttpClient();

            var clientIdConcatSecret =
                OAuth2Service.Base64Encode(SessionWebApiSettings.ClientId + ":" + SessionWebApiSettings.ClientSecret);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            var response = await httpClient.PostAsync(postUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var oAuth2AccessTokenResponse = JsonConvert.DeserializeObject<OAuth2AccessToken>(responseString);

            // For this Demo we'll just put the needed values in the ASP.NET session


            SessionWebApiSettings.AccessToken = oAuth2AccessTokenResponse.AccessToken;
            SessionWebApiSettings.RefreshToken = oAuth2AccessTokenResponse.RefreshToken;
            SessionWebApiSettings.ExpirationDateTime = DateTime.Now.AddSeconds(oAuth2AccessTokenResponse.ExpiresIn);
            SessionWebApiSettings.Save();

            return RedirectToAction("DisplayInformation", new RouteValueDictionary(oAuth2AccessTokenResponse));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult KeywordSearch()
        {
            var viewModel = new KeywordSearchRequestViewModel
            {
                Keywords = "P5555-ND",
                RecordCount = 25
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<string> DoSearch(string keyword)
        {
            var client = new DigiKeyClient(SessionWebApiSettings);
            return await client.KeywordSearch(keyword);
        }
    }
}
