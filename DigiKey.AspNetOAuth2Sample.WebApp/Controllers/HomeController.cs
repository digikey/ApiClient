using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using DigiKey.Api.Constants;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using DigiKey.Api.OAuth2.Models;
using DigiKey.AspNetOAuth2Sample.WebApp.ViewModels;

namespace DigiKey.AspNetOAuth2Sample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var clientId = ConfigurationManager.AppSettings["DigiKeyClientId"];
            var clientSecret = ConfigurationManager.AppSettings["DigiKeyClientSecret"];
            var callback = ConfigurationManager.AppSettings["DigiKeyOAuth2Callback"];

            var viewModel = new OAuth2ConfigurationViewModel
            {
                Callback = callback,
                ClientId = clientId,
                ClientSecret = clientSecret,
                AuthorizationEndpoint = DigiKeyUriConstants.AuthorizationEndpoint,
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(OAuth2ConfigurationViewModel authTokenModel)
        {
            var appCredentials = new DigiKeyAppCredentials(authTokenModel.ClientId, authTokenModel.ClientSecret);
            Session["AppCredentials"] = appCredentials;
            Session["DigiKeyOAuth2Callback"] = authTokenModel.Callback;

            var authenticator = new OAuth2Service(appCredentials, authTokenModel.Callback);
            var scopes = "";
            var authUrl = authenticator.GenerateAuthUrl(scopes);

            return Redirect(authUrl);
        }

        public ActionResult FinishAuth()
        {
            var appCredentials = Session["AppCredentials"] as DigiKeyAppCredentials;
            var callback = Session["DigiKeyOAuth2Callback"] as string;

            var authenticator = new OAuth2Service(appCredentials, callback);
            var code = Request.Params["code"];

            var oAuth2AccessTokenResponse = authenticator.FinishAuthorization(code, callback).Result;

            return RedirectToAction("DisplayInformation", new RouteValueDictionary(oAuth2AccessTokenResponse));
        }

        public ActionResult DisplayInformation(OAuth2AccessTokenResponse oAuth2AccessTokenResponse)
        {
            var appCredentials = Session["AppCredentials"] as DigiKeyAppCredentials;
            var displayInformationViewModel =
                DisplayInformationViewModel.CreateFrom(appCredentials, oAuth2AccessTokenResponse);

            return View(displayInformationViewModel);
        }

        public ActionResult RefreshAuthorization()
        {
            //var oAuth = new PingFedOAuth2Service(ServiceDescription,
            //                                     Settings.ClientId,
            //                                     Settings.ClientSecret,
            //                                     Settings.Callback);

            ////OAuth2RefreshTokenRequest request;
            ////var tokenReponse = oAuth.RefreshToken(request);
            
            var viewModel = new OAuth2RefreshTokenRequest();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            throw new NotImplementedException();
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
    }
}
