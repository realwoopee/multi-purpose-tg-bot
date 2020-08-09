using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCounterBot.BLL.Common
{
    public static class TableGenerator
    {
        public static string GenerateNumberedList(
            IEnumerable<(object, object)> values)
        {
            var rows = values.Select(v => new {User = v.Item1.ToString(), Counter = v.Item2.ToString()}).ToList();

            var table = new StringBuilder();

            for (var i = 0; i < rows.Count(); i++)
            {
                var element = rows.ElementAt(i);
                var rowStr = $"<b>0x{i:X}</b> {element.User} — 0d{element.Counter} <i>words</i>.";
                table.AppendLine(rowStr);
            }

            return table.ToString();
        }
    }
}
