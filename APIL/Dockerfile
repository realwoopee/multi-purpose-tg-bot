FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["APIL/WordCounterBot.APIL.WebApi.csproj", "APIL/"]
COPY ["BLL/WordCounterBot.BLL.Core/WordCounterBot.BLL.Core.csproj", "BLL/WordCounterBot.BLL.Core/"]
COPY ["BLL/WordCounterBot.BLL.Common/WordCounterBot.BLL.Common.csproj", "BLL/WordCounterBot.BLL.Common/"]
COPY ["Common/WordCounterBot.Common.Logging/WordCounterBot.Common.Logging.csproj", "Common/WordCounterBot.Common.Logging/"]
COPY ["DAL/WordCounterBot.DAL.Entities/WordCounterBot.Common.Entities.csproj", "DAL/WordCounterBot.DAL.Entities/"]
COPY ["DAL/WordCounterBot.DAL.Contracts/WordCounterBot.DAL.Contracts.csproj", "DAL/WordCounterBot.DAL.Contracts/"]
COPY ["DAL/WordCounterBot.DAL.DatabaseAccess/WordCounterBot.DAL.Postgresql.csproj", "DAL/WordCounterBot.DAL.DatabaseAccess/"]
COPY ["BLL/WordCounterBot.BLL.Contracts/WordCounterBot.BLL.Contracts.csproj", "BLL/WordCounterBot.BLL.Contracts/"]
RUN dotnet restore "APIL/WordCounterBot.APIL.WebApi.csproj"
COPY . .
WORKDIR /src/APIL
RUN dotnet build "WordCounterBot.APIL.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordCounterBot.APIL.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordCounterBot.APIL.WebApi.dll"]