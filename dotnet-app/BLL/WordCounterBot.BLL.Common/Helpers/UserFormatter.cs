using WordCounterBot.Common.Entities;

namespace WordCounterBot.BLL.Common.Helpers
{
    public static class UserFormatter
    {
        public static string FormatUserName(User user) => 
            (user?.FullName).HtmlEscape() ?? "%Unknown%";
    }
}