#!/bin/bash
# build-push-all.sh — spustit na Macu
# Buildí všechny Docker obrazy a pushne je na ghcr.io
#
# Použití:
#   cd deploy/mac
#   bash build-push-all.sh
#
# Při prvním spuštění je potřeba login:
#   echo "TVUJ_GITHUB_TOKEN" | docker login ghcr.io -u olsanvit --password-stdin

set -e

GHCR_USER="olsanvit"
PROJECTS_ROOT="/Users/rtvdata/Projects"

# Mapování: název obrazu → složka projektu
declare -A PROJECTS
PROJECTS["vo2datamanager"]="$PROJECTS_ROOT/VO2DataManager"
PROJECTS["vinwmivehicles"]="$PROJECTS_ROOT/VINWMIVehicles"
PROJECTS["unisportmanager"]="$PROJECTS_ROOT/UNISportManager"
PROJECTS["topelevenst"]="$PROJECTS_ROOT/TopElevenStats"
PROJECTS["simulatereal"]="$PROJECTS_ROOT/SimulateReal"
PROJECTS["simulategames"]="$PROJECTS_ROOT/SimulateGames"
PROJECTS["simulatecar"]="$PROJECTS_ROOT/SimulateCar"
PROJECTS["myzabbix"]="$PROJECTS_ROOT/MyZabbix"
PROJECTS["mercenariesandbeasts"]="$PROJECTS_ROOT/MercenariesAndBeasts/Mercenaries-and-Beasts"

echo "╔══════════════════════════════════════════════════╗"
echo "║     vo2info.cz — build + push všech obrazů      ║"
echo "╚══════════════════════════════════════════════════╝"
echo ""

FAILED=()

for name in "${!PROJECTS[@]}"; do
  dir="${PROJECTS[$name]}"
  image="ghcr.io/$GHCR_USER/$name:latest"
  echo "── $name ────────────────────────────────────────────"

  if [ ! -f "$dir/Dockerfile" ]; then
    echo "  ⚠️  Dockerfile nenalezen v $dir — přeskakuji"
    FAILED+=("$name")
    continue
  fi

  echo "  build $image ..."
  if docker build --platform linux/amd64 -t "$image" "$dir"; then
    echo "  push ..."
    docker push "$image"
    echo "  ✅ OK"
  else
    echo "  ❌ BUILD SELHAL"
    FAILED+=("$name")
  fi
  echo ""
done

echo "══════════════════════════════════════════════════════"
if [ ${#FAILED[@]} -eq 0 ]; then
  echo "✅ Všechny obrazy pushnuty na ghcr.io/$GHCR_USER/"
  echo ""
  echo "Na QNAP spusť:"
  echo "  docker compose pull && docker compose up -d"
else
  echo "⚠️  Selhalo: ${FAILED[*]}"
fi
