#!/bin/bash
# publish-all.sh — spustit na Macu
# Publishne všechny projekty a nahraje je na QNAP přes SCP
#
# Použití:
#   bash deploy/mac/publish-all.sh
#
# Předpoklady:
#   - SSH klíč nastaven pro VOAdmin@192.168.60.221
#     (nebo se bude ptát na heslo pro každý projekt)

set -e

QNAP="VOAdmin@192.168.60.221"
PROJECTS_ROOT="/Users/rtvdata/Projects"
RUNTIME="linux-x64"

# Mapování: název → (csproj, dll, qnap cílová složka)
# Formát: "csproj|dll|qnap_dir"
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

PUBLISH_TMP="/tmp/blazor-publish"
FAILED=()

echo "╔══════════════════════════════════════════════════╗"
echo "║     vo2info.cz — publish + upload na QNAP       ║"
echo "╚══════════════════════════════════════════════════╝"
echo ""

for name in "${!APPS[@]}"; do
  IFS='|' read -r CSPROJ DLL QNAP_DIR <<< "${APPS[$name]}"
  OUT="$PUBLISH_TMP/$name"

  echo "── $name ────────────────────────────────────────────"
  rm -rf "$OUT"

  echo "  publish ($RUNTIME)..."
  if dotnet publish "$CSPROJ" -c Release -r "$RUNTIME" --no-self-contained -o "$OUT" --nologo -v q; then
    echo "  scp → $QNAP:$QNAP_DIR/publish ..."
    ssh "$QNAP" "mkdir -p $QNAP_DIR/publish"
    rsync -az --delete "$OUT/" "$QNAP:$QNAP_DIR/publish/"
    echo "  ✅ OK"
  else
    echo "  ❌ PUBLISH SELHAL"
    FAILED+=("$name")
  fi
  echo ""
done

rm -rf "$PUBLISH_TMP"

echo "══════════════════════════════════════════════════════"
if [ ${#FAILED[@]} -eq 0 ]; then
  echo "✅ Všechny projekty nahrány na QNAP."
  echo ""
  echo "Na QNAP spusť:"
  echo "  docker compose -f /share/homes/VOAdmin/deploy/docker-compose.yml up -d"
else
  echo "⚠️  Selhalo: ${FAILED[*]}"
fi
