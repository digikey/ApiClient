using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;

namespace OAuth2.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ClientId = "341d3e9f-b6f2-4cab-aa38-4f202dd7975b";
            var ClientSecret = "V7fP2yQ3rS2iB8hV8dU8yL8wF4kL3vS6eD3eS2wG3jX1tY4rQ3";
            var callback  = "https://localhost:44300/Home/FinishAuth"; 

            var credentials = new DigiKeyAppCredentials(ClientId, ClientSecret);

            Task.Run(async () =>
            {
                // authenticate
                var service = new OAuth2Service(credentials, callback);
                var authUrl = service.GenerateAuthUrl();

                Process.Start(authUrl);

                Console.WriteLine("Enter code :");
                var code = Console.ReadLine();

                var token = await service.FinishAuthorization(code, callback);
                Console.WriteLine($"AccessToken is {token}");

                //// var request
                Console.ReadLine();
            }).Wait();
        }
    }
}
