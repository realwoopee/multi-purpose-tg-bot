using System.Diagnostics.CodeAnalysis;

namespace WordCounterBot.BLL.Common.Helpers
{
    public static class HtmlHelper
    {
        public static string HtmlEscape([NotNull] this string input)
        {
            return input
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        public static string HtmlBold([NotNull] this string input)
        {
            return $"<b>{input}</b>";
        }

        public static string HtmlItalic([NotNull] this string input)
        {
            return $"<i>{input}</i>";
        }

        public static string HtmlCode([NotNull] this string input)
        {
            return $"<code>{input}</code>";
        }

        public static string HtmlPre([NotNull] this string input)
        {
            return $"<pre>{input}</pre>";
        }

        public static string HtmlLink([NotNull] this string text, [NotNull] string url)
        {
            return $"<a href=\"{url}\">{text}</a>";
        }
    }
}
