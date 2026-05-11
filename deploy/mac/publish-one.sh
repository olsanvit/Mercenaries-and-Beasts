#!/bin/bash
# publish-one.sh — publishne a nahraje jeden projekt na QNAP
#
# Použití:
#   bash deploy/mac/publish-one.sh simulatereal
#   bash deploy/mac/publish-one.sh vo2datamanager
#
# Dostupné názvy:
#   vo2datamanager  vinwmivehicles  unisportmanager  topelevenst
#   simulatereal    simulategames   simulatecar
#   myzabbix        mercenariesandbeasts

set -e

QNAP="VOAdmin@192.168.60.221"
PROJECTS_ROOT="/Users/rtvdata/Projects"
RUNTIME="linux-x64"
NAME="$1"

if [ -z "$NAME" ]; then
  echo "Použití: $0 <název-projektu>"
  echo "Příklad: $0 simulatereal"
  exit 1
fi

declare -A APPS
APPS["vo2datamanager"]="$PROJECTS_ROOT/VO2DataManager/src/VO2DataManager.Web/VO2DataManager.Web.csproj|VO2DataManager.Web.dll|/share/Public/BlazorVO2DataManager"
APPS["vinwmivehicles"]="$PROJECTS_ROOT/VINWMIVehicles/src/VINWMIVehicles.Web/VINWMIVehicles.Web.csproj|VINWMIVehicles.Web.dll|/share/Public/BlazorVINWMI"
APPS["unisportmanager"]="$PROJECTS_ROOT/UNISportManager/src/SportManager.Web/SportManager.Web.csproj|SportManager.Web.dll|/share/Public/BlazorSportManager"
APPS["topelevenst"]="$PROJECTS_ROOT/TopElevenStats/src/TopElevenStats.Web/TopElevenStats.Web.csproj|TopElevenStats.Web.dll|/share/Public/BlazorTopEleven"
APPS["simulatereal"]="$PROJECTS_ROOT/SimulateReal/src/SimulateReal.Web/SimulateReal.Web.csproj|SimulateReal.Web.dll|/share/Public/BlazorSimulateReal1"
APPS["simulategames"]="$PROJECTS_ROOT/SimulateGames/src/SimulateGames.Web/SimulateGames.Web.csproj|SimulateGames.Web.dll|/share/Public/BlazorSimulateGames"
APPS["simulatecar"]="$PROJECTS_ROOT/SimulateCar/src/SimulateCar.Web/SimulateCar.Web.csproj|SimulateCar.Web.dll|/share/Public/BlazorSimulateCar"
APPS["myzabbix"]="$PROJECTS_ROOT/MyZabbix/src/MyZabbix.Web/MyZabbix.Web.csproj|MyZabbix.Web.dll|/share/Public/BlazorMyZabbix"
APPS["mercenariesandbeasts"]="$PROJECTS_ROOT/MercenariesAndBeasts/Mercenaries-and-Beasts/src/MercenariesAndBeasts.Web/MercenariesAndBeasts.Web.csproj|MercenariesAndBeasts.Web.dll|/share/Public/BlazorMercenariesAndBeasts"

ENTRY="${APPS[$NAME]}"
if [ -z "$ENTRY" ]; then
  echo "❌ Neznámý projekt: $NAME"
  echo "Dostupné: ${!APPS[*]}"
  exit 1
fi

IFS='|' read -r CSPROJ DLL QNAP_DIR <<< "$ENTRY"
OUT="/tmp/blazor-publish/$NAME"

echo "── Publish: $NAME ───────────────────────────────────"
rm -rf "$OUT"

echo "  dotnet publish ($RUNTIME)..."
dotnet publish "$CSPROJ" -c Release -r "$RUNTIME" --no-self-contained -o "$OUT" --nologo

echo "  scp → $QNAP:$QNAP_DIR/publish ..."
ssh "$QNAP" "mkdir -p $QNAP_DIR/publish"
rsync -az --delete "$OUT/" "$QNAP:$QNAP_DIR/publish/"

rm -rf "$OUT"

echo ""
echo "✅ $NAME nahrán na QNAP."
echo ""
echo "Na QNAP restartuj kontejner:"
echo "  docker compose -f /share/homes/VOAdmin/deploy/docker-compose.yml restart $NAME"
