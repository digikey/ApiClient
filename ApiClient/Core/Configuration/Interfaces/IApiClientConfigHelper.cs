using System;

namespace ApiClient.Core.Configuration.Interfaces
{
    public interface IApiClientConfigHelper
    {
        /// <summary>
        ///     ClientId for ApiClient Usage
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        ///     ClientSecret for ApiClient Usage
        /// </summary>
        string ClientSecret { get; set; }

        /// <summary>
        ///     RedirectUri for ApiClient Usage
        /// </summary>
        string RedirectUri { get; set; }

        /// <summary>
        ///     AccessToken for ApiClient Usage
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        ///     RefreshToken for ApiClient Usage
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        ///     ExpirationDateTime for ApiClient Usage
        /// </summary>
        DateTime ExpirationDateTime { get; set; }
    }
}
