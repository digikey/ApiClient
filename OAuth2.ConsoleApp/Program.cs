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
            var credentials = new DigiKeyAppCredentials(ClientId, ClientSecret);

            Task.Run(async () =>
            {
                // authenticate
                var service =
                    new OAuth2Service(credentials, "https://localhost:44300/Home/FinishAuth"); // example call back url
                var authUrl = service.GenerateAuthUrl();

                Process.Start(authUrl);

                Console.WriteLine("Enter code :");
                var pin = Console.ReadLine();

                var token = await service.ExchangeAuthCodeForAccessTokenAsync(pin);

                //// var request
                //var fitbitClient = new FitbitClient(credentials, token);

                //var stats = await fitbitClient.GetActivitiesStatsAsync();

                //Console.WriteLine($"Total steps: {stats.Lifetime.Total.Steps}");
                Console.ReadLine();
            }).Wait();
        }
    }
}
