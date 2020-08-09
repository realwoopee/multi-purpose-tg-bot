using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Memory
{
    public class CounterDatedDao : ICounterDatedDao
    {
        private readonly List<CounterDated> _counters = new List<CounterDated>();

        private Task Add(long chatId, long userId, long value, DateTime date)
        {
            _counters.Add(new CounterDated{ Date = date, ChatId = chatId, UserId =  userId, Value = value });
            return Task.CompletedTask;
        }

        private Task Increment(long chatId, long userId, long value, DateTime date)
        { 
            _counters
                .First(counter => 
                    counter.ChatId == chatId 
                    && counter.UserId == userId
                    && counter.Date == date)
                .Value += value;
            return Task.CompletedTask;
        }
        public Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit)
        {
            var values = _counters
                .Where(c => c.ChatId == chatId && c.Date == date.Date)
                .OrderByDescending(c => c.Value)
                .Take(userLimit)
                .ToList();

            return Task.FromResult(values);
        }

        public Task<List<CounterDated>> GetCounters(long chatId, DateTime dateFrom, TimeSpan dateLimit, int userLimit)
        {
            var values = _counters
                .Where(c => c.ChatId == chatId && c.Date >= dateFrom.Date - dateLimit && c.Date <= dateFrom)
                .OrderByDescending(c => c.Date)
                .Take(userLimit * (int)dateLimit.TotalDays)
                .ToList();

            return Task.FromResult(values);
        }

        public async Task UpdateElseCreateCounter(long chatId, long userId, DateTime date, long counts)
        {
            if (_counters.Any(c => c.ChatId == chatId && c.UserId == userId && c.Date == date.Date))
            {
                await Increment(chatId, userId, counts, date.Date);
            }
            else
            {
                await Add(chatId, userId, counts, date.Date);
            }
        }
    }
}
