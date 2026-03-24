using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCounterBot.BLL.Common.Helpers
{
    public static class TableGenerator
    {
        public static string GenerateNumberedList(IEnumerable<(object, object)> values)
        {
            var rows = values
                .Select(v => new { User = v.Item1.ToString(), Counter = v.Item2.ToString() })
                .ToList();

            var table = new StringBuilder();

            for (var i = 0; i < rows.Count(); i++)
            {
                var element = rows.ElementAt(i);
                var rowStr = $"{$"{i + 1})".HtmlBold()} {element.User} — {element.Counter} {"words".HtmlItalic()}.";
                table.AppendLine(rowStr);
            }

            return table.ToString();
        }

        public static string GenerateTop(
            string message,
            IEnumerable<(string Username, long Counter)> users
        )
        {
            var text = new StringBuilder();

            var values = users.ToList();

            text.AppendLine(message);

            var table = GenerateNumberedList(
                values
                    .OrderByDescending(uc => uc.Counter)
                    .Select(uc => ((object)uc.Username, (object)uc.Counter))
            );

            text.AppendLine(table);

            return text.ToString();
        }
    }
}
