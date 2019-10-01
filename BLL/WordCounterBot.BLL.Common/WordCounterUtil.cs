using System;
using System.Linq;

namespace WordCounterBot.BLL.Common
{
    public class WordCounterUtil
    {
        public int CountWords(string text)
        {
            return text.Split(' ')
                .Where(s => !string.IsNullOrEmpty(s))
                .Count();
        }
    }
}
