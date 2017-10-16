using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;

namespace DigiKey.Net.IntegrationExams
{
    public static class Program
    {
     static void Main(string[] args)
     {
         const string clientId = "341d3e9f-b6f2-4cab-aa38-4f202dd7975b";
         const string clientSecret = "V7fP2yQ3rS2iB8hV8dU8yL8wF4kL3vS6eD3eS2wG3jX1tY4rQ3";
         const string callback = "https://localhost:44300/Home/FinishAuth"; 

         var credentials = new DigiKeyAppCredentials(clientId, clientSecret);

         Task.Run(async () =>
            {
                // authenticate
                var service = new OAuth2Service(credentials, callback); 
                var authUrl = service.GenerateAuthUrl();

                Process.Start(authUrl);

                var code = Console.ReadLine();

                var token = await service.FinishAuthorization(code, callback);

                Console.WriteLine("Token is {0}", token);
                Console.ReadLine();

            }).Wait();
     }
    }
}
