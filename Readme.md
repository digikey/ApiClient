# DigiKey.Api Client Library V2

## V2Info

### Features

* Makes structured calls to the DigiKey API from .NET projects
* Logs in users using the OAuth2 code flow
* Easy data storage using POCO objects responses

### Basic Usage

```csharp
var settings = WebApiSettings.CreateFromConfigFile();
DigiKeyClient client = new DigiKeyClient(settings);
postResponse = await sut.KeywordSearch("P5555-ND");
Console.WriteLine("respionse is {0}", postResponse);
```

### Project Contents

* DigiKey.Api - Client Library
* DigiKey.Api.IntegrationExams - NUnit based Integration tests
* DigiKey.IntegrationTests - NUnit integration tests with the API
* DigiKey.AspNetOAUth2Sample.WebApp - An ASP.NET app using the OAuth2 client library

### Getting Started  (Currently not correct.. )

1. Download the Project
2. Go to dev.DigiKey.com and create an app account (ConsumerKey and ConsumerSecret). If you are debugging in Visual Studio, set the callback URL to localhost and your local debug port, something like localhost:12345/DigiKey/Callback
3. Open webapi.config and replace the settings with the ones you obtained from DigiKey
```
<add key="WebApi.ClientId"" value="YOUR_CLIENT_ID_HERE" />
<add key="WebApi.ClientSecret" value="YOUR_CLIENT_SECRET_HERE" />
<add key="WebApi.RedirectUri"  value="YOUR_REDIRECT_URI_HERE" />
```
4. Run the sample web MVC project
5. (optional) Setting up the Integration Tests (which connect to the live API)
  Open the Configuration.cs file and insert an app ConsumerKey and ConsumerSecret, then follow the 3 step process listed in that app. You're trying to end up with permanent oauth credentials, doing that once in NUnit and saving it locally.

### Meta


