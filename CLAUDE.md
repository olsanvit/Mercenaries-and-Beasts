# Mercenaries-and-Beasts

Fantasy RPG hra — najimani zoldneru, boj v kobkach a lokacich, obchod s predmety — .NET 10 Blazor Server + MAUI.

## Technologie

- .NET 10, Blazor Server, `@rendermode InteractiveServer`
- Entity Framework Core — `AppDbContextMercenariesAndBeasts` (pres SharedServices)
- `Blazored.Typeahead` — autocomplete vyhledavani
- Lokalizace: `@S["klic"]` pattern (IStringLocalizer)
- Bootstrap 5, Bootstrap Icons
- `SharedServices` — git submodule
- `ThemePicker` — pouze pro roli Admin

## Struktura projektu

```
src/
  MercenariesAndBeasts.Web/
    Components/
      Pages/
        Admin/
          Admin.razor                  # /admin (Roles=Admin — seed, nastaveni hry)
          ItemCategoriesAdmin.razor    # /admin/item-categories
          ItemsAdmin.razor             # /admin/items-admin
          TemplatesActivation.razor    # /admin/templates-activation
          UsersAdmin.razor             # /admin/users
        Dungeon/
          Dungeons.razor               # /dungeons (vypis kobek)
          DungeonFight.razor           # /dungeon/{id}/fight
          DungeonsAdmin.razor          # /admin/dungeons-admin
        Location/
          Locations.razor              # /locations (vypis lokaci)
          LocationFight.razor          # /location/{id}/fight
          LocationsAdmin.razor         # /admin/locations-admin
        Dashboard.razor                # / (hlavni dashboard hrace)
        MercenariesPanel.razor         # /mercenaries
        BeastsPanel.razor              # /beasts
        Shop.razor                     # /shop
        Profile.razor                  # /profile
        UserCountryBadge.razor         # sdilena komponenta
  MercenariesAndBeasts.Tests/
  MercenariesAndBeasts.Mobile/         # MAUI
  SharedServices/                      # git submodule
```

## Navigace

Home (Dashboard), Dungeons, Expeditions/Locations, Shop, Profile | Admin, Locations Admin, Dungeons Admin, Items Admin, Item Categories, Admin/Users

## Klicove domeny (MercenariesAndBeasts.Domain)

- `Items` — predmety, kategorie predmetu
- `Localization` — preklady (@S["key"])
- `Utils` — pomocne nastroje
- `MercenariesAndBeasts.Infrastructure` — DbContext, repozitare

## Auth a role

- Vsechny stranky: `@attribute [Authorize]`
- Admin sekce: `@attribute [Authorize(Roles = "Admin")]`
- `ThemePicker` dostupny pouze pro Admin roli

## Konvence

- Lokalizace: `@inject IStringLocalizer<SharedResource> S` + `@S["klic"]`
- `Blazored.Typeahead` pro vyhledavaci pole (napr. hledani predmetu, zoldneru)
- Admin seed: `RunSeedAsync(bool full)` — plny nebo minimalny seed (lokace, kobky, predmety, AI generace)
- Filter/search: `_search` string + `@bind:event="oninput"` + filtrovani v code bloku
