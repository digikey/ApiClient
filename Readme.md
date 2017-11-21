# DigiKey.Api Client Library

### Features

* Makes structured calls to the DigiKey API from .NET projects
* Logs in users using the OAuth2 code flow

### Basic Usage

```csharp
var settings = WebApiSettings.CreateFromConfigFile();
DigiKeyClient client = new DigiKeyClient(settings);
postResponse = await sut.KeywordSearch("P5555-ND");
Console.WriteLine("respionse is {0}", postResponse);
```

### Project Contents

* **DigiKey.Api** - Client Library that contains the code to manage a config file with OAuth2 settings and classes to do the  OAuth2 and DigiKey Authentication. 
* DigiKey.Api.IntegrationExams - NUnit based Integration tests
* **DigiKey.AspNetOAuth2Sample.WebApp** - An ASP.NET app using the OAuth2 client library
* **DigiKey.Api.DigiKeyClient.ConsoleApp** - Console app to test out programmatic refresh of access token when needed and also check if access token failed to work and then refresh and try again.
* **DigiKey.Api.OAuth2Service.ConsoleApp** - Console app to create an access token and refresh token from the inform to user login to Digikey.

### Getting Started  

1. Download the zip file containing the solution Digikey.Api
2. You may need to Register an application in to receive your unique
   client ID and client secret, follow the steps (1 thru 4) from <https://api-portal.digikey.com/start>
3. . If you are debugging in Visual Studio, set the callback URL to localhost and your local debug port, something like https://localhost:44300/Home/FinishAuth
4. Create the file webapi.config in c:\users\\<user name>\AppData\Roaming\Digi-Key\DigiKey.Api" your will need to create the directories under Roaming and replace the settings with the ones you obtained from DigiKey. Using this webapi.config file we can use the same configuration for all the programs in this solution.****
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
        <add key="WebApi.ClientId"" value="YOUR_CLIENT_ID_HERE" />
        <add key="WebApi.ClientSecret" value="YOUR_CLIENT_SECRET_HERE" />
        <add key="WebApi.RedirectUri"  value="YOUR_REDIRECT_URI_HERE" />
        <add key="WebApi.AccessToken" value="" />
        <add key="WebApi.RefreshToken" value="" />
        <add key="WebApi.ExpirationDateTime" value="" />
    </appSettings>
</configuration>
```
4. Run DigiKey.Api.OAuth2Service.ConsoleApp to set the access token, refresh token and expiration date in webapi.config. 
5. Run DigiKey.Api.DigiKeyClient.ConsoleApp to get results from keyword search.




