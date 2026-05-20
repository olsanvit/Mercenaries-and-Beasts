#!/bin/sh
# mcp-deploy.sh — Nasadí qnap-game-mcp s kompletním security hardeningem
# Spustit na QNAP: sh /share/homes/VOAdmin/deploy/mcp-deploy.sh
#
# Předpoklady:
#   - server.js nahraný do /share/CACHEDEV1_DATA/qnap-game-mcp/server.js
#   - ENV proměnné DATABASE_URL a AUTH_TOKEN nastaveny

DOCKER=/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker
MCP_DIR=/share/CACHEDEV1_DATA/qnap-game-mcp
MCP_IMAGE=qnap-game-mcp

echo "── MCP Server Deploy ───────────────────────────────────────────────────"

# Ověřit že server.js existuje
if [ ! -f "$MCP_DIR/server.js" ]; then
  echo "❌ Chybí $MCP_DIR/server.js"
  echo "   Nakopíruj ho z Macu: scp server.js VOAdmin@192.168.60.221:$MCP_DIR/server.js"
  exit 1
fi

# Ověřit ENV vars
if [ -z "$DATABASE_URL" ] || [ -z "$AUTH_TOKEN" ]; then
  # Zkusit načíst z env souboru
  if [ -f "$MCP_DIR/.env" ]; then
    . "$MCP_DIR/.env"
    echo "  ℹ️  ENV načteny z $MCP_DIR/.env"
  else
    echo "❌ Chybí DATABASE_URL nebo AUTH_TOKEN"
    echo "   Nastav je nebo vytvoř $MCP_DIR/.env"
    exit 1
  fi
fi

# Zkontrolovat jestli je třeba rebuild
CURRENT_VERSION=""
if $DOCKER inspect $MCP_IMAGE-container >/dev/null 2>&1; then
  CURRENT_VERSION=$($DOCKER exec $MCP_IMAGE-container node -e "const s=require('./server.js'); console.log(s.MCP_VERSION||'')" 2>/dev/null || true)
fi

echo "  Aktuální verze: ${CURRENT_VERSION:-neznámá}"
echo "  Nová verze v server.js: $(grep 'MCP_VERSION' $MCP_DIR/server.js | head -1)"

# Build image
echo ""
echo "  Building image..."
cat > "$MCP_DIR/Dockerfile" << 'DOCKERFILE'
FROM node:22-alpine
RUN addgroup -S mcpgroup && adduser -S mcpuser -G mcpgroup
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY server.js ./
RUN mkdir -p /app/uploads && chown mcpuser:mcpgroup /app/uploads
USER mcpuser
EXPOSE 3000
CMD ["node", "server.js"]
DOCKERFILE

$DOCKER build -t $MCP_IMAGE "$MCP_DIR"

# Stop + remove old container
echo "  Stopping old container..."
$DOCKER stop qnap-game-mcp 2>/dev/null || true
$DOCKER rm qnap-game-mcp 2>/dev/null || true

# Run new container with security hardening
echo "  Starting new container..."
$DOCKER run -d \
  --name qnap-game-mcp \
  --restart unless-stopped \
  -p 127.0.0.1:3000:3000 \
  -e DATABASE_URL="$DATABASE_URL" \
  -e AUTH_TOKEN="$AUTH_TOKEN" \
  -e TZ="Europe/Prague" \
  -v "$MCP_DIR/uploads:/app/uploads" \
  --security-opt no-new-privileges:true \
  --cap-drop ALL \
  --pids-limit 300 \
  -m 512m \
  --cpus 1.0 \
  --log-driver json-file \
  --log-opt max-size=20m \
  --log-opt max-file=5 \
  --label com.centurylinklabs.watchtower.enable=true \
  $MCP_IMAGE

echo ""
# Verify
sleep 3
STATUS=$($DOCKER inspect qnap-game-mcp --format '{{.State.Status}}' 2>/dev/null)
if [ "$STATUS" = "running" ]; then
  VERSION=$(curl -s --max-time 3 http://127.0.0.1:3000/version 2>/dev/null | grep -o '"version":"[^"]*"' | cut -d'"' -f4)
  echo "✅ qnap-game-mcp běží | verze: ${VERSION:-nezjištěna}"
else
  echo "❌ Kontejner se nespustil (status: $STATUS)"
  $DOCKER logs --tail 20 qnap-game-mcp
  exit 1
fi
