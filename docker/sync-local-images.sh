#!/usr/bin/env sh
set -eu

AGRI_IMAGES_SOURCE="${AGRI_IMAGES_SOURCE:-/assets/firebase_images.zip}"
UPLOADS_ROOT="${UPLOADS_ROOT:-/uploads}"

echo "Syncing local image assets..."
echo "Configured source: ${AGRI_IMAGES_SOURCE}"

mkdir -p "${UPLOADS_ROOT}/blogs" "${UPLOADS_ROOT}/products"

SRC=""
if [ -f "${AGRI_IMAGES_SOURCE}" ] || [ -d "${AGRI_IMAGES_SOURCE}" ]; then
  SRC="${AGRI_IMAGES_SOURCE}"
else
  for candidate in \
    "/assets/firebase_images.zip" \
    "/assets/docs/firebase_images.zip" \
    "/assets/firebase_images" \
    "/assets/docs/firebase_images"
  do
    if [ -f "${candidate}" ] || [ -d "${candidate}" ]; then
      SRC="${candidate}"
      break
    fi
  done
fi

if [ -z "${SRC}" ]; then
  echo "No local image source found. Expected zip or folder containing blogs/products."
  exit 1
fi

echo "Using source: ${SRC}"
export SRC UPLOADS_ROOT

if [ -f "${SRC}" ]; then
  python3 - <<'PY'
import os
import zipfile

src = os.environ["SRC"]
dest = os.environ["UPLOADS_ROOT"]

with zipfile.ZipFile(src, "r") as zf:
    members = [m for m in zf.namelist() if m.startswith("blogs/") or m.startswith("products/")]
    if not members:
        raise SystemExit("Zip does not contain blogs/ or products/ paths")
    zf.extractall(dest, members=members)
PY
else
  python3 - <<'PY'
import os
import shutil

src = os.environ["SRC"]
dest = os.environ["UPLOADS_ROOT"]

copied = False
for folder in ("blogs", "products"):
    from_path = os.path.join(src, folder)
    to_path = os.path.join(dest, folder)
    if os.path.isdir(from_path):
        os.makedirs(to_path, exist_ok=True)
        for entry in os.listdir(from_path):
            src_file = os.path.join(from_path, entry)
            dst_file = os.path.join(to_path, entry)
            if os.path.isfile(src_file):
                shutil.copy2(src_file, dst_file)
                copied = True

if not copied:
    raise SystemExit("Source folder does not contain blogs/ or products/ files")
PY
fi

echo "Local image sync completed."
