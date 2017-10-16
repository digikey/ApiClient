using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiKey.AspNetOAuth2Sample.WebApp.ViewModels
{
    public class OAuth2ConfigurationViewModel
    {
        public string Callback { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Uri AuthorizationEndpoint { get; set; }
        public Uri TokenEndpoint { get; set; }
    }
}