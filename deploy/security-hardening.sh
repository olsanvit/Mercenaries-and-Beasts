#!/bin/sh
# security-hardening.sh — QNAP Docker security hardening
# Spustit na QNAP: sh /share/homes/VOAdmin/deploy/security-hardening.sh
#
# Co skript dělá:
#   1. Zkontroluje + opraví Watchtower (label filter)
#   2. Zkontroluje + opraví Portainer (svázat na 127.0.0.1)
#   3. Opraví MCP kontejner (přidat security parametry)
#   4. Zkontroluje FTP/Telnet služby
#   5. Vypíše souhrn

DOCKER=/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker
DEPLOY_DIR=/share/homes/VOAdmin/deploy

echo "╔══════════════════════════════════════════════════╗"
echo "║         QNAP Docker Security Hardening          ║"
echo "╚══════════════════════════════════════════════════╝"
echo ""

# ── 1. WATCHTOWER ─────────────────────────────────────────────────────────────
echo "── 1. Watchtower ──────────────────────────────────────────────────────"

if $DOCKER inspect watchtower >/dev/null 2>&1; then
  # Zkontrolovat jestli má label filter
  LABEL_ENABLE=$($DOCKER inspect watchtower --format '{{range .Config.Env}}{{println .}}{{end}}' | grep "WATCHTOWER_LABEL_ENABLE")
  if [ -z "$LABEL_ENABLE" ]; then
    echo "  ⚠️  Watchtower nemá WATCHTOWER_LABEL_ENABLE=true — updatuje VŠECHNY kontejnery!"
    echo "  → Restartuji Watchtower s label filtrem..."
    $DOCKER stop watchtower 2>/dev/null
    $DOCKER rm watchtower 2>/dev/null
    $DOCKER run -d \
      --name watchtower \
      --restart unless-stopped \
      -v /var/run/docker.sock:/var/run/docker.sock \
      -e WATCHTOWER_LABEL_ENABLE=true \
      -e WATCHTOWER_CLEANUP=true \
      -e WATCHTOWER_SCHEDULE="0 0 4 * * *" \
      -e TZ=Europe/Prague \
      --security-opt no-new-privileges:true \
      --cap-drop ALL \
      --pids-limit 100 \
      --log-driver json-file \
      --log-opt max-size=10m \
      --log-opt max-file=3 \
      containrrr/watchtower
    echo "  ✅ Watchtower restartován s label filtrem (aktualizuje jen labeled kontejnery)"
  else
    echo "  ✅ Watchtower má label filter: $LABEL_ENABLE"
  fi
else
  echo "  ℹ️  Watchtower neběží — přeskakuji"
fi

# Přidat watchtower label na MCP kontejner (pokud existuje)
if $DOCKER inspect qnap-game-mcp >/dev/null 2>&1; then
  echo "  ℹ️  qnap-game-mcp existuje — přidám watchtower label při přestavbě"
fi

echo ""

# ── 2. PORTAINER ──────────────────────────────────────────────────────────────
echo "── 2. Portainer ───────────────────────────────────────────────────────"

if $DOCKER inspect portainer >/dev/null 2>&1; then
  # Zkontrolovat na jakém portu a adrese poslouchá
  PORTAINER_PORTS=$($DOCKER inspect portainer --format '{{range $p, $conf := .NetworkSettings.Ports}}{{$p}}->{{range $conf}}{{.HostIp}}:{{.HostPort}}{{end}} {{end}}')
  echo "  Aktuální porty: $PORTAINER_PORTS"

  # Zkontrolovat jestli je na 0.0.0.0
  if echo "$PORTAINER_PORTS" | grep -q "0.0.0.0:9000\|0.0.0.0:9443"; then
    echo "  ⚠️  Portainer dostupný na 0.0.0.0 — přesvázuji na 127.0.0.1..."
    $DOCKER stop portainer 2>/dev/null
    $DOCKER rm portainer 2>/dev/null

    # Získat existující volume
    PORTAINER_VOL=$($DOCKER volume ls -q | grep portainer || echo "portainer_data")

    $DOCKER run -d \
      --name portainer \
      --restart unless-stopped \
      -p 127.0.0.1:9443:9443 \
      -v /var/run/docker.sock:/var/run/docker.sock \
      -v ${PORTAINER_VOL}:/data \
      --security-opt no-new-privileges:true \
      --cap-drop ALL \
      --cap-add CHOWN \
      --cap-add DAC_OVERRIDE \
      --pids-limit 200 \
      --log-driver json-file \
      --log-opt max-size=10m \
      --log-opt max-file=3 \
      portainer/portainer-ce:latest \
      --ssl
    echo "  ✅ Portainer restartován na 127.0.0.1:9443 (HTTPS only)"
    echo "  ℹ️  Pro přístup: SSH tunnel → ssh -L 9443:127.0.0.1:9443 VOAdmin@qnap"
  else
    echo "  ✅ Portainer správně svázán"
  fi
