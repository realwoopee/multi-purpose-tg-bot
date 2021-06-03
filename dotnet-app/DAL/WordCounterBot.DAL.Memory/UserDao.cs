using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Memory
{
    public class UserDaoMemory : IUserDao
    {
        private readonly List<User> _users = new List<User>();

        public Task UpdateUser(User user)
        {
            if (_users.All(u => u.Id != user.Id))
            {
                _users.Add(user);
            }

            return Task.CompletedTask;
        }

        async Task<User> IUserDao.GetUserById(long userId)
        {
            return await GetUserById(userId);
        }

        public Task<User> GetUserById(long userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);

            return Task.FromResult(user);
        }

        public Task<User> GetUserByUserName(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            return Task.FromResult(user);
        }
    }
}