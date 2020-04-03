using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.DAL.Memory
{
    internal class Counter
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public long Value { get; set; }

        public Counter(long chatId, long userId, long value)
        {
            this.ChatId = chatId;
            this.UserId = userId;
            this.Value = value;
        }
    }

    public class CounterDao : ICounterDao
    {
        private List<Counter> _counters = new List<Counter>();

        private Task Add(long chatId, long userId, long value)
        {
            _counters.Add(new Counter(chatId, userId, value));
            return Task.CompletedTask;
        }

        private Task Increment(long chatId, long userId, long value)
        { 
            _counters
                .First(counter => 
                    counter.ChatId == chatId 
                    && counter.UserId == userId)
                .Value += value;
            return Task.CompletedTask;
        }

        public async Task AddCounter(long chatId, long userId) => 
            await Add(chatId, userId, 0);
        
        public async Task AddCounter(long chatId, long userId, long value) => 
            await Add(chatId, userId, value);

        public Task IncrementCounter(long chatId, long userId)
            => Increment(chatId, userId, 1);

        public Task IncrementCounter(long chatId, long userId, long value)
            => Increment(chatId, userId, value);

        public Task ResetCounter(long chatId, long userId)
        {
            var counter = _counters
                .First(c => 
                    c.ChatId == chatId 
                    && c.UserId == userId);
            counter.Value = 0;
            return Task.CompletedTask;
        }

        public Task<long> GetCounter(long chatId, long userId)
        {
            var counter = _counters
                .First(c => 
                    c.ChatId == chatId 
                    && c.UserId == userId);
            return Task.FromResult(counter.Value);
        }

        public Task<List<(long userId, long counter)>> GetCounters(long chatId)
        {
            var values = _counters
                .Where(c => c.ChatId == chatId)
                .Select(c => (c.UserId, c.Value))
                .ToList();
            return Task.FromResult(values);
        }

        public Task<List<(long userId, long counter)>> GetCountersWithLimit(long chatId, int limit)
        {
            var values = _counters
                .Where(c => c.ChatId == chatId)
                .Select(c => (c.UserId, c.Value))
                .Take(limit)
                .ToList();
            return Task.FromResult(values);
        }

        public Task<bool> CheckCounter(long chatId, long userId)
        {
            var exists = _counters
                .Any(c => c.ChatId == chatId && c.UserId == userId);
            return Task.FromResult(exists);
        }
    }
}
