FROM mcr.microsoft.com/dotnet/aspnet:10.0.14 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0.14 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AgriEnergyPlatform.Web/AgriEnergyPlatform.Web.csproj", "src/AgriEnergyPlatform.Web/"]
COPY ["src/AgriEnergyPlatform.Application/AgriEnergyPlatform.Application.csproj", "src/AgriEnergyPlatform.Application/"]
COPY ["src/AgriEnergyPlatform.Domain/AgriEnergyPlatform.Domain.csproj", "src/AgriEnergyPlatform.Domain/"]
COPY ["src/AgriEnergyPlatform.Infrastructure/AgriEnergyPlatform.Infrastructure.csproj", "src/AgriEnergyPlatform.Infrastructure/"]
RUN dotnet restore "./src/AgriEnergyPlatform.Web/AgriEnergyPlatform.Web.csproj"
COPY . .
WORKDIR "/src/src/AgriEnergyPlatform.Web"
RUN dotnet build "./AgriEnergyPlatform.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AgriEnergyPlatform.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /app/wwwroot/uploads && chown -R app:app /app
USER app
ENTRYPOINT ["dotnet", "AgriEnergyPlatform.Web.dll"]
