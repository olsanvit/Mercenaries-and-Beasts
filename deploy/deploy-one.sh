#!/bin/sh
# deploy-one.sh — na QNAP: restartuje jeden kontejner
# Použití: sh deploy-one.sh simulatereal

DEPLOY_DIR="/share/homes/VOAdmin/deploy"
SERVICE="$1"

if [ -z "$SERVICE" ]; then
  echo "Použití: $0 <název-služby>"
  echo "Příklad: $0 simulatereal"
  exit 1
fi

docker compose -f "$DEPLOY_DIR/docker-compose.yml" restart "$SERVICE"
docker compose -f "$DEPLOY_DIR/docker-compose.yml" ps "$SERVICE"
