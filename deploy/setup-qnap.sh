#!/bin/bash
# setup-qnap.sh — PRVNÍ SPUŠTĚNÍ na QNAP
# Naklonuje všechny repozitáře a připraví prostředí
#
# Spustit jednou přes SSH:
#   bash setup-qnap.sh

set -e

APPS_DIR="/share/homes/admin/apps"
mkdir -p "$APPS_DIR"

echo "── Klonování repozitářů ─────────────────────────────"

declare -A REPOS
REPOS["VO2DataManager"]="https://github.com/olsanvit/VO2DataManager.git"
REPOS["VINWMIVehicles"]="https://github.com/olsanvit/VINWMIVehicles.git"
REPOS["UNISportManager"]="https://github.com/olsanvit/UniSportManager.git"
REPOS["TopElevenStats"]="https://github.com/olsanvit/TopElevenStats.git"
REPOS["SimulateReal"]="https://github.com/olsanvit/SimulateReal.git"
REPOS["SimulateGames"]="https://github.com/olsanvit/SimulateGames.git"
REPOS["SimulateCar"]="https://github.com/olsanvit/SimulateCar.git"
REPOS["MyZabbix"]="https://github.com/olsanvit/MyZabbix.git"

for name in "${!REPOS[@]}"; do
  dir="$APPS_DIR/$name"
  url="${REPOS[$name]}"
  if [ -d "$dir" ]; then
    echo "  $name — složka existuje, přeskakuji"
  else
    echo "  Klonuji $name ..."
    git clone --recurse-submodules "$url" "$dir"
  fi
done

# MercenariesAndBeasts má jiný podadresář
MAB_URL="https://github.com/olsanvit/Mercenaries-and-Beasts.git"
MAB_DIR="$APPS_DIR/MercenariesAndBeasts/Mercenaries-and-Beasts"
mkdir -p "$APPS_DIR/MercenariesAndBeasts"
if [ -d "$MAB_DIR" ]; then
  echo "  MercenariesAndBeasts — složka existuje, přeskakuji"
else
  echo "  Klonuji MercenariesAndBeasts ..."
  git clone --recurse-submodules "$MAB_URL" "$MAB_DIR"
fi

echo ""
echo "── Nastavení deploy ────────────────────────────────"
DEPLOY_DIR="$MAB_DIR/deploy"
cd "$DEPLOY_DIR"

# Nastav správnou cestu v .env
sed -i "s|APPS_DIR=.*|APPS_DIR=$APPS_DIR|" .env

chmod +x deploy-all.sh deploy-one.sh

echo ""
echo "✅ Setup hotov!"
echo ""
echo "Teď:"
echo "  1. Zkontroluj $DEPLOY_DIR/.env (cesta APPS_DIR)"
echo "  2. Ujisti se, že DNS záznamy *.vo2info.cz ukazují na IP tohoto QNAP"
echo "  3. Spusť: bash $DEPLOY_DIR/deploy-all.sh"
