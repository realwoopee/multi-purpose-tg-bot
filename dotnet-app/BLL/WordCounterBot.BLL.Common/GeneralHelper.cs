using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WordCounterBot.BLL.Common
{
    public static class GeneralHelper
    {
        public static bool In<T>([NotNull] this T element, IEnumerable<T> enumerable)
        {
            return enumerable?.Contains(element) ?? false;
        }
    }
}
