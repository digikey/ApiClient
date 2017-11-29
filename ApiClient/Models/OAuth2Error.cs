using Newtonsoft.Json;

namespace ApiClient.Models
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
