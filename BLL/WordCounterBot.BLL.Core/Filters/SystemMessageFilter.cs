using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Filters
{
    public class SystemMessageFilter : IFilter
    {
        private static readonly MessageType[] _allowed = {
            MessageType.ChatMemberLeft, MessageType.ChatMembersAdded, MessageType.ChannelCreated,
            MessageType.ChatTitleChanged, MessageType.MessagePinned, MessageType.MigratedFromGroup,
            MessageType.MigratedToSupergroup, MessageType.ChatPhotoChanged, MessageType.ChatPhotoDeleted,
            MessageType.GroupCreated, MessageType.SupergroupCreated
        };

        public bool Predicate(Update update) => update.Message != null && _allowed.Contains(update.Message.Type);
    }
}
