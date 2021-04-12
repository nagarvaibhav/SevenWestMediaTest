using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SevenWestTest.Api.Infrastructure.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Providers
{
    public class HttpDataProvider : IDataProvider
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly UserApiConfig _userApiConfig;
        private readonly ILogger<HttpDataProvider> _logger;
        private readonly IMemoryCache _cache;
        private readonly string CahceKey = "AllUsers";

        public HttpDataProvider(IHttpClientFactory clientFactory,
            ILogger<HttpDataProvider> logger,
            IOptions<UserApiConfig> userApiConfig,
            IMemoryCache cache)
        {
            _clientFactory = clientFactory;
            _userApiConfig = userApiConfig.Value;
            _logger = logger;
            _cache = cache;
        }

        public async Task<string> GetAllUsers()
        {
            try
            {
                var result = string.Empty;
                if (!_cache.TryGetValue(CahceKey, out result))
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_userApiConfig.CacheTTLInSeconds));

                    using (var client = _clientFactory.CreateClient())
                    {
                        var baseUri = new Uri(_userApiConfig.BaseUrl);
                        var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(baseUri, _userApiConfig.UserEndpoint));
                        var httpResponse = await client.SendAsync(httpRequest);
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var response =  await httpResponse.Content.ReadAsStringAsync();
                            _cache.Set(CahceKey, response, cacheEntryOptions);
                            return response;
                        }
                        _logger.LogCritical("User Service returned unsuccessful response with status code:" + httpResponse.StatusCode);
                        return string.Empty;
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in calling User Service");
                throw ex;
            }
        }
    }
}
