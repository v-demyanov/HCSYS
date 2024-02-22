FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HCSYS.ConsoleClient/HCSYS.ConsoleClient.csproj", "HCSYS.ConsoleClient/"]
COPY ["HCSYS.Core/HCSYS.Core.csproj", "HCSYS.Core/"]
COPY ["HCSYS.Persistence/HCSYS.Persistence.csproj", "HCSYS.Persistence/"]
RUN dotnet restore "./HCSYS.ConsoleClient/./HCSYS.ConsoleClient.csproj"
COPY . .
WORKDIR "/src/HCSYS.ConsoleClient"
RUN dotnet build "./HCSYS.ConsoleClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HCSYS.ConsoleClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCSYS.ConsoleClient.dll"]