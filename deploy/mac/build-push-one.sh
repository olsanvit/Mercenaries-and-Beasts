#!/bin/bash
# build-push-one.sh — buildí a pushne jeden konkrétní projekt
#
# Použití:
#   bash build-push-one.sh vo2datamanager
#   bash build-push-one.sh simulatereal
#
# Dostupné názvy:
#   vo2datamanager  vinwmivehicles  unisportmanager  topelevenst
#   simulatereal    simulategames   simulatecar
#   myzabbix        mercenariesandbeasts

set -e

GHCR_USER="olsanvit"
PROJECTS_ROOT="/Users/rtvdata/Projects"
NAME="$1"

if [ -z "$NAME" ]; then
  echo "Použití: $0 <název-projektu>"
  echo "Příklad: $0 vo2datamanager"
  exit 1
fi

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

DIR="${PROJECTS[$NAME]}"
if [ -z "$DIR" ]; then
  echo "❌ Neznámý projekt: $NAME"
  echo "Dostupné: ${!PROJECTS[*]}"
  exit 1
fi

IMAGE="ghcr.io/$GHCR_USER/$NAME:latest"

echo "── Build: $NAME ──────────────────────────────────────"
docker build --platform linux/amd64 -t "$IMAGE" "$DIR"

echo "── Push: $IMAGE ──────────────────────────────────────"
docker push "$IMAGE"

echo ""
echo "✅ Hotovo. Na QNAP spusť:"
echo "  docker compose pull $NAME && docker compose up -d $NAME"
