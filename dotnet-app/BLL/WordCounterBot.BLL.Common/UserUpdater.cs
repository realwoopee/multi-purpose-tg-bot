using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.BLL.Common
{
    public class UserUpdater
    {
        private readonly IUserDao _userDao;
        public UserUpdater(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public async Task Update(Telegram.Bot.Types.User user)
        {
            if (user == null) return;
            await _userDao.UpdateUser(new WordCounterBot.Common.Entities.User(user));
        }
    }
}
