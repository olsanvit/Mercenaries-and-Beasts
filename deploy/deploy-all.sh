#!/bin/sh
# deploy-all.sh — na QNAP: restartuje všechny kontejnery s novými soubory
# (soubory jsou nahrány z Macu přes publish-all.sh)

DEPLOY_DIR="/share/homes/VOAdmin/deploy"

docker compose -f "$DEPLOY_DIR/docker-compose.yml" up -d --remove-orphans

echo ""
docker compose -f "$DEPLOY_DIR/docker-compose.yml" ps
