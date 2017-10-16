using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiKey.Api.Models
{
    public class DigiKeyAppCredentials
    {
        public string ClientId { get; }
        public string ClientSecret { get;  }

        public DigiKeyAppCredentials(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
