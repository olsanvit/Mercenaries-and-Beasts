#!/bin/sh
# setup-qnap.sh — PRVNÍ SPUŠTĚNÍ na QNAP
# Vytvoří potřebné složky a spustí všechny kontejnery
#
# 1. Na Macu zkopíruj deploy složku:
#    rsync -az deploy/ VOAdmin@192.168.60.221:/share/homes/VOAdmin/deploy/
#
# 2. Na QNAP spusť:
#    sh /share/homes/VOAdmin/deploy/setup-qnap.sh

DEPLOY_DIR="/share/homes/VOAdmin/deploy"

echo "== vo2info.cz — QNAP setup =="
echo ""

# Vytvoř publish složky pro všechny projekty
echo "Vytvářím publish složky..."
for dir in \
  /share/Public/BlazorVO2DataManager/publish \
  /share/Public/BlazorVINWMI/publish \
  /share/Public/BlazorSportManager/publish \
  /share/Public/BlazorTopEleven/publish \
  /share/Public/BlazorSimulateReal1/publish \
  /share/Public/BlazorSimulateGames/publish \
  /share/Public/BlazorSimulateCar/publish \
  /share/Public/BlazorMyZabbix/publish \
  /share/Public/BlazorMercenariesAndBeasts/publish; do
  mkdir -p "$dir"
  echo "  $dir"
done

# Vytvoř Docker síť (pokud neexistuje)
echo ""
echo "Docker síť appnet..."
docker network create appnet 2>/dev/null && echo "  Vytvořena" || echo "  Již existuje"

echo ""
echo "== Setup hotov! =="
echo ""
echo "Teď na Macu:"
echo "  1. Publishni projekty: bash deploy/mac/publish-all.sh"
echo "  2. Zkontroluj DNS záznamy pro *.vo2info.cz → IP tohoto QNAP"
echo ""
echo "Pak na QNAP:"
echo "  docker compose -f $DEPLOY_DIR/docker-compose.yml up -d"
