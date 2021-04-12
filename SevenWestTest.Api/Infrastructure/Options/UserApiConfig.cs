namespace SevenWestTest.Api.Infrastructure.Options
{
    public class UserApiConfig
    {
        public string BaseUrl { get; set; }
        public string UserEndpoint { get; set; }
        public int CacheTTLInSeconds { get; set; }
    }
}
