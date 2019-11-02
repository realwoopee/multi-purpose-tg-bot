using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordCounterBot.DAL.Contracts
{
    public interface IUserDao
    {
        Task UpdateUser(User user);

        Task<User> GetUserById(long userId);
    }
}