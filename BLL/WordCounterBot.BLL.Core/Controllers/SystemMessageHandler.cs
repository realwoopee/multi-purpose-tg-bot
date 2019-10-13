using System;
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
        public SystemMessageHandler(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task HandleUpdate(Update update)
        {
            switch (update.Message.Type)
            {
                case MessageType.ChatMembersAdded:
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, @"Представься.",
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
