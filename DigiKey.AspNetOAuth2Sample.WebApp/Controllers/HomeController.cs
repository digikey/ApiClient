using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using DigiKey.Api;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using DigiKey.Api.OAuth2.Models;
using DigiKey.AspNetOAuth2Sample.WebApp.Models;
using DigiKey.AspNetOAuth2Sample.WebApp.ViewModels;

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
            var authUrl = authenticator.GenerateAuthUrl("");

            return Redirect(authUrl);
        }

        public ActionResult FinishAuth()
        {
            var authenticator = new OAuth2Service(SessionWebApiSettings);
            var code = Request.Params["code"];

            var oAuth2AccessTokenResponse = authenticator.FinishAuthorization(code).Result;
            SessionWebApiSettings.UpdateAndSave(oAuth2AccessTokenResponse);

            return RedirectToAction("DisplayInformation", new RouteValueDictionary(oAuth2AccessTokenResponse));
        }

        public ActionResult DisplayInformation(OAuth2AccessToken oAuth2AccessToken)
        {
            var viewModel = DisplayInformationViewModel.CreateFrom(SessionWebApiSettings, oAuth2AccessToken);

            return View(viewModel);
        }

        public ActionResult RefreshAuthorization()
        {
            var viewModel = new OAuth2RefreshTokenRequestViewModel
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
            var oAuth2AccessTokenResponse = await OAuth2Helpers.RefreshTokenAsync(SessionWebApiSettings);

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
            var client = new DigiKeyApiClient(SessionWebApiSettings);
            return await client.KeywordSearch(keyword);
        }
    }
}
