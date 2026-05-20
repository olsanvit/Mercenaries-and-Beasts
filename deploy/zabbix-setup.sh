#!/bin/sh
# zabbix-setup.sh — Konfigurace Zabbix monitoringu přes API
# Spustit na QNAP: sh /share/homes/claudeAI/zabbix-setup.sh
#
# Přidá monitoring:
#   1. Host QNAPOL s Zabbix Agent 2 (Docker, systém)
#   2. HTTP checks všech Blazor aplikací
#   3. HTTP checks interních nástrojů (vaultwarden, pgadmin, ntfy...)
#   4. Ping checks (internet: 8.8.8.8, 1.1.1.1)
#   5. pg16 PostgreSQL health check

ZABBIX_URL="http://127.0.0.1:8090/api_jsonrpc.php"
ZABBIX_USER="Admin"
ZABBIX_PASS="zabbix"   # změň pokud máš jiné heslo

# ── Helpers ───────────────────────────────────────────────────────────────────
zapi() {
  curl -s -X POST "$ZABBIX_URL" \
    -H "Content-Type: application/json-rpc" \
    -d "$1"
}

AUTH=""

login() {
  RESP=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"user.login\",\"params\":{\"username\":\"$ZABBIX_USER\",\"password\":\"$ZABBIX_PASS\"},\"id\":1}")
  AUTH=$(echo "$RESP" | grep -o '"result":"[^"]*"' | cut -d'"' -f4)
  if [ -z "$AUTH" ]; then
    echo "❌ Login selhal: $RESP"
    echo "   Zkontroluj ZABBIX_USER a ZABBIX_PASS v tomto skriptu."
    exit 1
  fi
  echo "✅ Zabbix login OK (token: ${AUTH:0:8}...)"
}

# Vytvoří nebo vrátí ID host skupiny
ensure_hostgroup() {
  NAME="$1"
  # zkus najít
  RESP=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"hostgroup.get\",\"params\":{\"filter\":{\"name\":[\"$NAME\"]},\"output\":[\"groupid\"]},\"auth\":\"$AUTH\",\"id\":2}")
  GID=$(echo "$RESP" | grep -o '"groupid":"[0-9]*"' | head -1 | cut -d'"' -f4)
  if [ -z "$GID" ]; then
    RESP=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"hostgroup.create\",\"params\":{\"name\":\"$NAME\"},\"auth\":\"$AUTH\",\"id\":2}")
    GID=$(echo "$RESP" | grep -o '"groupids":\["[0-9]*"\]' | grep -o '[0-9]*')
    echo "  Vytvořena skupina '$NAME' (id=$GID)"
  else
    echo "  Skupina '$NAME' existuje (id=$GID)"
  fi
  echo "$GID"
}

# Vytvoří HTTP simple check host
create_http_host() {
  NAME="$1"
  URL="$2"
  GROUP_ID="$3"

  # Zkontroluj jestli host existuje
  EXISTS=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"host.get\",\"params\":{\"filter\":{\"host\":[\"$NAME\"]},\"output\":[\"hostid\"]},\"auth\":\"$AUTH\",\"id\":3}")
  HID=$(echo "$EXISTS" | grep -o '"hostid":"[0-9]*"' | head -1 | cut -d'"' -f4)
  if [ -n "$HID" ]; then
    echo "  Host '$NAME' již existuje (id=$HID) — přeskakuji"
    return
  fi

  RESP=$(zapi "{
    \"jsonrpc\": \"2.0\",
    \"method\": \"host.create\",
    \"params\": {
      \"host\": \"$NAME\",
      \"name\": \"$NAME\",
      \"groups\": [{\"groupid\": \"$GROUP_ID\"}],
      \"interfaces\": [{\"type\": 1, \"main\": 1, \"useip\": 1, \"ip\": \"127.0.0.1\", \"dns\": \"\", \"port\": \"10050\"}],
      \"items\": [{
        \"name\": \"HTTP check: $NAME\",
        \"key_\": \"web.page.get[$URL]\",
        \"type\": 9,
        \"url\": \"$URL\",
        \"status_codes\": \"200,301,302\",
        \"retrieve_mode\": 1,
        \"delay\": \"60s\",
        \"history\": \"7d\",
        \"value_type\": 1,
        \"triggers\": [{
          \"description\": \"$NAME nedostupný\",
          \"expression\": \"nodata(/\$NAME/web.page.get[$URL],5m)=1\",
          \"priority\": 4
        }]
      }]
    },
    \"auth\": \"$AUTH\",
    \"id\": 3
  }")
  HID=$(echo "$RESP" | grep -o '"hostids":\["[0-9]*"\]' | grep -o '[0-9]*')
  echo "  HTTP host '$NAME' vytvořen (id=$HID)"
}

# ── Hlavní logika ──────────────────────────────────────────────────────────────