else
  echo "  ℹ️  Portainer neběží — přeskakuji"
fi

echo ""

# ── 3. MCP KONTEJNER ──────────────────────────────────────────────────────────
echo "── 3. MCP server (qnap-game-mcp) ─────────────────────────────────────"

if $DOCKER inspect qnap-game-mcp >/dev/null 2>&1; then
  MCP_IMAGE=$($DOCKER inspect qnap-game-mcp --format '{{.Config.Image}}')
  MCP_STATUS=$($DOCKER inspect qnap-game-mcp --format '{{.State.Status}}')
  echo "  Image: $MCP_IMAGE | Status: $MCP_STATUS"

  # Zkontrolovat security parametry
  PRIV=$($DOCKER inspect qnap-game-mcp --format '{{.HostConfig.Privileged}}')
  SEC_OPT=$($DOCKER inspect qnap-game-mcp --format '{{.HostConfig.SecurityOpt}}')
  MEM=$($DOCKER inspect qnap-game-mcp --format '{{.HostConfig.Memory}}')

  echo "  Privileged: $PRIV | SecurityOpt: $SEC_OPT | Memory: $MEM"

  if [ "$PRIV" = "true" ]; then
    echo "  ❌ KRITICKÉ: MCP kontejner běží v privileged módu!"
  fi

  if [ "$MEM" = "0" ]; then
    echo "  ⚠️  Žádný memory limit — doporučeno 512m"
  fi

  # Uložit aktuální ENV pro přestavbu
  MCP_ENV=$($DOCKER inspect qnap-game-mcp --format '{{range .Config.Env}}{{println .}}{{end}}' | grep -v "^PATH=\|^HOME=\|^HOSTNAME=")
  MCP_PORTS=$($DOCKER inspect qnap-game-mcp --format '{{range $p, $conf := .NetworkSettings.Ports}}{{$p}}={{range $conf}}{{.HostIp}}:{{.HostPort}}{{end}} {{end}}')
  echo "  Porty: $MCP_PORTS"

  echo ""
  echo "  ℹ️  Pro přestavbu MCP s security hardening:"
  echo "  (spusť manuálně — potřebuje DATABASE_URL a AUTH_TOKEN)"
  cat << 'MCP_HELP'

  $DOCKER stop qnap-game-mcp && $DOCKER rm qnap-game-mcp
  $DOCKER run -d \
    --name qnap-game-mcp \
    --restart unless-stopped \
    -p 127.0.0.1:3000:3000 \
    -e DATABASE_URL="$DATABASE_URL" \
    -e AUTH_TOKEN="$AUTH_TOKEN" \
    -v /share/CACHEDEV1_DATA/qnap-game-mcp/uploads:/app/uploads \
    --security-opt no-new-privileges:true \
    --cap-drop ALL \
    --pids-limit 300 \
    --mem-limit 512m \
    --cpus 1.0 \
    --log-driver json-file \
    --log-opt max-size=20m \
    --log-opt max-file=5 \
    --label com.centurylinklabs.watchtower.enable=true \
    qnap-game-mcp:latest

MCP_HELP
else
  echo "  ℹ️  qnap-game-mcp neběží — přeskakuji"
fi

echo ""

# ── 4. FTP / TELNET ───────────────────────────────────────────────────────────
echo "── 4. FTP / Telnet ────────────────────────────────────────────────────"

