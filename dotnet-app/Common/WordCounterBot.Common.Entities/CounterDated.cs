using System;

namespace WordCounterBot.Common.Entities
{
    public class CounterDated
    {
        public DateTime Date { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public long Value { get; set; }
    }
}
