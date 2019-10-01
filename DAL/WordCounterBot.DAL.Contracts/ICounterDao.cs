using System.Threading.Tasks;

namespace WordCounterBot.DAL.Contracts
{
    public interface ICounterDao
    {
        Task AddCounter(long chatId, long userId);
        Task AddCounter(long chatId, long userId, long counts);
        Task IncrementCounter(long chatId, long userId);
        Task IncrementCounter(long chatId, long userId, long counts);
        Task ResetCounter(long chatId, long userId);
        Task<long> GetCounter(long chatId, long userId);
        Task<bool> CheckCounter(long chatId, long userId);
    }
}