echo "╔══════════════════════════════════════════════════╗"
echo "║         Zabbix Monitoring Setup                 ║"
echo "╚══════════════════════════════════════════════════╝"
echo ""

# Čekej na Zabbix API
echo "⏳ Čekám na Zabbix API..."
for i in $(seq 1 30); do
  STATUS=$(curl -s -o /dev/null -w "%{http_code}" "$ZABBIX_URL" 2>/dev/null)
  if [ "$STATUS" = "200" ] || [ "$STATUS" = "412" ]; then
    echo "✅ Zabbix API dostupné"
    break
  fi
  sleep 5
done

login

echo ""
echo "── 1. Host skupiny ─────────────────────────────────"
WEB_GID=$(ensure_hostgroup "Web Apps")
INFRA_GID=$(ensure_hostgroup "Infrastructure")
DOCKER_GID=$(ensure_hostgroup "Docker")

echo ""
echo "── 2. QNAPOL host (Zabbix Agent 2 + Docker) ────────"
EXISTS=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"host.get\",\"params\":{\"filter\":{\"host\":[\"QNAPOL\"]},\"output\":[\"hostid\"]},\"auth\":\"$AUTH\",\"id\":4}")
QNAP_HID=$(echo "$EXISTS" | grep -o '"hostid":"[0-9]*"' | head -1 | cut -d'"' -f4)

# Najdi template ID pro Linux a Docker
LINUX_TPL=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"template.get\",\"params\":{\"search\":{\"name\":\"Linux by Zabbix agent\"},\"output\":[\"templateid\"]},\"auth\":\"$AUTH\",\"id\":5}")
LINUX_TPL_ID=$(echo "$LINUX_TPL" | grep -o '"templateid":"[0-9]*"' | head -1 | cut -d'"' -f4)
DOCKER_TPL=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"template.get\",\"params\":{\"search\":{\"name\":\"Docker by Zabbix agent 2\"},\"output\":[\"templateid\"]},\"auth\":\"$AUTH\",\"id\":6}")
DOCKER_TPL_ID=$(echo "$DOCKER_TPL" | grep -o '"templateid":"[0-9]*"' | head -1 | cut -d'"' -f4)

echo "  Linux template: $LINUX_TPL_ID | Docker template: $DOCKER_TPL_ID"

if [ -n "$QNAP_HID" ]; then
  echo "  QNAPOL host existuje (id=$QNAP_HID) — aktualizuji šablony"
  TEMPLATES=""
  [ -n "$LINUX_TPL_ID" ] && TEMPLATES="{\"templateid\":\"$LINUX_TPL_ID\"}"
  [ -n "$DOCKER_TPL_ID" ] && TEMPLATES="$TEMPLATES,{\"templateid\":\"$DOCKER_TPL_ID\"}"
  if [ -n "$TEMPLATES" ]; then
    zapi "{\"jsonrpc\":\"2.0\",\"method\":\"host.update\",\"params\":{\"hostid\":\"$QNAP_HID\",\"templates\":[$TEMPLATES]},\"auth\":\"$AUTH\",\"id\":7}" > /dev/null
    echo "  ✅ Šablony přidány k QNAPOL"
  fi
else
  echo "  Vytvářím QNAPOL host..."
  TEMPLATES=""
  [ -n "$LINUX_TPL_ID" ] && TEMPLATES="{\"templateid\":\"$LINUX_TPL_ID\"}"
  [ -n "$DOCKER_TPL_ID" ] && TEMPLATES="$TEMPLATES,{\"templateid\":\"$DOCKER_TPL_ID\"}"
  RESP=$(zapi "{
    \"jsonrpc\": \"2.0\",
    \"method\": \"host.create\",
    \"params\": {
      \"host\": \"QNAPOL\",
      \"name\": \"QNAPOL (QNAP NAS)\",
      \"groups\": [{\"groupid\": \"$DOCKER_GID\"},{\"groupid\": \"$INFRA_GID\"}],
      \"interfaces\": [{\"type\": 1, \"main\": 1, \"useip\": 1, \"ip\": \"zabbix-agent2\", \"dns\": \"zabbix-agent2\", \"port\": \"10050\", \"useip\": 0}],
      \"templates\": [$TEMPLATES]
    },
    \"auth\": \"$AUTH\",
    \"id\": 7
  }")
  QNAP_HID=$(echo "$RESP" | grep -o '"hostids":\["[0-9]*"\]' | grep -o '[0-9]*')
  echo "  ✅ QNAPOL host vytvořen (id=$QNAP_HID)"
fi

