using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.DAL.Contracts
{
    public interface ICounterDatedDao
    {
        Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit);

        Task<List<CounterDated>> GetCounters(long chatId, DateTime startDate, DateTime endDate, int userLimit);

        Task UpdateElseCreateCounter(long chatId, long userId, DateTime date, long counts);
        
        Task<List<CounterDated>> GetPersonalCounters(long chatId, long userId, DateTime startDate, DateTime endDate);
        
        Task<List<CounterDated>> GetPersonalCounters(long chatId, long userId, DateTime date);
        Task<CounterDated> GetPersonalLastCounter(long chatId, long userId);
    }
}
