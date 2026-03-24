using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class SystemMessageHandler : IHandler
    {
        private readonly MessageSender _messageSender;

        private static readonly MessageType[] AllowedMessageTypes =
        {
            MessageType.ChatMemberLeft,
            MessageType.ChatMembersAdded,
            MessageType.ChannelCreated,
            MessageType.ChatTitleChanged,
            MessageType.MessagePinned,
            MessageType.MigratedFromGroup,
            MessageType.MigratedToSupergroup,
            MessageType.ChatPhotoChanged,
            MessageType.ChatPhotoDeleted,
            MessageType.GroupCreated,
            MessageType.SupergroupCreated
        };

        private static readonly Dictionary<MessageType, string> _messageMap = new()
        {
            [MessageType.ChatMembersAdded] = "Представься. Особо молчаливых кикаем.",
            [MessageType.ChatMemberLeft] = "Ну и вали.",
            [MessageType.ChatTitleChanged] = "Прошлое название было лучше!",
            [MessageType.ChatPhotoChanged] = "Верните старую.",
            [MessageType.MessagePinned] = "Надоели эти пины с нотифаем."
        };

        public SystemMessageHandler(MessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task<bool> IsHandleable(Update update, HandleContext context) =>
            await Task.Run(() => update.Message != null && update.Message.Type.In(AllowedMessageTypes)
            );

        public async Task<bool> HandleUpdate(Update update, HandleContext context)
        {
            if (!_messageMap.TryGetValue(update.Message.Type, out var responseText))
                return false;

            await _messageSender.SendHtmlReplyAsync(update, responseText);

            return true;
        }
    }
}