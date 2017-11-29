using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DigiKey.Api.Models
{
    public class OAuth2Error
    {
        [JsonProperty("httpCode")]
        public string HttpStatusCode { get; set; }

        [JsonProperty("httpMessage")]
        public string HttpMessage { get; set; }

        [JsonProperty("moreInformation")]
        public string MoreInformation { get; set; }
    }
}
