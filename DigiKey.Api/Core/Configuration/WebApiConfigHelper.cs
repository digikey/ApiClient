using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using DigiKey.Api.Models;

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
                // Us this version of if you want to use webapi.config from bin/[Debug|Release] directory
                // var configDir = AppDomain.CurrentDomain.BaseDirectory;

                // Use this for writing the webapi.config in c:\users\<<user name>\AppData\Roaming\Digi-Key\DigiKey.API"
                // Using this version we can use the same webapi.config for all the programs in this solution.
                var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digi-Key", "DigiKey.API");
                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(configDir, "webapi.config"),
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
            get { return GetAttribute(_ClientId); }
            set { Update(_ClientId, value); }
        }

        /// <summary>
        ///     ClientSecret for WebApi usage
        /// </summary>
        public string ClientSecret
        {
            get { return GetAttribute(_ClientSecret); }
            set { Update(_ClientSecret, value); }
        }

        /// <summary>
        ///     RedirectUri for WebApi usage
        /// </summary>
        public string RedirectUri
        {
            get { return GetAttribute(_RedirectUri); }
            set { Update(_RedirectUri, value); }
        }

        /// <summary>
        ///     AccessToken for WebApi usage
        /// </summary>
        public string AccessToken
        {
            get { return GetAttribute(_AccessToken); }
            set { Update(_AccessToken, value); }
        }

        /// <summary>
        ///     RefreshToken for WebApi usage
        /// </summary>
        public string RefreshToken
        {
            get { return GetAttribute(_RefreshToken); }
            set { Update(_RefreshToken, value); }
        }

        /// <summary>
        ///     Client for WebApi usage
        /// </summary>
        public DateTime ExpirationDateTime
        {
            get
            {
                var dateTime = GetAttribute(_ExpirationDateTime);
                return dateTime == null ? DateTime.MinValue : DateTime.Parse(dateTime, null, DateTimeStyles.RoundtripKind);
            }
            set
            {
                var dateTime = value.ToString("o"); // "o" is "roundtrip"
                Update(_ExpirationDateTime, dateTime);
            }
        }
    }
}
