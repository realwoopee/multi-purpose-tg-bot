using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
