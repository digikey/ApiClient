using System.Diagnostics.CodeAnalysis;
using System.Text;
using DigiKey.Api.Extensions;
using Newtonsoft.Json;

namespace DigiKey.Api.OAuth2.Models
{
    public class OAuth2AccessToken
    {
        /// <summary>Gets or sets the access token.</summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>Gets or sets the error.</summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public bool IsError => Error.IsPresent();

        /// <summary>Gets or sets the error description.</summary>
        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>Gets or sets the id token.</summary>
        [JsonProperty(PropertyName = "id_token")]
        public string IdToken { get; set; }

        /// <summary>Gets or sets the refresh token.</summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>Gets or sets the token type.</summary>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>Gets or sets the expiration in seconds from now.</summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"   ----------- [ OAuth2AccessToken ] ----------");
            sb.AppendLine(@"     AccessToken      : " + AccessToken);
            sb.AppendLine(@"     Error            : " + Error);
            sb.AppendLine(@"     ErrorDescription : " + ErrorDescription);
            sb.AppendLine(@"     IdToken          : " + IdToken);
            sb.AppendLine(@"     RefreshToken     : " + RefreshToken);
            sb.AppendLine(@"     TokenType        : " + TokenType);
            sb.AppendLine(@"     ExpiresIn        : " + ExpiresIn);
            sb.AppendLine(@"   ---------------------------------------------");

            return sb.ToString();
        }
    }
}
