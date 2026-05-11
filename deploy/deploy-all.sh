#!/bin/bash
# deploy-all.sh — spustit na QNAP (SSH)
# Stáhne nejnovější kód ze všech repo a restartuje všechny kontejnery
#
# Použití:
#   cd /share/homes/admin/apps/Mercenaries-and-Beasts/deploy
#   bash deploy-all.sh

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/.env"

REPOS=(
  "VO2DataManager"
  "VINWMIVehicles"
  "UNISportManager"
  "TopElevenStats"
  "SimulateReal"
  "SimulateGames"
  "SimulateCar"
  "MyZabbix"
)
# MercenariesAndBeasts má jiný podadresář
MAB_DIR="$APPS_DIR/MercenariesAndBeasts/Mercenaries-and-Beasts"

echo "╔══════════════════════════════════════════════════╗"
echo "║        vo2info.cz — kompletní deploy             ║"
echo "╚══════════════════════════════════════════════════╝"
echo ""

# ── 1. Git pull všech repo ────────────────────────────────────────────────────
echo "── git pull ─────────────────────────────────────────"
for repo in "${REPOS[@]}"; do
  dir="$APPS_DIR/$repo"
  if [ -d "$dir" ]; then
    echo -n "  $repo ... "
    git -C "$dir" pull --ff-only origin main 2>&1 | tail -1
  else
    echo "  ⚠️  $repo: složka nenalezena ($dir)"
  fi
done

echo -n "  MercenariesAndBeasts ... "
git -C "$MAB_DIR" pull --ff-only origin main 2>&1 | tail -1

echo ""

# ── 2. Docker compose build + up ─────────────────────────────────────────────
echo "── docker compose build + up ────────────────────────"
cd "$SCRIPT_DIR"
docker compose --env-file .env up --build -d

echo ""
echo "✅ Deploy hotov. Spuštěné kontejnery:"
docker compose ps --format "table {{.Name}}\t{{.Status}}"
