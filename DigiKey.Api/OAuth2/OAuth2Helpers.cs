using System;
using System.Text;
using Common.Logging;
using DigiKey.Api.Exception;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2.Models;
using Newtonsoft.Json;

namespace DigiKey.Api.OAuth2
{
    /// <summary>
    /// Helper functions for OAuth2Service class and other classes calling OAuth2Service functions
    /// </summary>
    public static class OAuth2Helpers
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OAuth2Helpers));

        /// <summary>
        ///     Determines whether response has a unauthorized error message.
        /// </summary>
        /// <param name="content">json response</param>
        /// <returns>
        ///     <c>true</c> if token is stale in the error response; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTokenStale(string content)
        {
            var errors = JsonConvert.DeserializeObject<OAuth2Error>(content);
            return errors.HttpMessage.ToLower().Contains("unauthorized");
        }

        /// <summary>
        ///     Convert plain text to a base 64 encoded string - http://stackoverflow.com/a/11743162
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }


        /// <summary>
        ///     Parses the OAuth2 access token response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>instance of OAuth2AccessToken</returns>
        /// <exception cref="DigiKeyApiException">ull)</exception>
        public static OAuth2AccessToken ParseOAuth2AccessTokenResponse(string response)
        {
            try
            {
                var oAuth2AccessTokenResponse = JsonConvert.DeserializeObject<OAuth2AccessToken>(response);
                _log.DebugFormat("RefreshToken: " + oAuth2AccessTokenResponse.ToString());
                return oAuth2AccessTokenResponse;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                _log.DebugFormat($"Unable to parse OAuth2 access token response {e.Message}");
                throw new DigiKeyApiException($"Unable to parse OAuth2 access token response {e.Message}", null);
            }
        }
    }
}
