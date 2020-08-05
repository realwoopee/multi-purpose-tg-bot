using System.Threading.Tasks;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Contracts
{
    public interface IUserDao
    {
        Task UpdateUser(User user);

        Task<User> GetUserById(long userId);
    }
}