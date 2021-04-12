using Newtonsoft.Json;

namespace SevenWestTest.Api.Infrastructure.Formatters
{
    public class JsonFormatter : IDataFormatter
    {
        public T Format<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
