using System.Collections.Generic;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Contracts
{
    public interface ICounterDao
    {
        /// <summary>
        /// Gets top N counters for all time from specified chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<Counter>> GetCountersWithLimit(long chatId, int limit);

        /// <summary>
        /// Tries to create a new counter with specified counts, if it already exists, updates existing one.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="userId"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        Task UpdateElseCreateCounter(long chatId, long userId, long counts);

        /// <summary>
        /// Gets counter for user from specified chat for all time
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<Counter> GetPersonalCounter(long chatId, long userId);
    }
}