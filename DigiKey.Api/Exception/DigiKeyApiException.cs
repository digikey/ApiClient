using System.Collections.Generic;
using DigiKey.Api.Models;

namespace DigiKey.Api.Exception
{
    public class DigiKeyApiException : System.Exception
    {
        public List<OAuth2Error> OAuth2Errors { get; set; }

        public DigiKeyApiException(string message, IEnumerable<OAuth2Error> errors, System.Exception innerEx = null) :
            base(message, innerEx)
        {
            OAuth2Errors = errors != null ? new List<OAuth2Error>(errors) : new List<OAuth2Error>();
        }
    }
}
