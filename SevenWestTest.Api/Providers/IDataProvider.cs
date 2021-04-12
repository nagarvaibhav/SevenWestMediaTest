using System.Threading.Tasks;

namespace SevenWestTest.Api.Providers
{
    public interface IDataProvider
    {
        Task<string> GetAllUsers();
    }
}
