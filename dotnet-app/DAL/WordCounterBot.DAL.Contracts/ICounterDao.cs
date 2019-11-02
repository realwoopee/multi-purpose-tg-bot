using System.Collections.Generic;
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

        /// <summary>
        /// Get counters from specified chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Task<List<(long userId, long counter)>> GetCounters(long chatId);

        /// <summary>
        /// Get top N counters from specified chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<(long userId, long counter)>> GetCountersWithLimit(long chatId, int limit);

        Task<bool> CheckCounter(long chatId, long userId);
    }
}