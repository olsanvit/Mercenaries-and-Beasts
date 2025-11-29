using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class ExpeditionStageDefinition
{
    public int StageNumber { get; set; }                // 1–10
    public string StageType { get; set; }  // ES1_… ES11_…

    public ExpeditionMercenaryDefinition Mercenary { get; set; } = new();
}