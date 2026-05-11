#!/bin/sh
# publish-on-qnap.sh — git pull + dotnet publish + restart na QNAP
# Nevyžaduje .NET SDK — buildí uvnitř Docker kontejneru
#
# Použití:
#   sh publish-on-qnap.sh              # všechny projekty
#   sh publish-on-qnap.sh simulatereal # jen jeden projekt

APPS_DIR="/share/homes/VOAdmin/apps"
SDK_IMAGE="mcr.microsoft.com/dotnet/sdk:10.0"
DEPLOY_DIR="/share/homes/VOAdmin/apps/MercenariesAndBeasts/Mercenaries-and-Beasts/deploy"

# Mapování: název služby → (repo složka, csproj, výstupní složka)
# Formát: "repo_dir|csproj_path|publish_dir"
VO2="$APPS_DIR/VO2DataManager|src/VO2DataManager.Web/VO2DataManager.Web.csproj|/share/Public/BlazorVO2DataManager/publish"
VIN="$APPS_DIR/VINWMIVehicles|src/VINWMIVehicles.Web/VINWMIVehicles.Web.csproj|/share/Public/BlazorVINWMI/publish"
UNI="$APPS_DIR/UniSportManager|src/SportManager.Web/SportManager.Web.csproj|/share/Public/BlazorSportManager/publish"
TOP="$APPS_DIR/TopElevenStats|src/TopElevenStats.Web/TopElevenStats.Web.csproj|/share/Public/BlazorTopEleven/publish"
SIR="$APPS_DIR/SimulateReal|src/SimulateReal.Web/SimulateReal.Web.csproj|/share/Public/BlazorSimulateReal1/publish"
SIG="$APPS_DIR/SimulateGames|src/SimulateGames.Web/SimulateGames.Web.csproj|/share/Public/BlazorSimulateGames/publish"
SIC="$APPS_DIR/SimulateCar|src/SimulateCar.Web/SimulateCar.Web.csproj|/share/Public/BlazorSimulateCar/publish"
ZAB="$APPS_DIR/MyZabbix|src/MyZabbix.Web/MyZabbix.Web.csproj|/share/Public/BlazorMyZabbix/publish"
MAB="$APPS_DIR/MercenariesAndBeasts/Mercenaries-and-Beasts|src/MercenariesAndBeasts.Web/MercenariesAndBeasts.Web.csproj|/share/Public/BlazorMercenariesAndBeasts/publish"

# Mapování název → proměnná
get_entry() {
  case "$1" in
    vo2datamanager)      echo "$VO2" ;;
    vinwmivehicles)      echo "$VIN" ;;
    unisportmanager)     echo "$UNI" ;;
    topelevenst)         echo "$TOP" ;;
    simulatereal)        echo "$SIR" ;;
    simulategames)       echo "$SIG" ;;
    simulatecar)         echo "$SIC" ;;
    myzabbix)            echo "$ZAB" ;;
    mercenariesandbeasts) echo "$MAB" ;;
    *) echo "" ;;
  esac
}

ALL_SERVICES="vo2datamanager vinwmivehicles unisportmanager topelevenst simulatereal simulategames simulatecar myzabbix mercenariesandbeasts"

publish_one() {
  NAME="$1"
  ENTRY="$(get_entry "$NAME")"
  if [ -z "$ENTRY" ]; then
    echo "❌ Neznámá služba: $NAME"
    return 1
  fi

  REPO_DIR="$(echo "$ENTRY" | cut -d'|' -f1)"
  CSPROJ="$(echo "$ENTRY"  | cut -d'|' -f2)"
  PUB_DIR="$(echo "$ENTRY" | cut -d'|' -f3)"

  echo "── $NAME ────────────────────────────────────────────"

  # git pull + submoduly
  echo "  git pull..."
  git -C "$REPO_DIR" pull --ff-only origin main
  git -C "$REPO_DIR" submodule update --remote --init

  # vytvoř výstupní složku
  mkdir -p "$PUB_DIR"

  # dotnet publish uvnitř SDK kontejneru
  echo "  dotnet publish..."
  docker run --rm \
    -v "$REPO_DIR":/src:ro \
    -v "$PUB_DIR":/out \
    "$SDK_IMAGE" \
    dotnet publish "/src/$CSPROJ" \
      -c Release \
      -r linux-x64 \
      --no-self-contained \
      -o /out \
      --nologo \
      -v q

  # restart app kontejneru
  echo "  restart kontejneru..."
  docker compose -f "$DEPLOY_DIR/docker-compose.yml" restart "$NAME"

  echo "  ✅ $NAME hotov"
  echo ""
}

# ── Spuštění ──────────────────────────────────────────────────────────────────
if [ -n "$1" ]; then
  publish_one "$1"
else
  echo "╔══════════════════════════════════════════════════╗"
  echo "║     vo2info.cz — publish všech projektů         ║"
  echo "╚══════════════════════════════════════════════════╝"
  echo ""
  for svc in $ALL_SERVICES; do
    publish_one "$svc"
  done
  echo "✅ Vše hotovo."
fi
