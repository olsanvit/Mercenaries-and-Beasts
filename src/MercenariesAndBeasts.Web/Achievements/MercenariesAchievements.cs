using SharedServices.Models.Achievement;

namespace MercenariesAndBeasts.Web.Achievements;

public static class MercenariesAchievements
{
    public static readonly IReadOnlyList<AchievementDef> All = new List<AchievementDef>
    {
        // ── Mercenarky ────────────────────────────────────────────────────────
        new("MB_MERC_FIRST",          "První mercenář",       "Zobraz prvního mercenáře",                             "bi-person-badge",         10, "Mercenarky"),
        new("MB_MERC_PANEL",          "Panel mercenářů",      "Otevři panel mercenářů",                               "bi-people",                5, "Mercenarky"),
        new("MB_MERC_HIRE",           "Nábor",                "Naber prvního mercenáře",                              "bi-person-plus-fill",     15, "Mercenarky"),
        new("MB_MERC_HIRE_5",         "5 mercenářů",          "Naber 5 mercenářů",                                    "bi-people-fill",          25, "Mercenarky"),
        new("MB_MERC_HIRE_10",        "10 mercenářů",         "Naber 10 mercenářů",                                   "bi-person-lines-fill",    40, "Mercenarky"),
        new("MB_MERC_HIRE_25",        "25 mercenářů",         "Naber 25 mercenářů",                                   "bi-collection",           75, "Mercenarky"),
        new("MB_MERC_LEVEL_5",        "Zkušený mercenář",     "Vytrénuj mercenáře na level 5",                        "bi-arrow-up-circle",      20, "Mercenarky"),
        new("MB_MERC_LEVEL_10",       "Veterán",              "Vytrénuj mercenáře na level 10",                       "bi-award",                35, "Mercenarky"),
        new("MB_MERC_LEVEL_20",       "Elita",                "Vytrénuj mercenáře na level 20",                       "bi-award-fill",           60, "Mercenarky"),
        new("MB_MERC_DISMISS",        "Propustit",            "Propusť mercenáře",                                    "bi-person-dash",          10, "Mercenarky"),
        new("MB_MERC_WARRIOR",        "Bojovník",             "Naber bojovníka do skupiny",                           "bi-shield-shaded",        15, "Mercenarky"),
        new("MB_MERC_MAGE",           "Mág",                  "Naber mága do skupiny",                                "bi-stars",                15, "Mercenarky"),
        new("MB_MERC_ROGUE",          "Zloděj",               "Naber zloděje do skupiny",                             "bi-incognito",            15, "Mercenarky"),
        new("MB_MERC_HEALER",         "Léčitel",              "Naber léčitele do skupiny",                            "bi-heart-pulse",          15, "Mercenarky"),
        new("MB_MERC_FULL_PARTY",     "Plná skupina",         "Naber plnou skupinu mercenářů",                        "bi-person-fill",          30, "Mercenarky"),
        new("MB_MERC_EQUIP",          "Vybavení",             "Vybaví mercenáře zbraní",                              "bi-hammer",               20, "Mercenarky"),
        new("MB_MERC_MAX_LEVEL",      "Legendární mercenář",  "Vytrénuj mercenáře na maximální level",                "bi-trophy",               80, "Mercenarky"),
        new("MB_MERC_RENAME",         "Pojmenování",          "Přejmenuj mercenáře",                                   "bi-pencil",               10, "Mercenarky"),
        new("MB_MERC_PROFILE",        "Profil",               "Zobraz profil mercenáře",                              "bi-person-circle",        10, "Mercenarky"),
        new("MB_MERC_ALL_CLASSES",    "Všechny třídy",        "Naber mercenáře každé třídy",                          "bi-collection-fill",      40, "Mercenarky"),

        // ── Bestie ────────────────────────────────────────────────────────────
        new("MB_BEAST_FIRST",         "První bestie",         "Zobraz první bestii",                                  "bi-bug",                  10, "Bestie"),
        new("MB_BEAST_PANEL",         "Panel bestií",         "Otevři panel bestií",                                   "bi-grid",                  5, "Bestie"),
        new("MB_BEAST_CAPTURE",       "Chytit bestii",        "Chyť první bestii",                                    "bi-minecart-loaded",      20, "Bestie"),
        new("MB_BEAST_CAPTURE_5",     "5 bestií",             "Chyť 5 bestií",                                        "bi-collection",           35, "Bestie"),
        new("MB_BEAST_CAPTURE_10",    "10 bestií",            "Chyť 10 bestií",                                       "bi-collection-fill",      50, "Bestie"),
        new("MB_BEAST_RARE",          "Vzácná bestie",        "Chyť vzácnou bestii",                                  "bi-gem",                  40, "Bestie"),
        new("MB_BEAST_LEGENDARY",     "Legendární bestie",    "Chyť legendární bestii",                               "bi-trophy-fill",          80, "Bestie"),
        new("MB_BEAST_TRAIN",         "Trénink",              "Vytrénuj bestii",                                       "bi-arrow-up-circle",      15, "Bestie"),
        new("MB_BEAST_LEVEL_10",      "Silná bestie",         "Vytrénuj bestii na level 10",                          "bi-arrow-up-circle-fill", 30, "Bestie"),
        new("MB_BEAST_DRAGON",        "Drak",                 "Chyť draka",                                           "bi-fire",                 50, "Bestie"),
        new("MB_BEAST_UNDEAD",        "Nemrtvý",              "Chyť nemrtvou bestii",                                  "bi-moon-stars-fill",      25, "Bestie"),
        new("MB_BEAST_WATER",         "Vodní tvor",           "Chyť vodní bestii",                                    "bi-droplet",              25, "Bestie"),
        new("MB_BEAST_SMALL",         "Malá bestie",          "Chyť malou bestii",                                    "bi-bug",                  10, "Bestie"),
        new("MB_BEAST_GIANT",         "Obr",                  "Chyť obřího tvora",                                    "bi-building-fill",        30, "Bestie"),
        new("MB_BEAST_ALL_TYPES",     "Bestiář",              "Chyť bestie všech typů",                               "bi-journal-richtext",     60, "Bestie"),

        // ── Dungeon ───────────────────────────────────────────────────────────
        new("MB_DUNG_FIRST",          "První dungeon",        "Vstup do prvního dungeonu",                            "bi-door-open",            10, "Dungeon"),
        new("MB_DUNG_COMPLETE",       "Dungeon vyčištěn",     "Dokončí dungeon",                                       "bi-check-circle-fill",    25, "Dungeon"),
        new("MB_DUNG_5",              "5 dungeonů",           "Dokončí 5 dungeonů",                                    "bi-map",                  40, "Dungeon"),
        new("MB_DUNG_10",             "10 dungeonů",          "Dokončí 10 dungeonů",                                   "bi-map-fill",             60, "Dungeon"),
        new("MB_DUNG_25",             "25 dungeonů",          "Dokončí 25 dungeonů",                                   "bi-signpost-2",           80, "Dungeon"),
        new("MB_DUNG_BOSS",           "Zabití bosse",         "Poraz prvního bosse",                                  "bi-lightning-fill",       30, "Dungeon"),
        new("MB_DUNG_NO_DEATH",       "Bez ztráty",           "Dokončí dungeon bez ztráty mercenáře",                  "bi-shield-fill-check",    35, "Dungeon"),
        new("MB_DUNG_RARE_LOOT",      "Vzácná kořist",        "Získej vzácný loot z dungeonu",                        "bi-box-seam-fill",        30, "Dungeon"),
        new("MB_DUNG_LEGENDARY_LOOT", "Legendární kořist",    "Získej legendární loot z dungeonu",                    "bi-gift-fill",            60, "Dungeon"),
        new("MB_DUNG_QUICK",          "Rychlý průchod",       "Dokončí dungeon za méně než 5 minut",                  "bi-lightning",            25, "Dungeon"),
        new("MB_DUNG_ELITE",          "Elitní výzva",         "Dokončí elitní dungeon",                               "bi-exclamation-diamond",  50, "Dungeon"),
        new("MB_DUNG_LOCATION",       "Lokace",               "Navštiv stránku lokací",                               "bi-geo-alt",               5, "Dungeon"),
        new("MB_DUNG_FLOOR_5",        "5. patro",             "Sestupni na 5. patro dungeonu",                        "bi-arrow-down",           20, "Dungeon"),
        new("MB_DUNG_FLOOR_10",       "10. patro",            "Sestupni na 10. patro dungeonu",                       "bi-arrow-down-circle",    35, "Dungeon"),
        new("MB_DUNG_EXPLORE_ALL",    "Vše prozkoumáno",      "Prozkoumej celý dungeon",                              "bi-compass",              40, "Dungeon"),

        // ── Obchod ────────────────────────────────────────────────────────────
        new("MB_SHOP_FIRST",          "První nákup",          "Nakup první předmět v obchodě",                        "bi-cart",                 10, "Obchod"),
        new("MB_SHOP_VISIT",          "Obchod",               "Navštiv obchod",                                       "bi-shop",                  5, "Obchod"),
        new("MB_SHOP_BUY_5",          "5 nákupů",             "Nakup 5 různých předmětů",                             "bi-bag",                  20, "Obchod"),
        new("MB_SHOP_BUY_25",         "25 nákupů",            "Nakup 25 různých předmětů",                            "bi-bag-fill",             40, "Obchod"),
        new("MB_SHOP_WEAPON",         "Zbraň",                "Nakup zbraň v obchodě",                                "bi-hammer",               15, "Obchod"),
        new("MB_SHOP_ARMOR",          "Brnění",               "Nakup brnění v obchodě",                               "bi-shield",               15, "Obchod"),
        new("MB_SHOP_POTION",         "Lektvar",              "Nakup lektvar v obchodě",                              "bi-droplet",              10, "Obchod"),
        new("MB_SHOP_LEGENDARY",      "Legendární předmět",   "Nakup legendární předmět",                             "bi-gem",                  50, "Obchod"),
        new("MB_SHOP_GOLD_1000",      "1 000 zlatých",        "Utrati 1 000 zlatých",                                 "bi-coin",                 20, "Obchod"),
        new("MB_SHOP_GOLD_10000",     "10 000 zlatých",       "Utrati 10 000 zlatých",                                "bi-currency-exchange",    50, "Obchod"),
        new("MB_SHOP_PROFILE",        "Profil hráče",         "Navštiv stránku profilu",                              "bi-person-circle",         5, "Obchod"),
        new("MB_SHOP_FULL_GEAR",      "Plné vybavení",        "Vybaví celou skupinu",                                 "bi-boxes",                45, "Obchod"),
        new("MB_SHOP_BARGAIN",        "Výhodná koupě",        "Nakup předmět se slevou",                              "bi-percent",              15, "Obchod"),
        new("MB_SHOP_SELL",           "Prodej",               "Prodej předmět v obchodě",                             "bi-arrow-left-right",     10, "Obchod"),
        new("MB_SHOP_REVISIT",        "Obchodník",            "Navštiv obchod 10×",                                   "bi-arrow-repeat",         10, "Obchod"),

        // ── Speciální ─────────────────────────────────────────────────────────
        new("MB_SPECIAL_WELCOME",     "Vítej, hrdino!",       "Otevři aplikaci poprvé",                               "bi-door-open",             5, "Speciální"),
        new("MB_SPECIAL_DASHBOARD",   "Dashboard",            "Navštiv hlavní dashboard",                             "bi-house",                 5, "Speciální"),
        new("MB_SPECIAL_ACHI_PAGE",   "Sběratel",             "Otevři stránku achievementů",                          "bi-trophy-fill",           5, "Speciální"),
        new("MB_SPECIAL_ACHI_10",     "10 achievementů",      "Odemkni 10 achievementů",                              "bi-award",                20, "Speciální"),
        new("MB_SPECIAL_ACHI_25",     "25 achievementů",      "Odemkni 25 achievementů",                              "bi-award-fill",           40, "Speciální"),
        new("MB_SPECIAL_ACHI_50",     "50 achievementů",      "Odemkni 50 achievementů",                              "bi-stars",                75, "Speciální"),
        new("MB_SPECIAL_ACHI_ALL",    "Platinový hrdina",     "Odemkni všechny achievementy",                         "bi-gem",                 200, "Speciální"),
        new("MB_SPECIAL_500_PTS",     "500 bodů",             "Nashromáždi 500 bodů",                                 "bi-circle-fill",          30, "Speciální"),
        new("MB_SPECIAL_1000_PTS",    "1 000 bodů",           "Nashromáždi 1000 bodů",                                "bi-pentagon-fill",        60, "Speciální"),
        new("MB_SPECIAL_NIGHT_OWL",   "Noční hráč",           "Hraj po půlnoci",                                      "bi-moon-stars",           15, "Speciální"),
        new("MB_SPECIAL_EARLY_BIRD",  "Ranní hrdina",         "Hraj před 7. hodinou",                                 "bi-sunrise",              15, "Speciální"),
        new("MB_SPECIAL_MOBILE",      "Mobilní hrdina",       "Otevři aplikaci na mobilním zařízení",                 "bi-phone",                15, "Speciální"),
        new("MB_SPECIAL_DARK_MODE",   "Temný rytíř",          "Přepni na tmavý režim",                                "bi-moon-fill",            10, "Speciální"),
        new("MB_SPECIAL_POWER_USER",  "Legendární hráč",      "Pouzij všechny sekce aplikace",                        "bi-person-gear",          35, "Speciální"),
        new("MB_SPECIAL_FIRST_WEEK",  "Týdenní dobrodružství","Hraj 7 dní za sebou",                                   "bi-calendar-week",        40, "Speciální"),
    };
}
