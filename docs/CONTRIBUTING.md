# Contributing

## Local setup

```bash
dotnet restore AgriEnergyPlatform.sln
dotnet build AgriEnergyPlatform.sln
dotnet run --project src/AgriEnergyPlatform.Web/AgriEnergyPlatform.Web.csproj
```

For containerized setup, follow `README-Docker.md`.

## Contribution workflow

1. Create a focused branch per task.
2. Keep changes scoped and easy to review.
3. Run build/tests relevant to your changes.
4. Update docs when behavior or configuration changes.

## Database changes

- Keep EF Core migrations in source control.
- Document migration or seed changes in `README-Database.md`.

## Pull request checklist

- Code builds successfully.
- Relevant tests pass.
- New config/env vars are documented.
- Documentation links remain valid.
