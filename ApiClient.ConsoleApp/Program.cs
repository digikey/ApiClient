using System;
using System.Runtime.InteropServices;
using Common.Logging;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DigiKey.Api.DigiKeyClient.ConsoleApp
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            var prog = new Program();

            prog.CallKeywordSearch();

            // This will keep the console window up until a key is press in the console window.
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        private void CallKeywordSearch()
        {
            // Brings the Console to Focus.
            BringConsoleToFront();

            var settings = WebApiSettings.CreateFromConfigFile();
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

                var client = new DigiKeyApiClient(settings);
                var response = client.KeywordSearch("P5555-ND").Result;

                // In order to pretty print the json object we need to do the following
                string jsonFormatted = JValue.Parse(response).ToString(Formatting.Indented);

                Console.WriteLine($"Reponse is {jsonFormatted} ");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        // Hack to bring the Console window to front.
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public void BringConsoleToFront()
        {
            SetForegroundWindow(GetConsoleWindow());
        }
    }
}
