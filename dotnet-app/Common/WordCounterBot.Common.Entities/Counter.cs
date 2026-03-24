using System;

namespace WordCounterBot.Common.Entities
{
    public record Counter(long ChatId, long UserId, long Value)
    {
        private readonly long _value = Value < 0 ? throw new ArgumentException("Value cannot be negative") : Value;

        public long Value 
        { 
            get => _value; 
            init => _value = value < 0 ? throw new ArgumentException("Value cannot be negative") : value; 
        }
        
        public bool Matches(long chatId, long userId) => 
            ChatId == chatId && UserId == userId;
    }
}