# Zkontrolovat FTP port 21
FTP_LISTEN=$(netstat -tlnp 2>/dev/null | grep ":21 " || ss -tlnp 2>/dev/null | grep ":21 ")
if [ -n "$FTP_LISTEN" ]; then
  echo "  ❌ FTP (port 21) naslouchá: $FTP_LISTEN"
  echo "  → Vypni FTP v QTS: Control Panel → File Services → FTP → uncheck 'Enable FTP service'"
  echo "  → Nebo spusť: /etc/init.d/Qftpd.sh stop"
else
  echo "  ✅ FTP (port 21) nenaslouchá"
fi

# Zkontrolovat Telnet port 23
TELNET_LISTEN=$(netstat -tlnp 2>/dev/null | grep ":23 " || ss -tlnp 2>/dev/null | grep ":23 ")
if [ -n "$TELNET_LISTEN" ]; then
  echo "  ❌ Telnet (port 23) naslouchá!"
  echo "  → Vypni v QTS: Control Panel → Network & File Services → Telnet"
else
  echo "  ✅ Telnet (port 23) nenaslouchá"
fi

# QNAP web management na portu 8080 / 443
MGMT_8080=$(netstat -tlnp 2>/dev/null | grep ":8080 " | grep -v docker)
if [ -n "$MGMT_8080" ]; then
  echo "  ⚠️  QTS management na portu 8080 (HTTP) — doporučeno HTTPS only"
fi

echo ""

# ── 5. DOCKER DAEMON SECURITY ─────────────────────────────────────────────────
echo "── 5. Docker daemon ───────────────────────────────────────────────────"

# Zkontrolovat jestli Docker naslouchá na TCP (nebezpečné)
DOCKER_TCP=$(netstat -tlnp 2>/dev/null | grep ":2375 \|:2376 ")
if [ -n "$DOCKER_TCP" ]; then
  echo "  ❌ KRITICKÉ: Docker daemon naslouchá na TCP portu!"
  echo "  $DOCKER_TCP"
  echo "  → Přidej do dockerd konfigurace: {\"hosts\": [\"unix:///var/run/docker.sock\"]}"
else
  echo "  ✅ Docker daemon neposlouchá na TCP"
fi

# Zkontrolovat UserNS remap
USERNS=$($DOCKER info 2>/dev/null | grep "userns")
if [ -z "$USERNS" ]; then
  echo "  ⚠️  User namespace remapping není aktivní"
  echo "  → Doporučeno: přidat userns-remap do /etc/docker/daemon.json"
else
  echo "  ✅ User namespace: $USERNS"
fi

echo ""

# ── 6. PŘEHLED VŠECH KONTEJNERŮ ───────────────────────────────────────────────
echo "── 6. Přehled kontejnerů ──────────────────────────────────────────────"
echo ""
printf "  %-30s %-12s %-10s %-8s\n" "KONTEJNER" "STATUS" "MEM_LIMIT" "PRIV"
printf "  %-30s %-12s %-10s %-8s\n" "─────────────────────────────" "──────────" "─────────" "────"

$DOCKER ps -a --format '{{.Names}}' | while read cname; do
  STATUS=$($DOCKER inspect $cname --format '{{.State.Status}}' 2>/dev/null)
  MEM=$($DOCKER inspect $cname --format '{{.HostConfig.Memory}}' 2>/dev/null)
  PRIV=$($DOCKER inspect $cname --format '{{.HostConfig.Privileged}}' 2>/dev/null)

  if [ "$MEM" = "0" ]; then MEM_SHOW="NONE ⚠️"; else MEM_SHOW="${MEM}b"; fi
  if [ "$PRIV" = "true" ]; then PRIV_SHOW="YES ❌"; else PRIV_SHOW="no ✅"; fi

  printf "  %-30s %-12s %-10s %-8s\n" "$cname" "$STATUS" "$MEM_SHOW" "$PRIV_SHOW"
done

echo ""
echo "══════════════════════════════════════════════════════"
echo "✅ Security audit dokončen."
echo ""
echo "Doporučené další kroky:"
echo "  1. Aplikovat hardened docker-compose.yml:"
echo "     docker compose -f $DEPLOY_DIR/docker-compose.yml up -d --force-recreate"
echo "  2. Vypnout FTP v QTS Control Panelu (pokud běží)"
echo "  3. Zkontrolovat Portainer přes SSH tunnel"
echo "  4. Přestavět qnap-game-mcp s security parametry (viz sekce 3)"
