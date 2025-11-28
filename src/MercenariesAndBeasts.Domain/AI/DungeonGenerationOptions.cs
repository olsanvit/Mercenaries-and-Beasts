using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class DungeonGenerationOptions
{
    public string Name { get; set; }
    public int TargetMinLevel { get; set; }
    public int TargetMaxLevel { get; set; }

    // volitelné, můžeš rozšířit:
    public ElementType? FocusElement { get; set; }
}