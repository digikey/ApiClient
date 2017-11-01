using System.Collections.Generic;
using System.Net.Http;
using DigiKey.Api.Models;

namespace DigiKey.Api.Exception
{
    public class DigikeyApiUnauthorizedException : DigiKeyApiRequestException
    {
        public DigikeyApiUnauthorizedException(HttpResponseMessage response,
                                               IEnumerable<OAuth2Error> errors = null,
                                               string message = default(string))
            : base(response,
                   errors,
                   message
                   ?? $"DigiKey Unauthorized exception - HTTP Status Code-- {response.StatusCode} -- see errors for more details.")
        {
        }
    }
}
