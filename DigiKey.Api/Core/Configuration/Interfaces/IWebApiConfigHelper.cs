using System;

namespace DigiKey.Api.Core.Configuration
{
    public interface IWebApiConfigHelper
    {
        /// <summary>
        ///     ClientId for WebApi Usage
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        ///     ClientSecret for WebApi Usage
        /// </summary>
        string ClientSecret { get; set; }

        /// <summary>
        ///     RedirectUri for WebApi Usage
        /// </summary>
        string RedirectUri { get; set; }

        /// <summary>
        ///     AccessToken for WebApi Usage
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        ///     RefreshToken for WebApi Usage
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        ///     ExpirationDateTime for WebApi Usage
        /// </summary>
        DateTime ExpirationDateTime { get; set; }
    }
}
