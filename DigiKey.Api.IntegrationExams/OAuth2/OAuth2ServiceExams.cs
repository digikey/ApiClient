using System;
using System.Globalization;
using System.Security.Policy;
using System.Web;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using NUnit.Framework;

namespace DigiKey.Api.IntegrationExams.OAuth2
{
    public class OAuth2ServiceExams
    {
        private const string _ExpectedClientId = "A63CCCEA-5A00-4A1C-98AB-70D75B7BA113";
        private const string _ExpectedClientSecret = "K3xJ6jV2kK1gN4tT3gG5jL1tV0kE1iT5hO7iD4xQ7bT8fA4rH2";
        private const string _ExpectedRedirectUri = "https://localhost:44300/Home/FinishAuth";
        private const string _ExpectedAccessToken = "MWc1IY8Weu8rGRVl8DfVetpY4fHv";
        private const string _ExpectedRefreshToken = "trKdIq3ROC4NCP5jgjYj8MQEnk3Qn09nYyMr3uFMIy";

        private readonly DateTime _expectedExpirationDateTime =
            DateTime.Parse("2017-10-27T10:46:20.7102709-05:00", null, DateTimeStyles.RoundtripKind);

        public WebApiSettings BuildWebApiSettings()
        {
            return new WebApiSettings()
            {
                ClientId = _ExpectedClientId,
                ClientSecret = _ExpectedClientSecret,
                RedirectUri = _ExpectedRedirectUri,
                AccessToken = _ExpectedAccessToken,
                RefreshToken = _ExpectedRefreshToken,
                ExpirationDateTime = _expectedExpirationDateTime
            };
        }

        [Test]
        public void Method_Scenario_Expected()
        {
            // Arrange
            string content = SampleDataHelper.GetContent("AccessToken.json");

            // Act
            var result = OAuth2Helpers.ParseOAuth2AccessTokenResponse(content);

            // Assert 
            Assert.AreEqual(result.AccessToken, _ExpectedAccessToken);
        }
    }
}
