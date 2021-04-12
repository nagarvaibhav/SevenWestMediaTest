using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using SevenWestTest.Api.Infrastructure.Options;
using SevenWestTest.Api.Providers;
using SevenWestTest.Api.Tests.Mocks;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Tests.Providers
{
    [TestFixture]
    public class HttpDataProviderTests
    {
        private IHttpClientFactory _clientFactory;
        private IOptions<UserApiConfig> _userApiConfig;
        private ILogger<HttpDataProvider> _logger;
        private IMemoryCache _cache;
        private HttpDataProvider _httpDataProvider;

        [SetUp]
        public void SetUp()
        {
            _clientFactory = Substitute.For<IHttpClientFactory>();
            _userApiConfig = Options.Create(new UserApiConfig
            {
                BaseUrl = "http://test.com",
                UserEndpoint = "test",
                CacheTTLInSeconds = 1
            });

            _logger = Substitute.For<ILogger<HttpDataProvider>>();
            _cache = Substitute.For<IMemoryCache>();
            _httpDataProvider = new HttpDataProvider(_clientFactory, _logger, _userApiConfig, _cache);
        }

        [Test]
        public async Task GetAllUser_Should_Return_Users_Succesfully_If_Cache_DoesNot_Exists()
        {
            _cache.TryGetValue("AllUsers", out Arg.Any<string>()).Returns(false);

            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MockDataHelper.UsersResponse, Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            _clientFactory.CreateClient().Returns(fakeHttpClient);

            var result = await _httpDataProvider.GetAllUsers();
            Assert.IsNotNull(result);
            Assert.AreEqual(result, MockDataHelper.UsersResponse);
        }

        [Test]
        public async Task GetAllUser_Should_Return_Empty_Result_When_UserAPI_Returns_UnSuccessfull_Response()
        {
            _cache.TryGetValue("AllUsers", out Arg.Any<string>()).Returns(false);

            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            _clientFactory.CreateClient().Returns(fakeHttpClient);

            var result = await _httpDataProvider.GetAllUsers();
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllUser_Should_Throw_Exception_ForAnyException()
        {
            _cache.TryGetValue("AllUsers", out Arg.Any<string>()).Returns(false);

            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            _clientFactory.When(x => x.CreateClient()).Do(x => { throw new Exception(); });

            Assert.ThrowsAsync<Exception>(async () => await _httpDataProvider.GetAllUsers());
        }
    }
}
