FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["dotnet-app/APIL/WordCounterBot.APIL.WebApi.csproj", "APIL/"]
COPY ["dotnet-app/BLL/WordCounterBot.BLL.Core/WordCounterBot.BLL.Core.csproj", "BLL/WordCounterBot.BLL.Core/"]
COPY ["dotnet-app/BLL/WordCounterBot.BLL.Common/WordCounterBot.BLL.Common.csproj", "BLL/WordCounterBot.BLL.Common/"]
COPY ["dotnet-app/Common/WordCounterBot.Common.Logging/WordCounterBot.Common.Logging.csproj", "Common/WordCounterBot.Common.Logging/"]
COPY ["dotnet-app/Common/WordCounterBot.Common.Entities/WordCounterBot.Common.Entities.csproj", "Common/WordCounterBot.Common.Entities/"]
COPY ["dotnet-app/DAL/WordCounterBot.DAL.Contracts/WordCounterBot.DAL.Contracts.csproj", "DAL/WordCounterBot.DAL.Contracts/"]
COPY ["dotnet-app/DAL/WordCounterBot.DAL.DatabaseAccess/WordCounterBot.DAL.Postgresql.csproj", "DAL/WordCounterBot.DAL.DatabaseAccess/"]
COPY ["dotnet-app/BLL/WordCounterBot.BLL.Contracts/WordCounterBot.BLL.Contracts.csproj", "BLL/WordCounterBot.BLL.Contracts/"]
COPY . .
WORKDIR /src/APIL

FROM build AS publish
RUN dotnet publish "WordCounterBot.APIL.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordCounterBot.APIL.WebApi.dll"]