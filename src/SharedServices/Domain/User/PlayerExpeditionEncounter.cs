

using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerExpeditionEncounter : BaseGuid
{
    public Guid PlayerId { get; set; }
    public PlayerProfile Player { get; set; } = null!;

    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    // 3 vybraní protivníci (orders)
    public Guid Target1MercenaryTemplateId { get; set; }
    public Guid Target2MercenaryTemplateId { get; set; }
    public Guid Target3MercenaryTemplateId { get; set; }

    // který index je boss (0/1/2)
    public int BossIndex { get; set; } = 2;

    // anti-refresh: když je třeba reroll, tak podle tohoto se pozná “nová sada”
    public int RollVersion { get; set; } = 1;

    public DateTime LastRolledUtc { get; set; } = DateTime.UtcNow;

    public Guid GetTargetId(int idx) => idx switch
    {
        0 => Target1MercenaryTemplateId,
        1 => Target2MercenaryTemplateId,
        2 => Target3MercenaryTemplateId,
        _ => throw new ArgumentOutOfRangeException(nameof(idx))
    };

    public void SetTargetId(int idx, Guid id)
    {
        if (idx == 0) Target1MercenaryTemplateId = id;
        else if (idx == 1) Target2MercenaryTemplateId = id;
        else if (idx == 2) Target3MercenaryTemplateId = id;
        else throw new ArgumentOutOfRangeException(nameof(idx));
    }
}