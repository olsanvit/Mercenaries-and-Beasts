#!/bin/sh
# deploy-all.sh — spustit na QNAP (SSH)
# Stáhne nejnovější obrazy z ghcr.io a restartuje všechny kontejnery
#
# Použití:
#   sh /share/homes/VOAdmin/deploy/deploy-all.sh

DEPLOY_DIR="/share/homes/VOAdmin/deploy"

echo "== Stahuji nové obrazy z ghcr.io =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" pull

echo ""
echo "== Restartuji kontejnery =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" up -d

echo ""
echo "== Hotovo =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" ps --format "table {{.Name}}\t{{.Status}}"
