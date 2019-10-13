using System.Linq;

namespace WordCounterBot.BLL.Common
{
    public class WordCounterUtil
    {
        public int CountWords(string text)
        {
            return text
                .Split(' ')
                .Count(s => !string.IsNullOrEmpty(s));
        }
    }
}
