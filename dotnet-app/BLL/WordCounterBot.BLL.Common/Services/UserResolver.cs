using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.DAL.Contracts;
using User = WordCounterBot.Common.Entities.User;

namespace WordCounterBot.BLL.Common.Services;

public class UserResolver
{
    private readonly IUserDao _userDao;

    public UserResolver(IUserDao userDao)
    {
        _userDao = userDao;
    }

    /// <summary>
    /// Tries to get user from in order:
    /// 1. Username in the first argument
    /// 2. User that was replied to in the update
    /// 3. User that sent the message in the update
    /// </summary>
    public async Task<User> ResolveTargetUser(string[] args, Update update)
    {
        if (args.Length > 0)
        {
            return await _userDao.GetUserByUserName(args[0].TrimStart('@'));
        }

        var targetUserId = update.GetReplySenderId() ?? update.GetSenderId();
        return await _userDao.GetUserById(targetUserId);
    }
}