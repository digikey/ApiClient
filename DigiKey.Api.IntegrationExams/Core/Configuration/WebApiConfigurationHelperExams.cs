using System;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using ApiClient.Core.Configuration;
using NUnit.Framework;

namespace DigiKey.Api.IntegrationExams.Core.Configuration
{
    [TestFixture]
    public class ApiClientConfigurationHelperExams
    {
        private const string _ExpectedClientId = "9b902e2c-15be-4d1d-afdc-c6c5f23588b1";
        private const string _ExpectedClientSecret = "K3xJ6jV2kK1gN4tT3gG5jL1tV0kE1iT5hO7iD4xQ7bT8fA4rH2";
        private const string _ExpectedCallback = "https://localhost:44300/Home/FinishAuth";
        private const string _ExpectedAccessToken = "MWc1IY8Weu8rGRVl8DfVetpY4fHv";
        private const string _ExpectedRefreshToken = "trKdIq3ROC4NCP5jgjYj8MQEnk3Qn09nYyMr3uFMIy";
        private readonly DateTime _expectedExpirationDateTime = DateTime.Parse("2017-10-27T10:46:20.7102709-05:00", null, DateTimeStyles.RoundtripKind);

        private const string _ExpectedWriteClientId = "ClientId";
        private const string _ExpectedWriteClientSecret = "ClientSecret";
        private const string _ExpectedWriteCallback = "https://localhost:44300/callback";
        private const string _ExpectedWriteAccessToken = "AccessToken";
        private const string _ExpectedWriteRefreshToken = "RefreshToken";
        private readonly DateTime _expectedWriteExpirationDateTime = DateTime.Parse("2011-10-27T10:46:20.7102709-05:00", null, DateTimeStyles.RoundtripKind);

        [Test]
        public void Read_ClientId_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().ClientId;

            Assert.AreEqual(_ExpectedClientId, result);
        }

        [Test]
        public void Read_ClientSecret_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().ClientSecret;

            Assert.AreEqual(_ExpectedClientSecret, result);
        }

        [Test]
        public void Read_Callback_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().RedirectUri;

            Assert.AreEqual(_ExpectedCallback, result);
        }

        [Test]
        public void Read_AccessToken_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().AccessToken;

            Assert.AreEqual(_ExpectedAccessToken, result);
        }

        [Test]
        public void Read_RefreshToken_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().RefreshToken;

            Assert.AreEqual(_ExpectedRefreshToken, result);
        }

        [Test]
        public void Read_ExpirationDateTime_ReturnsSuccessfully()
        {
            var result = ApiClientConfigHelper.Instance().ExpirationDateTime;

            Assert.AreEqual(_expectedExpirationDateTime, result);
        }


        [Test]
        public void Method_Scenario_Expected()
        {
            // Arrange
            
            Console.WriteLine(DateTime.Now.ToString("o")); 
            // Act

            // Assert 
        }

        [Test]
        public void Write_ClientId_ReturnsSuccessfully()
        {
            ApiClientConfigHelper.Instance().ClientId = _ExpectedWriteClientId;

            var result = ApiClientConfigHelper.Instance().ClientId;
            ApiClientConfigHelper.Instance().Save();

            Assert.AreEqual(_ExpectedWriteClientId, result);
        }

        [Test]
        public void SolutionPath_Tests()
        {
            // Arrange
            string solutionpath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            Console.WriteLine($"solutionPath is {solutionpath}");
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"solutionPath is {baseDir}");
            var parentDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent;
            Console.WriteLine($"solutionPath is {parentDir.FullName}");

            if (File.Exists(Path.Combine(parentDir.FullName, "apiclient.config")))
            {
                Console.WriteLine("We found the config file");
            }
            else
            {
                Console.WriteLine("Where the heck are we!!");
            }

            // Act

            // Assert 
        }


    }
}
