FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Patients/HCSYS.PatientsService.Api/HCSYS.PatientsService.Api.csproj", "Services/Patients/HCSYS.PatientsService.Api/"]
COPY ["Services/Patients/HCSYS.PatientsService.Persistence/HCSYS.PatientsService.Persistence.csproj", "Services/Patients/HCSYS.PatientsService.Persistence/"]
RUN dotnet restore "./Services/Patients/HCSYS.PatientsService.Api/./HCSYS.PatientsService.Api.csproj"
COPY . .
WORKDIR "/src/Services/Patients/HCSYS.PatientsService.Api"
RUN dotnet build "./HCSYS.PatientsService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HCSYS.PatientsService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCSYS.PatientsService.Api.dll"]