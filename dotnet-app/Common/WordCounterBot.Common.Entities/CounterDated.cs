using System;

namespace WordCounterBot.Common.Entities
{
    public record CounterDated(DateTime Date, long ChatId, long UserId, long Value)
    {
        private readonly long _value = Value < 0 ? throw new ArgumentException("Value cannot be negative") : Value;

        public long Value 
        { 
            get => _value; 
            init => _value = value < 0 ? throw new ArgumentException("Value cannot be negative") : value; 
        }
        
        public bool Matches(long chatId, long userId, DateTime date) => 
            ChatId == chatId && UserId == userId && Date.Date == date.Date;

        public bool IsInChatOnDate(long chatId, DateTime date) => 
            ChatId == chatId && Date.Date == date.Date;

        public bool IsInChatInRange(long chatId, DateTime start, DateTime end) => 
            ChatId == chatId && Date.Date >= start.Date && Date.Date <= end.Date;

    }
}
