using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Tests.Mocks
{
    public class MockHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}
