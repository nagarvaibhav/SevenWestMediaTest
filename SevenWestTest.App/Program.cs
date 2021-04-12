using Newtonsoft.Json;
using SevenWestTest.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SevenWestTest.App
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var users = await GetUsers();
            var idToGetFullname = 42;
            var ageToGetfirstNames = 23;

            var user = users.FirstOrDefault(x => x.Id == idToGetFullname);
            if (user != null)
            {
                Console.WriteLine($"User Full Name with id {idToGetFullname} is: {user.First} {user.Last}");
            }
            else
            {
                Console.WriteLine($"User Full Name with id {idToGetFullname} does not exists");
            }

            Console.WriteLine($"All Users First Name with Age {ageToGetfirstNames} { string.Join(',', users.Where(x => x.Age == ageToGetfirstNames).Select(x => x.First))}");

            var groupedUser = users.GroupBy(x => x.Age).OrderBy(x => x.Key).Select(x => new
            {
                Age = x.Key,
                Mcount = x.Count(m => m.Gender.Equals(Genders.M.ToString() ,StringComparison.CurrentCultureIgnoreCase)),
                FCount = x.Count(m => m.Gender.Equals(Genders.F.ToString(), StringComparison.CurrentCultureIgnoreCase)),
                TCount = x.Count(m => m.Gender.Equals(Genders.T.ToString(), StringComparison.CurrentCultureIgnoreCase)),
                YCount = x.Count(m => m.Gender.Equals(Genders.Y.ToString(), StringComparison.CurrentCultureIgnoreCase)),
            });

            foreach (var item in groupedUser)
            {
                Console.WriteLine($"Age: {item.Age} Female: {item.FCount} Male: {item.Mcount} Trans: {item.TCount} Y: {item.YCount}");
            }
            Console.ReadKey();
        }



        private async static Task<IEnumerable<User>> GetUsers()
        {
            var response = await client.GetStringAsync("https://localhost:5001/api/user");
            return JsonConvert.DeserializeObject<IEnumerable<User>>(response);
        }
    }
}
