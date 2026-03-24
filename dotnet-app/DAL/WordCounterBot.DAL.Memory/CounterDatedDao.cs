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
            _counters.Add(
                new CounterDated(date, chatId, userId, value)
            );
            return Task.CompletedTask;
        }

        private Task Increment(long chatId, long userId, long value, DateTime date)
        {
            var counter = _counters
                .First(counter =>
                    counter.Matches(chatId, userId, date)
                );
            _counters.Remove(counter);
            _counters.Add(counter with { Value = counter.Value + value });
            return Task.CompletedTask;
        }

        public Task<List<CounterDated>> GetCounters(long chatId, DateTime date, int userLimit)
        {
            var values = _counters
                .Where(c => c.IsInChatOnDate(chatId, date))
                .OrderByDescending(c => c.Value)
                .Take(userLimit)
                .ToList();

            return Task.FromResult(values);
        }

        public Task<List<CounterDated>> GetCounters(
            long chatId,
            DateRange dateRange,
            int userLimit
        )
        {
            var values = _counters
                .Where(c => c.IsInChatInRange(chatId, dateRange.StartDate, dateRange.EndDate)
                )
                .OrderByDescending(c => c.Date)
                .Take(userLimit * dateRange.DaysCount)
                .ToList();

            return Task.FromResult(values);
        }

        public async Task UpdateElseCreateCounter(
            long chatId,
            long userId,
            DateTime date,
            long counts
        )
        {
            if (_counters.Any(c => c.Matches(chatId, userId, date)))
            {
                await Increment(chatId, userId, counts, date.Date);
            }
            else
            {
                await Add(chatId, userId, counts, date.Date);
            }
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateRange dateRange
        )
        {
            throw new NotImplementedException();
        }

        public async Task<List<CounterDated>> GetPersonalCounters(
            long chatId,
            long userId,
            DateTime date
        )
        {
            throw new NotImplementedException();
        }

        public async Task<CounterDated> GetPersonalLastCounter(long chatId, long userId)
        {
            throw new NotImplementedException();
        }
    }
}