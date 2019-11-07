using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Memory
{

    public class UserDao : IUserDao
    {
        private List<User> _users = new List<User>();
        public Task UpdateUser(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if(existing != null)
            {
                existing = user;
                return Task.CompletedTask;
            }
            else
            {
                _users.Add(user);
            }
            return Task.CompletedTask;
        }

        public Task<Telegram.Bot.Types.User> GetUserById(long userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);

            return Task.FromResult(user);
        }
    }
}