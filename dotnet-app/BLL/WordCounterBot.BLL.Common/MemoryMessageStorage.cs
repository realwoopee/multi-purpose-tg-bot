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

        public void AddOrUpdate(long messageId, int wordCount)
        {
            var options = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = new TimeSpan(0, 20, 0)
            };

            _memoryCache.Set(messageId, wordCount, options);
        }

        public int? TryGetCount(long messageId)
        {
            _memoryCache.TryGetValue<int>(messageId, out var wordCount);
            return wordCount;
        }
    }
}