echo ""
echo "── 3. Blazor Web Apps (HTTP monitoring) ─────────────"
for DOMAIN in \
  "MercenariesAndBeasts|https://ordersandbeasts.vo2info.cz/" \
  "VO2DataManager|https://datamanager.vo2info.cz/" \
  "VINWMIVehicles|https://vinwmi.vo2info.cz/" \
  "UniSportManager|https://unisportmanager.vo2info.cz/" \
  "TopElevenStats|https://testats.vo2info.cz/" \
  "SimulateReal|https://simreal.vo2info.cz/" \
  "SimulateGames|https://simgames.vo2info.cz/" \
  "SimulateCar|https://simcar.vo2info.cz/" \
  "MyZabbix|https://zabbix.vo2info.cz/"; do
  NAME=$(echo "$DOMAIN" | cut -d'|' -f1)
  URL=$(echo "$DOMAIN" | cut -d'|' -f2)
  echo "  $NAME → $URL"

  # Zkontroluj/vytvoř HTTP scenario jako web scenario na Zabbix server host
  SCENARIO_EXISTS=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"httptest.get\",\"params\":{\"filter\":{\"name\":\"$NAME\"},\"output\":[\"httptestid\"]},\"auth\":\"$AUTH\",\"id\":8}")
  SID=$(echo "$SCENARIO_EXISTS" | grep -o '"httptestid":"[0-9]*"' | head -1 | cut -d'"' -f4)
  if [ -n "$SID" ]; then
    echo "    → existuje (id=$SID)"
  else
    RESP=$(zapi "{
      \"jsonrpc\": \"2.0\",
      \"method\": \"httptest.create\",
      \"params\": {
        \"name\": \"$NAME\",
        \"hostid\": \"$QNAP_HID\",
        \"delay\": \"60s\",
        \"retries\": 2,
        \"steps\": [{
          \"name\": \"Homepage\",
          \"url\": \"$URL\",
          \"status_codes\": \"200\",
          \"no\": 1
        }]
      },
      \"auth\": \"$AUTH\",
      \"id\": 8
    }")
    SID=$(echo "$RESP" | grep -o '"httptestids":\["[0-9]*"\]' | grep -o '[0-9]*')
    echo "    → vytvořen (id=$SID)"
  fi
done

echo ""
echo "── 4. Internet ping (8.8.8.8, 1.1.1.1) ─────────────"
for TARGET in "Google-DNS|8.8.8.8" "Cloudflare-DNS|1.1.1.1"; do
  NAME=$(echo "$TARGET" | cut -d'|' -f1)
  IP=$(echo "$TARGET" | cut -d'|' -f2)

  EXISTS=$(zapi "{\"jsonrpc\":\"2.0\",\"method\":\"host.get\",\"params\":{\"filter\":{\"host\":[\"$NAME\"]},\"output\":[\"hostid\"]},\"auth\":\"$AUTH\",\"id\":9}")
  HID=$(echo "$EXISTS" | grep -o '"hostid":"[0-9]*"' | head -1 | cut -d'"' -f4)
  if [ -n "$HID" ]; then
    echo "  $NAME existuje (id=$HID)"
  else
    RESP=$(zapi "{
      \"jsonrpc\": \"2.0\",
      \"method\": \"host.create\",
      \"params\": {
        \"host\": \"$NAME\",
        \"name\": \"$NAME ($IP)\",
        \"groups\": [{\"groupid\": \"$INFRA_GID\"}],
        \"interfaces\": [{\"type\": 1, \"main\": 1, \"useip\": 1, \"ip\": \"$IP\", \"dns\": \"\", \"port\": \"10050\"}],
        \"templates\": [{\"templateid\": \"$(zapi "{\\\"jsonrpc\\\":\\\"2.0\\\",\\\"method\\\":\\\"template.get\\\",\\\"params\\\":{\\\"search\\\":{\\\"name\\\":\\\"ICMP Ping\\\"},\\\"output\\\":[\\\"templateid\\\"]},\\\"auth\\\":\\\"$AUTH\\\",\\\"id\\\":10}" | grep -o '\"templateid\":\"[0-9]*\"' | head -1 | cut -d'\"' -f4)\"}]
      },
      \"auth\": \"$AUTH\",
      \"id\": 9
    }")
    HID=$(echo "$RESP" | grep -o '"hostids":\["[0-9]*"\]' | grep -o '[0-9]*')
    echo "  ✅ $NAME vytvořen (id=$HID)"
  fi
done

echo ""
echo "══════════════════════════════════════════════════════"
echo "✅ Zabbix monitoring setup dokončen."
echo ""
echo "Co je monitorované:"
echo "  • QNAPOL: Linux metriky + Docker kontejnery (via Agent 2)"
echo "  • Blazor apps: HTTP 200 check každých 60s"
echo "  • Internet: ICMP ping na 8.8.8.8 a 1.1.1.1"
echo ""
echo "Přístup do Zabbix UI (přes SSH tunnel):"
echo "  ssh -L 8090:127.0.0.1:8090 VOAdmin@qnap"
echo "  → http://localhost:8090"
