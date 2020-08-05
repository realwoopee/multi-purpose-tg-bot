using System;
using System.Collections.Generic;
using System.Text;

namespace WordCounterBot.Common.Entities
{
    public class CounterDated
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public long Value { get; set; }
    }
}
