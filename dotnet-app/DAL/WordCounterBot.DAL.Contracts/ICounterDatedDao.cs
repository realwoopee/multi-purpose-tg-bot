using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Contracts
{
    public interface ICounterDatedDao
    {
        Task AddCounter(long chatId, long userId, DateTime date);

        Task AddCounter(long chatId, long userId, DateTime date, long counts);

        Task IncrementCounter(long chatId, long userId, DateTime date);

        Task IncrementCounter(long chatId, long userId, DateTime date, long counts);

        Task<CounterDated> GetCounter(long chatId, long userId, DateTime date);

        Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit);

        Task<List<CounterDated>> GetCounters(long chatId, long userId, TimeSpan dateLimit);

        Task<bool> CheckCounter(long chatId, long userId, DateTime date);
    }
}
