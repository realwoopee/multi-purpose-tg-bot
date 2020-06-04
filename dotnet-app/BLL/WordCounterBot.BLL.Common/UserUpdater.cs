using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Common
{
    public class UserUpdater
    {
        private readonly IUserDao _userDao;
        public UserUpdater(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public async Task Update(User user)
        {
            if (user == null) return;
            await _userDao.UpdateUser(user);
        }
    }
}
