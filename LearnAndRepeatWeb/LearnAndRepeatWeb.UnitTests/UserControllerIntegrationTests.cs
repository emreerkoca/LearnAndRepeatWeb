using LearnAndRepeatWeb.Api;
using LearnAndRepeatWeb.Contracts.Requests.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearnAndRepeatWeb.UnitTests
{
    public class UserControllerIntegrationTests
    {
        protected TestServer _testServer;
        private readonly HttpClient _httpClient;

        public UserControllerIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _testServer = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseEnvironment("Test")
                .UseStartup<Startup>());

            _httpClient = _testServer.CreateClient();
        }

        [Fact]
        public async Task PostUser_WhenPostValidData_ReturnsOK()
        {
            var serializedObject = JsonConvert.SerializeObject(new PostUserRequest
            { 
                FirstName = "test",
                LastName = "test1",
                Email = "sampleuser@mail.com",
                Password = "123456Ee",
                PasswordConfirmation = "123456Ee"
            });
            var data = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/User", data);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostUser_WhenPostNotValidData_ReturnsBadRequest()
        {
            var serializedObject = JsonConvert.SerializeObject(new PostUserRequest
            {
                FirstName = "test",
                LastName = "test1",
                Email = "email is not valid",
                Password = "123456Ee",
                PasswordConfirmation = "123456Ee"
            });
            var data = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/User", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
