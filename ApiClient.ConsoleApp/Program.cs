using System;
using System.Runtime.InteropServices;
using ApiClient.Models;
using ApiClient.OAuth2;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiClient.ConsoleApp
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            var prog = new Program();

            prog.CallKeywordSearch();

            // This will keep the console window up until a key is pressed in the console window.
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        private void CallKeywordSearch()
        {
            var settings = ApiClientSettings.CreateFromConfigFile();
            _log.DebugFormat(settings.ToString());
            Console.WriteLine(settings.ToString());
            try
            {
                if (settings.ExpirationDateTime < DateTime.Now)
                {
                    // Let's refresh the token
                    var oAuth2Service = new OAuth2Service(settings);
                    var oAuth2AccessToken = oAuth2Service.RefreshTokenAsync().Result;
                    if (oAuth2AccessToken.IsError)
                    {
                        // Current Refresh token is invalid or expired 
                        _log.DebugFormat("Current Refresh token is invalid or expired ");
                        Console.WriteLine("Current Refresh token is invalid or expired ");
                        return;
                    }

                    settings.UpdateAndSave(oAuth2AccessToken);

                    _log.DebugFormat("After call to refresh");
                    _log.DebugFormat(settings.ToString());

                    Console.WriteLine("After call to refresh");
                    Console.WriteLine(settings.ToString());
                }

                var client = new ApiClientService(settings);
                var response = client.KeywordSearch("P5555-ND").Result;

                // In order to pretty print the json object we need to do the following
                var jsonFormatted = JToken.Parse(response).ToString(Formatting.Indented);

                Console.WriteLine($"Reponse is {jsonFormatted} ");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
