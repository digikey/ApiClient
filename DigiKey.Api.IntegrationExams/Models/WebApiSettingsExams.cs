using System;
using System.Globalization;
using ApiClient.Core.Configuration;
using ApiClient.Models;
using NUnit.Framework;

namespace DigiKey.Api.IntegrationExams.Models
{
    [TestFixture]
    public class ApiClientSettingsExams
    {
        private const string _ExpectedClientId = "A63CCCEA-5A00-4A1C-98AB-70D75B7BA113";
        private const string _ExpectedClientSecret = "K3xJ6jV2kK1gN4tT3gG5jL1tV0kE1iT5hO7iD4xQ7bT8fA4rH2";
        private const string _ExpectedRedirectUri = "https://localhost:44300/Home/FinishAuth";
        private const string _ExpectedAccessToken = "MWc1IY8Weu8rGRVl8DfVetpY4fHv";
        private const string _ExpectedRefreshToken = "trKdIq3ROC4NCP5jgjYj8MQEnk3Qn09nYyMr3uFMIy";

        private readonly DateTime _expectedExpirationDateTime =
            DateTime.Parse("2017-10-25T10:26:10.7102709-05:00", null, DateTimeStyles.RoundtripKind);

        [Test]
        public void WebApiSettings_SettingClientId_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();
            settings.Save();

            // Act

            // Assert 
            Assert.AreEqual(settings.ClientId, ApiClientConfigHelper.Instance().ClientId);
        }

        [Test]
        public void ApiClientSettings_SettingClientSecret_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.ClientSecret, ApiClientConfigHelper.Instance().ClientSecret);
        }

        [Test]
        public void ApiClientSettings_SettingRedirectUri_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.RedirectUri, ApiClientConfigHelper.Instance().RedirectUri);
        }

        [Test]
        public void ApiClientSettings_SettingAccessToken_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.AccessToken, ApiClientConfigHelper.Instance().AccessToken);
        }

        [Test]
        public void ApiClientSettings_SettingRefreshToken_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.RefreshToken, ApiClientConfigHelper.Instance().RefreshToken);
        }

        [Test]
        public void ApiClientSettings_SettingExpirationDateTime_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.ExpirationDateTime, ApiClientConfigHelper.Instance().ExpirationDateTime);
        }



        [Test]
        public void ApiClientSettings_WriteSettingClientId_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.ClientId = _ExpectedClientId;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientId, ApiClientConfigHelper.Instance().ClientId);
        }

        [Test]
        public void ApiClientSettings_WriteSettingClientSecret_ApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.ClientSecret = _ExpectedClientSecret;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientSecret, ApiClientConfigHelper.Instance().ClientSecret);
        }
        [Test]
        public void ApiClientSettings_WriteSettingRedirectUri_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.RedirectUri = _ExpectedRedirectUri;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientId, ApiClientConfigHelper.Instance().ClientId);
        }
        [Test]
        public void ApiClientSettings_WriteSettingAccessToken_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.AccessToken = _ExpectedAccessToken;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedAccessToken, ApiClientConfigHelper.Instance().AccessToken);
        }
        [Test]
        public void ApiClientSettings_WriteSettingRefreshToken_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.RefreshToken = _ExpectedRefreshToken;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedRefreshToken, ApiClientConfigHelper.Instance().RefreshToken);
        }
        [Test]
        public void ApiClientSettings_WriteSettingExpirationDateTime_ReturnsApiClientConfigValue()
        {
            // Arrange 
            var settings = ApiClientSettings.CreateFromConfigFile();

            // Act
            settings.ExpirationDateTime = _expectedExpirationDateTime;
            settings.Save();

            // Assert 
            Assert.AreEqual(_expectedExpirationDateTime, ApiClientConfigHelper.Instance().ExpirationDateTime);
        }
    }
}
