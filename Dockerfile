FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY /app .
ENTRYPOINT ["dotnet", "WordCounterBot.APIL.WebApi.dll"]
