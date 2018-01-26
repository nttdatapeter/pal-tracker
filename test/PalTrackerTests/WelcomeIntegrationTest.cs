using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class WelcomeIntegrationTest
    {
        private readonly HttpClient _testClient;

        public WelcomeIntegrationTest()
        {
            Environment.SetEnvironmentVariable("WELCOME_MESSAGE", "hello from integration test");
            _testClient = IntegrationTestServer.Start().CreateClient();
        }

        [Fact]
        public async Task ReturnsMessage()
        {
            var response = await _testClient.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var expectedResponse = "hello from integration test";
            var actualResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}