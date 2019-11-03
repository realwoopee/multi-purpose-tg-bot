using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class SystemMessageHandler : IHandler
    {
        private TelegramBotClient _client;

        private static readonly MessageType[] _allowedMessageTypes = {
            MessageType.ChatMemberLeft, MessageType.ChatMembersAdded, MessageType.ChannelCreated,
            MessageType.ChatTitleChanged, MessageType.MessagePinned, MessageType.MigratedFromGroup,
            MessageType.MigratedToSupergroup, MessageType.ChatPhotoChanged, MessageType.ChatPhotoDeleted,
            MessageType.GroupCreated, MessageType.SupergroupCreated
        };

        public SystemMessageHandler(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task<bool> Predicate(Update update) =>
            await Task.Run(() => update.Message != null && _allowedMessageTypes.Contains(update.Message.Type));

        public async Task HandleUpdate(Update update)
        {
            switch (update.Message.Type)
            {
                case MessageType.ChatMembersAdded:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Представься. Особо молчаливых кикаем наххой.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatMemberLeft:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Давай, пиздуй-бороздуй.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatTitleChanged:
                case MessageType.ChatPhotoChanged:
                case MessageType.MessagePinned:
                case MessageType.ChatPhotoDeleted:
                case MessageType.GroupCreated:
                case MessageType.SupergroupCreated:
                case MessageType.ChannelCreated:
                case MessageType.MigratedToSupergroup:
                case MessageType.MigratedFromGroup:
                    await Task.CompletedTask;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
