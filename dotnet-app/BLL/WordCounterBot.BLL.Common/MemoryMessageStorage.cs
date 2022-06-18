using System;
using Microsoft.Extensions.Caching.Memory;

namespace WordCounterBot.BLL.Common
{
    public class MemoryMessageStorage
    {
        private readonly IMemoryCache _memoryCache;
        
        public MemoryMessageStorage(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddOrUpdate(long messageId, string text)
        {
            var options = new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0)};

            _memoryCache.Set(messageId, text, options);
        }

        public string? TryGetText(long messageId)
        {
            _memoryCache.TryGetValue<string?>(messageId, out var text);
            return text;
        }
    }
}