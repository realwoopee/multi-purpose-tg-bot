using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Contracts
{
    public interface ICommand
    {
        string Name { get; }
        Task Execute(Update update, string command, params string[] args);
    }
}
