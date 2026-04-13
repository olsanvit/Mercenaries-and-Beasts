
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Domain.Interface;
public interface IUnitAiGenerator
{
    Task<Dungeon> GenerateNextDungeonAsync(Dungeon previousDungeon, Dungeon currentDungeon, bool current,
    string? predefinedBeastsCsv = "", CancellationToken ct = default);
    Task<Location> GenerateNextLocationAsync(Location previousDungeon, Location currentDungeon, bool current, 
    string? predefinedOrdersCsv = "",CancellationToken ct = default);
    Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
    string entityKind,            // "dungeon", "location", "monster", "mercenary"...
    string nameEn,
    string? descriptionEn,
    List<string?> missing,
    
    CancellationToken ct = default);
    Task<ItemTemplate?> GenerateItemTemplateAsync(ItemTemplate tpl, string? groupHint, CancellationToken ct = default);
    Task<ItemUpgradeResourceNamesResult?> GenerateUpgradeResourceNamesAsync(
    ItemTemplate tpl,
    CancellationToken ct = default);
}