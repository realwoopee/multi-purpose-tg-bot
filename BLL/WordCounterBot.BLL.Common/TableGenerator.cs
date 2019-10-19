using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCounterBot.BLL.Common
{
    public static class TableGenerator
    {
        public static string GenerateTable(
            string column1Name,
            string column2Name,
            IEnumerable<(object, object)> values,
            int column1WidthLimit = 24,
            int column2WidthLimit = 19)
        {
            var rows = values.Select(v => (v.Item1.ToString(), v.Item2.ToString()));

            var col1Width = Math.Min(
                Math.Max(rows.Max(v => v.Item1.Length), column1Name.Length), 
                column1WidthLimit);

            var col2Width = Math.Min(
                Math.Max(rows.Max(v => v.Item2.Length),column2Name.Length), 
                column2WidthLimit);

            var sumWidth = col1Width + 1 + col2Width;

            StringBuilder table = new StringBuilder();

            var headers = GenerateRow(
                column1Name, col1Width,
                column2Name, col2Width);

            table.AppendLine(headers);

            table.AppendLine(new string('-', sumWidth));

            foreach (var (item1, item2) in rows)
            {
                var rowStr = GenerateRow(
                    Constrain(item1, col1Width), col1Width,
                    Constrain(item2, col2Width), col2Width);
                table.AppendLine(rowStr);
            }

            return table.ToString();
        }

        private static string GenerateRow(string col1, int col1Width, string col2, int col2Width) =>
            col1 + new string(' ', col1Width - col1.Length) + "|" + new string(' ', col2Width - col2.Length) + col2;

        private static string Constrain(string str, int limit, string tail = "...")
        {
            if (tail == null) throw new ArgumentNullException(nameof(tail));

            if (str.Length <= limit)
            {
                return str;
            }

            return str.Substring(0, Math.Min(str.Length - 1 , limit - tail.Length - 1)) + tail;
        }
    }
}
