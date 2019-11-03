Multi-purpose Telegram Bot
======

![travis-ci](https://travis-ci.com/admiralWoop/multi-purpose-tg-bot.svg?branch=dev)

**Multi-purpose Telegram Bot** is a telegram bot inside a ASP.NET Core app.

The main idea of this bot is use of a architecture where bot uses filters and corresponding handlers to handle updates received from Telegram.

That way you don't need to have single handler that handles all kinds of messages.
What you have is **the** router and many filters that decide what handler will handle new received message. Thus handlers have only one responsibility - get some information from a message and do something with it.

### Third party libraries
* [Telegram.Bot library](https://github.com/TelegramBots/Telegram.Bot)

### License 
* [MIT License](https://github.com/admiralWoop/multi-purpose-tg-bot/blob/master/LICENSE)
