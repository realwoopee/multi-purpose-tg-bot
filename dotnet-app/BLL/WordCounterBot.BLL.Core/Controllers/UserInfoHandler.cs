﻿using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class UserInfoHandler : IHandler
    {
        private readonly IUserDao _userDao;

        public UserInfoHandler(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public Task<bool> IsHandable(Update update) => Task.FromResult(true);

        public async Task HandleUpdate(Update update)
        {
            var user = update.Message?.From;

            if (user == null) return;

            await _userDao.UpdateUser(new WordCounterBot.Common.Entities.User(user));
        }
    }
}
