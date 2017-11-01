using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace DigiKey.Api.Core.Configuration
{
    public class WebApiConfigHelper : ConfigurationHelper, IWebApiConfigHelper
    {
        // Static members are 'eagerly initialized', that is, 
        // immediately when class is loaded for the first time.
        // .NET guarantees thread safety for static initialization
        private static readonly WebApiConfigHelper _thisInstance = new WebApiConfigHelper();

        private const string _ClientId = "WebApi.ClientId";
        private const string _ClientSecret = "WebApi.ClientSecret";
        private const string _RedirectUri = "WebApi.RedirectUri";
        private const string _AccessToken = "WebApi.AccessToken";
        private const string _RefreshToken = "WebApi.RefreshToken";
        private const string _ExpirationDateTime = "WebApi.ExpirationDateTime";

        private WebApiConfigHelper()
        {
            try
            {
                var configDir = AppDomain.CurrentDomain.BaseDirectory;
                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(configDir, "webapi.config")
                };
                Console.WriteLine($"map.ExeConfigFilename {map.ExeConfigFilename}");
                _config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }
            catch (System.Exception)
            {
                // Add something here 
            }
        }

        public static WebApiConfigHelper Instance()
        {
            return _thisInstance;
        }

        /// <summary>
        ///     ClientId for WebApi usage
        /// </summary>
        public string ClientId
        {
            get => GetAttribute(_ClientId);
            set => Update(_ClientId, value);
        }

        /// <summary>
        ///     ClientSecret for WebApi usage
        /// </summary>
        public string ClientSecret
        {
            get => GetAttribute(_ClientSecret);
            set => Update(_ClientSecret, value);
        }

        /// <summary>
        ///     RedirectUri for WebApi usage
        /// </summary>
        public string RedirectUri
        {
            get => GetAttribute(_RedirectUri);
            set => Update(_RedirectUri, value);
        }

        /// <summary>
        ///     AccessToken for WebApi usage
        /// </summary>
        public string AccessToken
        {
            get => GetAttribute(_AccessToken);
            set => Update(_AccessToken, value);
        }

        /// <summary>
        ///     RefreshToken for WebApi usage
        /// </summary>
        public string RefreshToken
        {
            get => GetAttribute(_RefreshToken);
            set => Update(_RefreshToken, value);
        }

        /// <summary>
        ///     Client for WebApi usage
        /// </summary>
        public DateTime ExpirationDateTime
        {
            get
            {
                var dateTime = GetAttribute(_ExpirationDateTime);
                return DateTime.Parse(dateTime, null, DateTimeStyles.RoundtripKind);
            }
            set
            {
                var dateTime = value.ToString("o"); // "o" is "roundtrip"
                Update(_ExpirationDateTime, dateTime);
            }
        }
    }
}
