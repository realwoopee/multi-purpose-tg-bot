using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
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

        public Task<bool> IsHandleable(Update update, HandleContext context) =>
            Task.FromResult(true);

        public async Task<bool> HandleUpdate(Update update, HandleContext context)
        {
            var user = update.Message?.From;

            if (user == null)
                return false;

            await _userDao.UpdateUser(new WordCounterBot.Common.Entities.User(user));
            return true;
        }
    }
}
