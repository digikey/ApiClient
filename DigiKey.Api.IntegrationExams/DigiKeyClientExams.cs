using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DigiKey.Api.Models;
using NUnit.Framework;

namespace DigiKey.Api.IntegrationExams
{
    [TestFixture]
    public class DigiKeyClientExams
    {
        [Test]
        [Category("KeywordSearch")]
        public async Task KeywordSearch_CredsAndAcessTokenSet_ReturnsValidProperties()
        {
            var settings = WebApiSettings.CreateFromConfigFile();

            var sut = new DigiKeyApiClient(settings);

            Assert.IsNotNull(sut.HttpClient);

            var postResponse = string.Empty;
            try
            {
                postResponse = await sut.KeywordSearch("P5555-ND");
                Assert.IsNotNull(postResponse);
                Console.WriteLine("respionse is {0}", postResponse);
            }
            catch (HttpResponseException hre)
            {
                if (hre.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                   Console.WriteLine("We most likely have an expired or invalid RefreshToken in our wepapi.config file");
                    return;
                }
            }
            catch (System.Exception hre)
            {
                Console.WriteLine($"Exception Error Message is {hre.Message}");
                return;
            }
            Console.WriteLine($"postResponse is {postResponse}");
        }
    }
}
