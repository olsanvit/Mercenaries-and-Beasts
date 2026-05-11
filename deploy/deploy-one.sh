#!/bin/sh
# deploy-one.sh — aktualizuje jeden konkrétní projekt
#
# Použití:
#   sh /share/homes/VOAdmin/deploy/deploy-one.sh vo2datamanager
#
# Dostupné názvy služeb:
#   vo2datamanager  vinwmivehicles  unisportmanager  topelevenst
#   simulatereal    simulategames   simulatecar
#   myzabbix        mercenariesandbeasts

DEPLOY_DIR="/share/homes/VOAdmin/deploy"
SERVICE="$1"

if [ -z "$SERVICE" ]; then
  echo "Použití: $0 <název-služby>"
  exit 1
fi

echo "== Pull: $SERVICE =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" pull "$SERVICE"

echo "== Restart: $SERVICE =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" up -d "$SERVICE"

echo "== Hotovo =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" ps "$SERVICE"
