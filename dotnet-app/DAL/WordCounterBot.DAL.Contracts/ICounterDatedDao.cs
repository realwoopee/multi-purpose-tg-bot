using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Contracts
{
    public interface ICounterDatedDao
    {
        Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit);

        Task<List<CounterDated>> GetCounters(
            long chatId,
            DateRange dateRange,
            int userLimit
        );

        Task UpdateElseCreateCounter(long chatId, long userId, DateTime date, long counts);

        Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateRange dateRange
        );

        Task<List<CounterDated>> GetPersonalCounters(long chatId, long userId, DateTime date);
        Task<CounterDated> GetPersonalLastCounter(long chatId, long userId);
    }
}
