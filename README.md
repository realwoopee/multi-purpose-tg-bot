# Multi-purpose Telegram Bot Docker example
This branch is a guide and an example on how to deploy this bot on a server using docker.

## 1. Getting docker
Please check [docker documentation](https://docs.docker.com/get-started/)
## 2. Pull this branch
```bash
git clone -b example/docker https://github.com/admiralWoop/multi-purpose-tg-bot.git
```
## 3. Prepare for deployment
### 3.1 Configure the app
Remove `.example` postfixes from files and fill in your values.
Don't forget about filling your email in docker-compose.yml file
## 4. Deploy using docker-compose
#### 4.1 Get docker-compose
Use [this guide from docker](https://docs.docker.com/compose/install/)
#### 4.2 Configure `docker-compose.yml` (optional)
```bash
vim docker-compose.yml
# :!q + ENTER
```
#### 4.3 Deploy the bot
```bash 
docker-compose up
```
## 5. Update
Run `update.sh`

To get the latest version of the bot and run it using docker-compose
