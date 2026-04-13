
using MercenariesAndBeasts.Domain.Progress;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerExpeditionProgress : BaseGuid
{
    public Guid PlayerId { get; set; }
    public PlayerProfile Player { get; set; } = null!;

    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public bool IsCompleted { get; set; } = false;

    public int WinStreak { get; set; } = 0;
    public int TotalWins { get; set; } = 0;
    public int TotalLosses { get; set; } = 0;

    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    // 1:1
    
    public ICollection<PlayerExpeditionAchievement> Achievements { get; set; } = new List<PlayerExpeditionAchievement>();
    public PlayerExpeditionEncounter Encounter { get; set; } = null!;

    // ✅ OFFERS (5)
    public Guid? EncounterOrder1TemplateId { get; set; }
    public Guid? EncounterOrder2TemplateId { get; set; }
    public Guid? EncounterOrder3TemplateId { get; set; }
    public Guid? EncounterOrder4TemplateId { get; set; }   // NEW
    public Guid? EncounterOrder5TemplateId { get; set; }   // NEW

    public DateTime? EncounterRolledUtc { get; set; }

    public int MaxStageReached { get; set; } = 0;
}