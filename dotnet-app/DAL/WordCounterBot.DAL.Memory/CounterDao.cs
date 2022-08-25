using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Memory
{
    public class CounterDao : ICounterDao
    {
        private readonly List<Counter> _counters = new List<Counter>();

        private Task Add(long chatId, long userId, long value)
        {
            _counters.Add(
                new Counter
                {
                    ChatId = chatId,
                    UserId = userId,
                    Value = value
                }
            );
            return Task.CompletedTask;
        }

        private Task Increment(long chatId, long userId, long value)
        {
            _counters
                .First(counter => counter.ChatId == chatId && counter.UserId == userId)
                .Value += value;
            return Task.CompletedTask;
        }

        public async Task UpdateElseCreateCounter(long chatId, long userId, long counts)
        {
            if (_counters.Any(c => c.ChatId == chatId && c.UserId == userId))
            {
                await Increment(chatId, userId, counts);
            }
            else
            {
                await Add(chatId, userId, counts);
            }
        }

        public async Task<Counter> GetPersonalCounter(long chatId, long userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> GetTotalCount()
        {
            return _counters.Sum(c => c.Value);
        }

        public Task<List<Counter>> GetCountersWithLimit(long chatId, int limit = 10)
        {
            var values = _counters
                .Where(c => c.ChatId == chatId)
                .OrderByDescending(c => c.Value)
                .Take(limit)
                .ToList();

            return Task.FromResult(values);
        }
    }
}
