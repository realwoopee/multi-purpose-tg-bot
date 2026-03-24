using System.Text.RegularExpressions;

namespace WordCounterBot.BLL.Common.Helpers
{
    public static class WordCounterUtil
    {
        public static int CountWords(string text)
        {
            var regex = new Regex(@"\w+", RegexOptions.Compiled);

            var matchesCount = regex.Matches(text).Count;

            return matchesCount;
        }
    }
}
