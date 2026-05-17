# Mercenaries-and-Beasts

Blazor Server aplikace — RPG/fantasy hra s inventory systémem, upgrady itemů a Google OAuth přihlašováním.

## Rychlý start (Development)

### 1. User Secrets (povinné)

Projekt používá `appsettings.Development.json` s placeholdery. Skutečné hodnoty se musí nastavit přes [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```bash
cd src/MercenariesAndBeasts.Web

# Google OAuth (z Google Cloud Console → Credentials)
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_REAL_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_REAL_CLIENT_SECRET"

# OpenAI (z platform.openai.com/api-keys)
dotnet user-secrets set "OpenAI:ApiKey" "sk-proj-..."

# DB heslo (pokud se liší od výchozího)
dotnet user-secrets set "ConnectionStrings:QNAPGameDatabase" "Host=localhost;Port=5432;Database=mercs_beasts;Username=postgres;Password=REAL_PASSWORD;Include Error Detail=true"

# Seed admin účet
dotnet user-secrets set "SeedAdmins:0:Email" "admin@yourdomain.com"
dotnet user-secrets set "SeedAdmins:0:Password" "YourSecurePassword123!"
```

UserSecretsId: `mercenaries-and-beasts-dev`

### 2. Databáze

```bash
# Aplikuj EF Core migrace
dotnet ef database update --project src/MercenariesAndBeasts.Web
```

### 3. Spuštění

```bash
dotnet run --project src/MercenariesAndBeasts.Web
```

## Architektura

- **Blazor Server** (.NET 10) s InteractiveServer render modem
- **PostgreSQL** přes Npgsql + EF Core
- **Identity** (ASP.NET Core Identity + Google OAuth)
- **SharedServices** jako git submodul — sdíleno s ostatními projekty
- **OpenAI** API pro AI asistenta

## Klíčové funkce

- Inventory systém s kategoriemi itemů
- Upgrade systém (PlayerItemPieces jako měna)
- Google OAuth + lokální přihlášení
- Admin dashboard (správa uživatelů, seedování dat)
