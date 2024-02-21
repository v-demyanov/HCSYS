FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HCSYS.Api/HCSYS.Api.csproj", "HCSYS.Api/"]
COPY ["HCSYS.Core/HCSYS.Core.csproj", "HCSYS.Core/"]
COPY ["HCSYS.Persistence/HCSYS.Persistence.csproj", "HCSYS.Persistence/"]
RUN dotnet restore "./HCSYS.Api/HCSYS.Api.csproj"
COPY . .
WORKDIR "/src/HCSYS.Api"
RUN dotnet build "./HCSYS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HCSYS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCSYS.Api.dll"]