using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
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

        public async Task<bool> IsHandleable(Update update) =>
            await Task.Run(() => update.Message != null && update.Message.Type.In(_allowedMessageTypes));

        public async Task<bool> HandleUpdate(Update update)
        {
            switch (update.Message.Type)
            {
                case MessageType.ChatMembersAdded:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Представься. Особо молчаливых кикаем.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatMemberLeft:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Ну и вали.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatTitleChanged:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Прошлое название было лучше!",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatPhotoChanged:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Верните старую.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.MessagePinned:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Надоели эти пины с нотифаем.",
                        replyToMessageId: update.Message.MessageId);
                    break;
                case MessageType.ChatPhotoDeleted:
                case MessageType.GroupCreated:
                case MessageType.SupergroupCreated:
                case MessageType.ChannelCreated:
                case MessageType.MigratedToSupergroup:
                case MessageType.MigratedFromGroup:
                default:
                    return false;
                    break;
            }

            return true;
        }
    }
}
