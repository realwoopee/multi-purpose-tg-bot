# Multi-purpose Telegram Bot Docker example
This branch is a guide and an example on how to deploy this bot on a server using docker.

## 1. Getting docker
Please check [docker documentation](https://docs.docker.com/get-started/)
## 2. Pull this branch
```bash
git clone -b example/docker https://github.com/admiralWoop/multi-purpose-tg-bot.git
```
## 3. Configure `env` file
```bash
ASPNETCORE_ENVIRONMENT=Production # sets ASP.NET Core enviroment to Production
ASPNETCORE_URLS=https://0.0.0.0:443/ # sets a port to wich bot will listen to
ASPNETCORE_Kestrel__Certificates__Default__Password=[your password] # password for your SSL sertcificate
ASPNETCORE_Kestrel__Certificates__Default__Path=ssl.pfx # path to your SSL certificate for HTTPS
DbConnectionString=[your_string] # connection string for a postgresql server
TgToken=[your_token] # your bot's telegram token
WebhookUrl=[example_webhook] # url to which telegram will send updates
SSLCertPath=ssl.pem # path to SSL certificate that will be sent to telegram (see https://core.telegram.org/bots/self-signed)
UseSocks5=false # should bot use a socks5 proxy when sending requests to telegram or not
Socks5Host=[your_host] # IP of the socks5 proxy
Socks5Port=[your_port] # port of the socks5 proxy
```
## 4. Deploy
### 4.1 Using docker-compose
#### Get docker-compose
Use [this guide from docker](https://docs.docker.com/compose/install/)
#### Configure docker-compose.yml (optional)
```bash
vim docker-compose.yml
# :!q + ENTER
```
#### Deploy the bot
```bash 
docker-compose up
```
### OR
### 4.2 Using docker
#### Create secrets for SSL certificates
```bash
docker secret create ssl_pfx [path_to_your_ssl.pfx] &&
docker secret create ssl_pem [path_to_your_ssl.pem]
```
#### Deploy the bot
```bash
# /app is the path to the ASP.NET Core executable
docker run -env-file env -p 443 \
 --secret src=ssl_pfx,target="/app/ssl.pfx" \
 --secret src=ssl_pem,target="/app/ssl.pem" \
 admiralwoop/multi-purpose-tg-bot:latest
```