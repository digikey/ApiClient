using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiKey.Api.OAuth2.Models
{
    public class OAuth2RefreshTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
        public string RefreshToken { get; set; }
    }
}
