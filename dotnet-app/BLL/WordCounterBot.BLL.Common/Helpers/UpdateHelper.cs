using System;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Common.Helpers
{
    public static class UpdateHelper
    {
        public static bool IsNotForwarded(this Message message)
        {
            return message?.ForwardFrom == null && message?.ForwardFromChat == null;
        }

        public static long GetChatId(this Update update)
        {
            return update.GetActualMessage()?.Chat.Id ?? throw new InvalidOperationException($"Update type {update.Type} does not contain a Chat ID.");
        }
        
        public static DateTime GetMessageDate(this Update update)
        {
            return update.GetActualMessage()?.Date.Date ?? throw new InvalidOperationException($"Update type {update.Type} does not contain a message date.");
        }

        public static long GetSenderId(this Update update)
        {
            return update.GetActualMessage()?.From?.Id ?? throw new InvalidOperationException($"Update type {update.Type} does not contain a sender id.");
        }

        public static long? GetReplySenderId(this Update update)
        {
            return update.GetActualMessage()?.ReplyToMessage?.From?.Id;
        }
        
        public static string GetText(this Message message) =>
            message?.Text ?? message?.Caption;

        public static string GetReplyText(this Update update) =>
            update.Message?.ReplyToMessage?.GetText() ?? 
            update.EditedMessage?.ReplyToMessage?.GetText();

        public static Message GetActualMessage(this Update update) => update.Message ?? update.EditedMessage;
        
        public static int GetMessageId(this Update update)
        {
            return update.GetActualMessage()?.MessageId ?? throw new InvalidOperationException($"Update type {update.Type} does not contain a message id.");;
        }
    }
}