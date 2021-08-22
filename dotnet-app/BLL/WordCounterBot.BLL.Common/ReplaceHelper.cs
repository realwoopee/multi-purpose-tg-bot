using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordCounterBot.BLL.Common
{
    public class ReplaceHelper
    {
        // s/$pattern/$string(/$flags)
        // s\/(?'regex'.*?)\/(?:(?'string'.*?)\/(?'flags'[gi]*))?

        private record Options(string regex, string @string, string flags);

        public static bool IsHandable(string pattern)
        {
            return pattern.StartsWith("s/") && pattern.Count(x => x == '/') >= 2;
        }

        public static string Replace(string input, string pattern)
        {
            if (!IsHandable(pattern)) 
                return input;

            var options = ToOptions(pattern);

            var output = string.Empty;

            var ignoreCase = options.flags.Contains('i');
            var global = options.flags.Contains('g');
            
            var regex = new Regex(options.regex, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
            
            if (global)
            {
                output = regex.Replace(input, options.@string);
            }
            else
            {
                output = regex.Replace(input, options.@string, 1);
            }
            
            return output;
        }

        private static Options ToOptions(string pattern)
        {
            var options = pattern.TrimStart('s', '/').Split('/');
            var regex = options[0];
            var @string = options.Length > 1 ? options[1] : "";
            var flags = options.Length > 2 ? options[2] : "";
            return new Options(regex, @string, flags);
        }
    }
}