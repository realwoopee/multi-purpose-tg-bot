FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim-arm64v8
WORKDIR /app
COPY /app .
ENTRYPOINT ["dotnet", "WordCounterBot.APIL.WebApi.dll"]
