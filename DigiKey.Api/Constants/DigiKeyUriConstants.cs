using System;

namespace DigiKey.Api.Constants
{
    /// <summary>
    ///     Uri constants to talk to our OAuth2 server implementation.
    /// </summary>
    public static class DigiKeyUriConstants
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        public static readonly Uri BaseAddress = new Uri("https://sso.digikey.com");
        public static readonly Uri TokenEndpoint = new Uri("https://sso.digikey.com/as/token.oauth2");
        public static readonly Uri AuthorizationEndpoint = new Uri("https://sso.digikey.com/as/authorization.oauth2");
#pragma warning restore S1075 // URIs should not be hardcoded
    }
}
