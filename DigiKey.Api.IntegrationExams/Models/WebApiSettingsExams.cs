using System;
using System.Globalization;
using DigiKey.Api.Core.Configuration;
using DigiKey.Api.Models;
using NUnit.Framework;

namespace DigiKey.Api.IntegrationExams.Models
{
    [TestFixture]
    public class WebApiSettingsExams
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
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.ClientId, WebApiConfigHelper.Instance().ClientId);
        }

        [Test]
        public void WebApiSettings_SettingClientSecret_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.ClientSecret, WebApiConfigHelper.Instance().ClientSecret);
        }

        [Test]
        public void WebApiSettings_SettingRedirectUri_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.RedirectUri, WebApiConfigHelper.Instance().RedirectUri);
        }

        [Test]
        public void WebApiSettings_SettingAccessToken_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.AccessToken, WebApiConfigHelper.Instance().AccessToken);
        }

        [Test]
        public void WebApiSettings_SettingRefreshToken_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.RefreshToken, WebApiConfigHelper.Instance().RefreshToken);
        }

        [Test]
        public void WebApiSettings_SettingExpirationDateTime_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Assert 
            Assert.AreEqual(settings.ExpirationDateTime, WebApiConfigHelper.Instance().ExpirationDateTime);
        }



        [Test]
        public void WebApiSettings_WriteSettingClientId_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.ClientId = _ExpectedClientId;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientId, WebApiConfigHelper.Instance().ClientId);
        }

        [Test]
        public void WebApiSettings_WriteSettingClientSecret_WebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.ClientSecret = _ExpectedClientSecret;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientSecret, WebApiConfigHelper.Instance().ClientSecret);
        }
        [Test]
        public void WebApiSettings_WriteSettingRedirectUri_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.RedirectUri = _ExpectedRedirectUri;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedClientId, WebApiConfigHelper.Instance().ClientId);
        }
        [Test]
        public void WebApiSettings_WriteSettingAccessToken_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.AccessToken = _ExpectedAccessToken;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedAccessToken, WebApiConfigHelper.Instance().AccessToken);
        }
        [Test]
        public void WebApiSettings_WriteSettingRefreshToken_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.RefreshToken = _ExpectedRefreshToken;
            settings.Save();

            // Assert 
            Assert.AreEqual(_ExpectedRefreshToken, WebApiConfigHelper.Instance().RefreshToken);
        }
        [Test]
        public void WebApiSettings_WriteSettingExpirationDateTime_ReturnsWebApiConfigValue()
        {
            // Arrange 
            var settings = WebApiSettings.CreateFromConfigFile();

            // Act
            settings.ExpirationDateTime = _expectedExpirationDateTime;
            settings.Save();

            // Assert 
            Assert.AreEqual(_expectedExpirationDateTime, WebApiConfigHelper.Instance().ExpirationDateTime);
        }
    }
}
