using SevenWestTest.Api.Infrastructure.Formatters;
using SevenWestTest.Api.Providers;
using SevenWestTest.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataFormatter _dataFormatter;

        public UserService(IDataProvider dataProvider, IDataFormatter dataFormatter)
        {
            _dataProvider = dataProvider;
            _dataFormatter = dataFormatter;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _dataProvider.GetAllUsers();
            return _dataFormatter.Format<IEnumerable<User>>(users);
        }
    }
}
