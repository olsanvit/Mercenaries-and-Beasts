using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class ExpeditionGenerationOptions
{    public string Name { get; set; } = String.Empty;
    public int TargetMinLevel { get; set; }
    public int TargetMaxLevel { get; set; }

    public ElementType? FocusElement { get; set; }
}