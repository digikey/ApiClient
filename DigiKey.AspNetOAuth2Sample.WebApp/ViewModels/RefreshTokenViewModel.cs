using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiKey.AspNetOAuth2Sample.WebApp.ViewModels
{
    public class RefreshTokenViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
        public string RefreshToken { get; set; }
    }
}