# DigiKey.NET API Client Library V2

## V2Info

### Features

* Makes structured calls to the DigiKey API from .NET projects
* Logs in users using the OAuth2 code flow
* Easy data storage using POCO objects responses

### Basic Usage

```csharp
DigiKeyClient client = new DigiKeyClient(ConsumerKey, ConsumerSecret, userProfile.DigiKeyAuthToken, userProfile.DigiKeyAuthSecret);

Activity dayActivity = client.GetDayActivity(new DateTime(2012,7,1));
```

### Project Contents

* DigiKey - Client Library
* DigiKey.Tests - NUnit tests
* DigiKey.IntegrationTests - NUnit integration tests with the API
* DigiKey.AspNetOAUth2Sample.WebApp - A quick simple app using the OAuth2 client library

### Getting Started

1. Download the Project
2. Go to dev.DigiKey.com and create an app account (ConsumerKey and ConsumerSecret). If you are debugging in Visual Studio, set the callback URL to localhost and your local debug port, something like localhost:12345/DigiKey/Callback
3. Open Web.config and replace the settings with the ones you obtained from DigiKey
```
<add key="FitbitConsumerKey" value="YOUR_CONSUMER_KEY_HERE" />
<add key="FitbitConsumerSecret" value="YOUR_CONSUMER_SECRET_HERE" />
```
4. Run the sample web MVC project
5. (optional) Setting up the Integration Tests (which connect to the live API)
  Open the Configuration.cs file and insert an app ConsumerKey and ConsumerSecret, then follow the 3 step process listed in that app. You're trying to end up with permanent oauth credentials, doing that once in NUnit and saving it locally.

### Contributing

Lots of ways to contribute

* Help fill in the rest of the API calls. Please use the existing calls as an example. Also, at least one unit test and integration test (NUnit) are required before I'll take a pull request.
* Documentation - If you'd like to write some getting started guides, or more indepth walkthroughs, you're a hero to me.
* Suggestions for code cleanup / shrinking - Please engage in some conversation here on Github. 
* Adding example pages to the SampleWebMVC site showing what the API can do.

### Meta


