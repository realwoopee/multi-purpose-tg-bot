using System.Diagnostics.CodeAnalysis;

namespace WordCounterBot.BLL.Common
{
    public static class StringHelper
    {
        public static string Escape([NotNull]this string input)
        {
            return input
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("'", "&quot;");
        }
    }
}
