namespace SevenWestTest.Api.Infrastructure.Formatters
{
    public interface IDataFormatter
    {
        T Format<T>(string value);
    }
}
