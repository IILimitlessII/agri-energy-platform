# Agri Energy Platform - Architecture

## Solution layout

- `src/AgriEnergyPlatform.Web` - ASP.NET Core MVC web application.
- `src/AgriEnergyPlatform.Application` - application-level services and use cases.
- `src/AgriEnergyPlatform.Domain` - domain entities and core business logic.
- `src/AgriEnergyPlatform.Infrastructure` - infrastructure components.
- `tests` - automated tests.

## Current layering note (pre-split)

The named Application/Domain/Infrastructure projects are currently transition shells for pre-split clarity.  
`Domain` and `Infrastructure` still compile selected source files from `Web` via `Compile Include` entries in their `.csproj` files.  
This keeps runtime behavior stable while preserving a clear path for a later physical code extraction.

## Web project structure

Within `src/AgriEnergyPlatform.Web`:

- `Controllers` - request handling and route actions.
- `Views` - Razor views.
- `Models` and `ViewModels` - data and view contracts.
- `Database` and `Migrations` - EF Core DbContext and migration artifacts.
- `Services/Storage` - local and Firebase image storage implementations.
- `wwwroot` - static assets and uploaded files.

## Runtime flow

1. Resolve database connection from environment or appsettings.
2. Register storage provider (`Local` or `Firebase`) based on configuration.
3. Configure Identity and role-based policies.
4. Optionally run database creation/migrations when `Database:AutoMigrate=true`.
5. Configure middleware and MVC routes, then start the app.
