using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCounterBot.BLL.Common
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
                var rowStr = $"<b>{i + 1})</b> {element.User} — {element.Counter} <i>words</i>.";
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
