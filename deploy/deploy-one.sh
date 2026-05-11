#!/bin/bash
# deploy-one.sh — aktualizuje jeden konkrétní projekt bez rušení ostatních
#
# Použití:
#   bash deploy-one.sh vo2datamanager
#   bash deploy-one.sh simulatereal
#
# Dostupné názvy služeb:
#   vo2datamanager  vinwmivehicles  unisportmanager  topelevenst
#   simulatereal    simulategames   simulatecar
#   myzabbix        mercenariesandbeasts

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/.env"

SERVICE="$1"
if [ -z "$SERVICE" ]; then
  echo "Použití: $0 <název-služby>"
  echo "Příklad: $0 vo2datamanager"
  exit 1
fi

# Mapování service → git repo složka
declare -A REPO_MAP
REPO_MAP["vo2datamanager"]="$APPS_DIR/VO2DataManager"
REPO_MAP["vinwmivehicles"]="$APPS_DIR/VINWMIVehicles"
REPO_MAP["unisportmanager"]="$APPS_DIR/UNISportManager"
REPO_MAP["topelevenst"]="$APPS_DIR/TopElevenStats"
REPO_MAP["simulatereal"]="$APPS_DIR/SimulateReal"
REPO_MAP["simulategames"]="$APPS_DIR/SimulateGames"
REPO_MAP["simulatecar"]="$APPS_DIR/SimulateCar"
REPO_MAP["myzabbix"]="$APPS_DIR/MyZabbix"
REPO_MAP["mercenariesandbeasts"]="$APPS_DIR/MercenariesAndBeasts/Mercenaries-and-Beasts"

REPO_DIR="${REPO_MAP[$SERVICE]}"
if [ -z "$REPO_DIR" ]; then
  echo "❌ Neznámá služba: $SERVICE"
  echo "Dostupné: ${!REPO_MAP[*]}"
  exit 1
fi

echo "── Deploying: $SERVICE ──────────────────────────────"
echo ""

echo -n "  git pull ... "
git -C "$REPO_DIR" pull --ff-only origin main 2>&1 | tail -1

echo "  docker compose build + restart ..."
cd "$SCRIPT_DIR"
docker compose --env-file .env up --build -d "$SERVICE"

echo ""
echo "✅ $SERVICE nasazen."
docker compose ps "$SERVICE" --format "table {{.Name}}\t{{.Status}}"
