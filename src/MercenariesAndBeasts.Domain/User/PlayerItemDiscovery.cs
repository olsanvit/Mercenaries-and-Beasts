using System;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;

public class PlayerItemDiscovery : BaseGuid
{
    public Guid PlayerId { get; set; }           // PlayerProfile.Id
    public PlayerProfile Player { get; set; } = null!;

    public Guid TemplateId { get; set; }         // ItemTemplate.Id
    public Items.ItemTemplate Template { get; set; } = null!;

    public bool IsDiscovered { get; set; } = false;

    // kolikrát item padl (včetně prvního nálezu)
    public int TimesFound { get; set; } = 0;

    // “XP pro předmět”
    public long ItemXp { get; set; } = 0;

    // volitelně: rank/level collectible (badge, mastery)
    public int MasteryLevel { get; set; } = 1;

    public DateTime? FirstDiscoveredUtc { get; set; }
    public DateTime? LastFoundUtc { get; set; }
}