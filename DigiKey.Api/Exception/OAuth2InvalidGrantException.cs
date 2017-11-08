using Newtonsoft.Json;

namespace DigiKey.Api.Exception
{
    public class OAuth2InvalidGrantException
    {
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
