# Agri Energy Platform - Docker

Run the platform with SQL Server, automatic migration, SQL seed import, and local image storage.

This project targets .NET 10 for local non-Docker workflows (`global.json` pins `10.0.100`).

## Quick start

```bash
cp docker/.env.example docker/.env
# set a strong SA_PASSWORD in docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Open `http://localhost:8001` (or your `APP_PORT` value).

## Start existing containers (detached)

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## Environment variables

| Variable | Purpose |
| --- | --- |
| `SA_PASSWORD` | SQL Server `sa` password (required) |
| `DB_NAME` | Database name (default `AgriEnergyConnect`) |
| `APP_PORT` | Host app port (default `8001`) |
| `SQL_PORT` | Host SQL Server port (default `8002`) |
| `AGRI_SQL_SEED_VERSION` | Seed SQL version (`2022` or `2019`) |
| `AGRI_IMAGES_SOURCE` | Source zip/folder path in container assets (default `/assets/firebase_images.zip`) |

The app container also sets:

- `DB_HOST`, `DB_NAME`, `DB_SA_PASSWORD` for SQL connectivity.
- `Storage__Provider=Local` to keep uploads in `/app/wwwroot/uploads`.
- `Database__AutoMigrate=true` to apply migrations on startup.
- `DisableHttpsRedirect=true` for local HTTP in containers.

## Seed + local image sync behavior

Startup order in Docker is:

1. SQL Server becomes healthy.
2. Image sync container extracts/copies restored image assets into the shared uploads volume (`/uploads/blogs`, `/uploads/products`).
3. Seed container imports SQL (first run) and always rewrites Firebase URLs in DB to local `/uploads/...` paths.
4. Web container starts and serves those local files from `wwwroot/uploads`.

By default the image sync container reads `/assets/firebase_images.zip`, which maps to `agri-energy-platform/firebase_images.zip` in your repo root.  
You can override via `AGRI_IMAGES_SOURCE` (for example `/assets/docs/firebase_images.zip` or `/assets/docs/firebase_images`).

On first run, the seed container imports `Agri-Energy-Platform_2022.sql` (or `2019` if configured). Seeded users are reset to password `Password@123` and include:

- `admin`
- `Employee1` to `Employee3`
- `Farmer1` to `Farmer30`

## Storage mode

Docker runs with local file uploads by default.  
For non-Docker runs, you can switch to Firebase with:

- `Storage__Provider=Firebase`
- `Storage__Firebase__ApiKey`
- `Storage__Firebase__Bucket`

## Troubleshooting

- SQL Server password must satisfy complexity requirements.
- If SQL healthcheck fails, confirm port `8002` is free and retry.
- If startup is slow on your machine, retry after increasing compose healthcheck timings.
- If image sync fails, confirm your zip/folder contains `blogs/` and `products/` at the top level.

## Verification checks

After startup, you can verify the migration from Firebase URLs to local URLs:

```sql
SELECT TOP 10 Url
FROM dbo.ProductImages
ORDER BY Id;
```

```sql
SELECT TOP 10 BlogImageUrl, Heading1ImageUrl, Heading2ImageUrl, Heading3ImageUrl, Heading4ImageUrl, Heading5ImageUrl
FROM dbo.Blogs
ORDER BY Id;
```

Expected: URLs begin with `/uploads/...` and no rows should contain `firebasestorage.googleapis.com`.
