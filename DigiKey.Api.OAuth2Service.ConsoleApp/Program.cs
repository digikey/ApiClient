using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using DigiKey.Api.Extensions;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;

namespace DigiKey.Api.ConsoleApp
{
    public class Program
    {
        private WebApiSettings _settings;

        static void Main()
        {
            var prog = new Program();

            // Read configuration values from webapi.config file and run OAuth2 code flow with OAuth2 Server
            prog.Authorize();

            // This will keep the console window up until a key is press in the console window.
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        ///     OAuth2 code flow authorization with webapi.config values
        /// </summary>
        private async void Authorize()
        {
            // read settings values from webapi.config
            _settings = WebApiSettings.CreateFromConfigFile();
            Console.WriteLine(_settings.ToString());

            // start up a HttpListener for the callback(RedirectUri) from the OAuth2 server
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(_settings.RedirectUri.EnsureTrailingSlash());
            Console.WriteLine($"listening to {_settings.RedirectUri}");
            httpListener.Start();

            // Initialize our OAuth2 service
            var oAuth2Service = new OAuth2Service(_settings);
            var scopes = "";

            // create Authorize url and send call it thru Process.Start
            var authUrl = oAuth2Service.GenerateAuthUrl(scopes);
            Process.Start(authUrl);

            // get the URL returned from the callback(RedirectUri)
            var context = await httpListener.GetContextAsync();

            // Done with the callback, so stop the HttpListener
            httpListener.Stop();

            // exact the query parameters from the returned URL
            var queryString = context.Request.Url.Query;
            var queryColl = HttpUtility.ParseQueryString(queryString);

            // Grab the needed query parameter code from the query collection
            var code = queryColl["code"];
            Console.WriteLine($"Using code {code}");

            // Brings the Console to Focus.
            BringConsoleToFront();

            // Pass the returned code value to finish the OAuth2 authorization
            var result = await oAuth2Service.FinishAuthorization(code);

            // Check if you got an error during finishing the OAuth2 authorization
            if (result.IsError)
            {
                Console.WriteLine("\n\nError            : {0}", result.Error);
                Console.WriteLine("\n\nError.Description: {0}", result.ErrorDescription);
            }
            else
            {
                // Display the Access Token and Refresh Token to the Console.
                Console.WriteLine();
                Console.WriteLine("Access token : {0}", result.AccessToken);
                Console.WriteLine("Refresh token: {0}", result.RefreshToken);
                Console.WriteLine("Expires in   : {0}", result.ExpiresIn);

                _settings.UpdateAndSave(result);
                Console.WriteLine("After a good refresh");
                Console.WriteLine(_settings.ToString());
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
