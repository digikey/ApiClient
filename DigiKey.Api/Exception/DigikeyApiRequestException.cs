using System.Collections.Generic;
using System.Net.Http;
using DigiKey.Api.Models;

namespace DigiKey.Api.Exception
{
    public class DigiKeyApiRequestException : DigiKeyApiException
    {
        public HttpResponseMessage Response { get; set; }

        public DigiKeyApiRequestException(HttpResponseMessage response,
                                          IEnumerable<OAuth2Error> errors,
                                          string message = default(string),
                                          System.Exception innerEx = null)
            : base(message
                   ?? $"DigiKey Api Request exception - Http Status Code: {response.StatusCode} - see errors for more details.",
                   errors,
                   innerEx)
        {
            this.Response = response;
        }
    }
}
