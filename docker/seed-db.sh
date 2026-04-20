#!/usr/bin/env bash
set -euo pipefail

DB_HOST="${DB_HOST:-agri-energy-platform-db}"
DB_PORT="${DB_PORT:-1433}"
DB_NAME="${DB_NAME:-AgriEnergyConnect}"
SA_PASSWORD="${SA_PASSWORD:?SA_PASSWORD is required}"
AGRI_SQL_SEED_VERSION="${AGRI_SQL_SEED_VERSION:-2022}"

SEED_SQL_FILE="/seed/Agri-Energy-Platform_2022.sql"
if [ "$AGRI_SQL_SEED_VERSION" = "2019" ]; then
  SEED_SQL_FILE="/seed/Agri-Energy-Platform_2019.sql"
fi

if [ ! -f "$SEED_SQL_FILE" ]; then
  echo "Seed SQL file not found: $SEED_SQL_FILE"
  exit 1
fi

echo "Waiting for SQL Server at ${DB_HOST}:${DB_PORT}..."
for _ in $(seq 1 30); do
  if /opt/mssql-tools18/bin/sqlcmd -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -Q "SELECT 1" >/dev/null 2>&1; then
    break
  fi
  sleep 2
done

echo "Ensuring database ${DB_NAME} exists..."
/opt/mssql-tools18/bin/sqlcmd -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -Q "IF DB_ID(N'${DB_NAME}') IS NULL CREATE DATABASE [${DB_NAME}];"

HAS_TABLES=$(
  /opt/mssql-tools18/bin/sqlcmd -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -h -1 -W -Q "SET NOCOUNT ON; SELECT CASE WHEN EXISTS (SELECT 1 FROM [${DB_NAME}].sys.tables WHERE name = 'AspNetUsers' AND schema_id = SCHEMA_ID('dbo')) THEN 1 ELSE 0 END;"
)

IMPORTED=0
if [ "${HAS_TABLES//[[:space:]]/}" = "1" ]; then
  echo "Database already seeded, skipping SQL import."
else
  TMP_SQL="/tmp/agri-seed.sql"
  cp "$SEED_SQL_FILE" "$TMP_SQL"
  sed -i "1s|^USE \\[[^]]*\\]|USE [${DB_NAME}]|" "$TMP_SQL"

  echo "Seeding ${DB_NAME} using ${SEED_SQL_FILE}..."
  /opt/mssql-tools18/bin/sqlcmd -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -b -d "${DB_NAME}" -i "$TMP_SQL"
  IMPORTED=1
fi

echo "Rewriting Firebase URLs to local /uploads paths..."
/opt/mssql-tools18/bin/sqlcmd -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -b -d "${DB_NAME}" -Q "
SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;

IF OBJECT_ID(N'dbo.ProductImages', N'U') IS NOT NULL
BEGIN
  UPDATE dbo.ProductImages
  SET Url = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Url,
      CHARINDEX('/o/', Url) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Url) > 0 THEN CHARINDEX('?alt=', Url) - (CHARINDEX('/o/', Url) + 3)
        ELSE LEN(Url) - (CHARINDEX('/o/', Url) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Url LIKE 'https://firebasestorage.googleapis.com/%/o/%';
END

IF OBJECT_ID(N'dbo.Blogs', N'U') IS NOT NULL
BEGIN
  UPDATE dbo.Blogs
  SET BlogImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      BlogImageUrl,
      CHARINDEX('/o/', BlogImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', BlogImageUrl) > 0 THEN CHARINDEX('?alt=', BlogImageUrl) - (CHARINDEX('/o/', BlogImageUrl) + 3)
        ELSE LEN(BlogImageUrl) - (CHARINDEX('/o/', BlogImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE BlogImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';

  UPDATE dbo.Blogs
  SET Heading1ImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Heading1ImageUrl,
      CHARINDEX('/o/', Heading1ImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Heading1ImageUrl) > 0 THEN CHARINDEX('?alt=', Heading1ImageUrl) - (CHARINDEX('/o/', Heading1ImageUrl) + 3)
        ELSE LEN(Heading1ImageUrl) - (CHARINDEX('/o/', Heading1ImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Heading1ImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';

  UPDATE dbo.Blogs
  SET Heading2ImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Heading2ImageUrl,
      CHARINDEX('/o/', Heading2ImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Heading2ImageUrl) > 0 THEN CHARINDEX('?alt=', Heading2ImageUrl) - (CHARINDEX('/o/', Heading2ImageUrl) + 3)
        ELSE LEN(Heading2ImageUrl) - (CHARINDEX('/o/', Heading2ImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Heading2ImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';

  UPDATE dbo.Blogs
  SET Heading3ImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Heading3ImageUrl,
      CHARINDEX('/o/', Heading3ImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Heading3ImageUrl) > 0 THEN CHARINDEX('?alt=', Heading3ImageUrl) - (CHARINDEX('/o/', Heading3ImageUrl) + 3)
        ELSE LEN(Heading3ImageUrl) - (CHARINDEX('/o/', Heading3ImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Heading3ImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';

  UPDATE dbo.Blogs
  SET Heading4ImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Heading4ImageUrl,
      CHARINDEX('/o/', Heading4ImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Heading4ImageUrl) > 0 THEN CHARINDEX('?alt=', Heading4ImageUrl) - (CHARINDEX('/o/', Heading4ImageUrl) + 3)
        ELSE LEN(Heading4ImageUrl) - (CHARINDEX('/o/', Heading4ImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Heading4ImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';

  UPDATE dbo.Blogs
  SET Heading5ImageUrl = '/uploads/' + REPLACE(REPLACE(SUBSTRING(
      Heading5ImageUrl,
      CHARINDEX('/o/', Heading5ImageUrl) + 3,
      CASE
        WHEN CHARINDEX('?alt=', Heading5ImageUrl) > 0 THEN CHARINDEX('?alt=', Heading5ImageUrl) - (CHARINDEX('/o/', Heading5ImageUrl) + 3)
        ELSE LEN(Heading5ImageUrl) - (CHARINDEX('/o/', Heading5ImageUrl) + 2)
      END
    ), '%2F', '/'), '%2f', '/')
  WHERE Heading5ImageUrl LIKE 'https://firebasestorage.googleapis.com/%/o/%';
END
"

if [ "$IMPORTED" = "1" ]; then
  echo "Database seed completed."
else
  echo "Database already existed; URL normalization completed."
fi
