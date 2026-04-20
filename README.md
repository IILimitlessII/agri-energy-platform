# Agri Energy Platform

Cleaned project layout:

- `src/` runtime projects (`AgriEnergyPlatform.Web`, `Application`, `Domain`, `Infrastructure`)
- `tests/` test project
- `docker/` Docker compose and environment files
- `docs/` setup and migration notes

## Prerequisites

- .NET SDK 10.x (`global.json` pins `10.0.100`)
- Docker + Docker Compose (for container workflow)

## Quick start

From this folder:

```bash
dotnet restore AgriEnergyPlatform.sln
dotnet build AgriEnergyPlatform.sln
dotnet run --project src/AgriEnergyPlatform.Web/AgriEnergyPlatform.Web.csproj
```

## Docker quick start

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

App URL: `http://localhost:8001` (default)

## Docker start (already built)

When images are already built and containers just need starting:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## Local image archive location

For the default local-image sync flow, keep:

- `firebase_images.zip` at the project root: `agri-energy-platform/firebase_images.zip`

## Credentials

- Docker SQL seed initialization sets known demo users (see docs for full list) with password `Password@123`.
- SQL Server `sa` password is set via `docker/.env` (`SA_PASSWORD`).

## Documentation

See `docs/README.md` for the standardized documentation index.
