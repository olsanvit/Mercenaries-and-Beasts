#!/bin/sh
# setup-qnap.sh — PRVNÍ SPUŠTĚNÍ na QNAP (nevyžaduje git)
#
# 1. Na Macu pushni deploy složku na QNAP:
#    scp -r deploy/ VOAdmin@192.168.60.221:/share/homes/VOAdmin/deploy
#
# 2. Na QNAP spusť:
#    sh /share/homes/VOAdmin/deploy/setup-qnap.sh

DEPLOY_DIR="/share/homes/VOAdmin/deploy"
GHCR_USER="olsanvit"

echo "== vo2info.cz — QNAP setup =="
echo ""

# Login do ghcr.io
echo "Přihlášení do ghcr.io ..."
echo "Zadej GitHub Personal Access Token (read:packages):"
read -s TOKEN
echo "$TOKEN" | docker login ghcr.io -u "$GHCR_USER" --password-stdin
echo ""

# Pull všech obrazů
echo "Stahuji obrazy z ghcr.io ..."
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" pull

echo ""
echo "Spouštím kontejnery ..."
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" up -d

echo ""
echo "== Hotovo! Spuštěné kontejnery: =="
docker compose -f "$DEPLOY_DIR/docker-compose.yml" \
  --env-file "$DEPLOY_DIR/.env" ps
