# Agri Energy Platform - Database Guide

## Database engine

- Primary engine: SQL Server.
- Docker stack starts SQL Server in `agri-energy-platform-db`.

## Connection modes

The app resolves connection strings in this order:

1. Docker env vars (`DB_HOST`, `DB_NAME`, `DB_SA_PASSWORD`) to build a SQL Server connection.
2. `ConnectionStrings:DefaultConnection` or `ConnectionStrings:default` from `appsettings.json`.

## Migration behavior

- Controlled by `Database:AutoMigrate`.
- When enabled, startup ensures the target database exists, then runs EF Core migrations.

## Seed SQL files

The docker seed service imports one of these files:

- `docs/Agri-Energy-Platform_2022.sql` (default)
- `docs/Agri-Energy-Platform_2019.sql` (optional via `AGRI_SQL_SEED_VERSION=2019`)

## Seeded users

After SQL seed import, seeded users use password `Password@123`.

- Admin: `admin`
- Employees: `Employee1` to `Employee3`
- Farmers: `Farmer1` to `Farmer30`

## Local development checklist

1. Ensure SQL Server is reachable.
2. Set the appropriate connection source (env vars or appsettings).
3. Decide whether `Database:AutoMigrate` should run.
4. If using Docker, confirm `SA_PASSWORD` and optional seed version in `docker/.env`.
