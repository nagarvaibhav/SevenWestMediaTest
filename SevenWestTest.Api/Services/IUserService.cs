using SevenWestTest.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
    }   
}
