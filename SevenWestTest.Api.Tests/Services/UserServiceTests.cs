using NSubstitute;
using NUnit.Framework;
using SevenWestTest.Api.Infrastructure.Formatters;
using SevenWestTest.Api.Providers;
using SevenWestTest.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private IDataProvider _dataProvider;

        [SetUp]
        public void SetUp()
        {
            _dataProvider = Substitute.For<IDataProvider>();
            _userService = new UserService(_dataProvider, new JsonFormatter());
        }

        [Test]
        public async Task GetAllUser_Should_CallDataProvider_And_ShouldReturn_Users_Succesfully()
        {
            _dataProvider.GetAllUsers().Returns("[{ 'id': 53, 'first': 'Bill', 'last': 'Bryson', 'age':23, 'gender':'M' }]");
            var result = await _userService.GetAllUsers();
            await _dataProvider.Received(1).GetAllUsers();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var user = result.First();
            Assert.AreEqual(53, user.Id);
            Assert.AreEqual("Bill", user.First);
            Assert.AreEqual("Bryson", user.Last);
        }
    }
}
