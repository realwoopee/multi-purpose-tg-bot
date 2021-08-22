Multi-purpose Telegram Bot
======
![](https://img.shields.io/github/v/release/admiralwoop/multi-purpose-tg-bot?include_prereleases&sort=semver)
[![](https://img.shields.io/badge/docker%20hub-099cec)](https://hub.docker.com/r/admiralwoop/multi-purpose-tg-bot)

![](https://img.shields.io/github/last-commit/admiralWoop/multi-purpose-tg-bot/master)
[![](https://www.codefactor.io/repository/github/admiralwoop/multi-purpose-tg-bot/badge)](https://www.codefactor.io/repository/github/admiralwoop/multi-purpose-tg-bot)

**Multi-purpose Telegram Bot** is a telegram bot based on ASP.NET Core & [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot).

The main idea of this bot is use of filter-handler architecture similar to ASP.NET routes and controllers.

That way you don't need to put all your logic in a single controller.

What you have is a single router and many filters that decide what handler will with a received message. Thus handlers have only one responsibility - get some information from the message and do something based on that info.

### Third party libraries
* [Telegram.Bot library](https://github.com/TelegramBots/Telegram.Bot)

### License 
* [MIT License](https://github.com/admiralWoop/multi-purpose-tg-bot/blob/master/LICENSE)
