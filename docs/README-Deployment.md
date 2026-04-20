# Agri Energy Platform - Deployment Guide

Project target framework is .NET 10 (`global.json` pins SDK `10.0.100` for local development commands).

## Runtime configuration

Key environment values used by the web application:

- `ASPNETCORE_ENVIRONMENT`
- `ASPNETCORE_URLS`
- `DB_HOST`, `DB_NAME`, `DB_SA_PASSWORD`
- `Storage__Provider`
- `Storage__Firebase__ApiKey`, `Storage__Firebase__Bucket` (when Firebase is used)
- `Database__AutoMigrate`
- `DisableHttpsRedirect`

## Container deployment

From project root:

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Start already-built containers:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

Default ports:

- App: `8001` (`APP_PORT`)
- SQL Server: `8002` (`SQL_PORT`)

## HTTPS behavior

- In Docker, compose sets `DisableHttpsRedirect=true` for local HTTP.
- In non-development environments, HSTS is enabled by default.

## Storage provider choices

- `Local` (default): uploads stored under `wwwroot/uploads`.
- `Firebase`: requires Firebase API key and bucket configuration.

## Production notes

- Use strong, non-default SQL passwords.
- Run with explicit environment-specific settings.
- Keep migration strategy intentional (`Database__AutoMigrate=true` for automated startup migrations, or disabled for controlled release processes).
