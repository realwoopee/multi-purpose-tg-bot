Multi-purpose Telegram Bot
======
![](https://img.shields.io/github/v/release/admiralwoop/multi-purpose-tg-bot?include_prereleases&sort=semver)
![](https://img.shields.io/github/last-commit/admiralWoop/multi-purpose-tg-bot)
[![](https://www.codefactor.io/repository/github/admiralwoop/multi-purpose-tg-bot/badge)](https://www.codefactor.io/repository/github/admiralwoop/multi-purpose-tg-bot)

[![](https://img.shields.io/travis/com/admiralwoop/multi-purpose-tg-bot/master?label=travis%20build)](https://travis-ci.com/admiralWoop/multi-purpose-tg-bot)
[![](https://images.microbadger.com/badges/version/admiralwoop/multi-purpose-tg-bot.svg)](hhttps://microbadger.com/images/admiralwoop/multi-purpose-tg-bot)
[![](https://img.shields.io/badge/docker%20images-099cec)](https://cloud.docker.com/u/admiralwoop/repository/docker/admiralwoop/multi-purpose-tg-bot)

**Multi-purpose Telegram Bot** is a telegram bot inside a ASP.NET Core app.

The main idea of this bot is use of a architecture where bot uses filters and corresponding handlers to handle updates received from Telegram.

That way you don't need to have single handler that handles all kinds of messages.
What you have is **the** router and many filters that decide what handler will handle new received message. Thus handlers have only one responsibility - get some information from a message and do something with it.

### Third party libraries
* [Telegram.Bot library](https://github.com/TelegramBots/Telegram.Bot)

### License 
* [MIT License](https://github.com/admiralWoop/multi-purpose-tg-bot/blob/master/LICENSE)
