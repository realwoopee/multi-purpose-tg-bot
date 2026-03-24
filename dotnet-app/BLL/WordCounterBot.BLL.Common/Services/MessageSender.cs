using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common.Helpers;

namespace WordCounterBot.BLL.Common.Services;

public class MessageSender
{
    private readonly TelegramBotClient _client;

    public MessageSender(TelegramBotClient client)
    {
        _client = client;
    }

    public async Task SendHtmlReplyAsync(Update update, string htmlText)
    {
        await _client.SendTextMessageAsync(
            update.GetChatId(),
            htmlText,
            replyToMessageId: update.GetMessageId(),
            parseMode: ParseMode.Html
        );
    }

    public async Task SendHtmlToChatAsync(long chatId, string htmlText)
    {
        await _client.SendTextMessageAsync(
            chatId,
            htmlText,
            parseMode: ParseMode.Html
        );
    }
    
    public async Task SendReplyAsync(Update update, string text)
    {
        await SendReplyToMessageAsync(update, update.GetMessageId(), text);
    }

    public async Task SendReplyToMessageAsync(Update update, int replyToMessageId, string text)
    {
        await _client.SendTextMessageAsync(
            update.GetChatId(),
            text,
            replyToMessageId: replyToMessageId
        );
    }
}