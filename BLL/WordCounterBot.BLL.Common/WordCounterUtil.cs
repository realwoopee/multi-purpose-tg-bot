using System.Linq;

namespace WordCounterBot.BLL.Common
{
    public static class WordCounterUtil
    {
        public static int CountWords(string text)
        {
            return text
                .Split(' ')
                .Count(s => !string.IsNullOrEmpty(s));
        }
    }
}
