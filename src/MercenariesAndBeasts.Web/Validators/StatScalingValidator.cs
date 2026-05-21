using FluentValidation;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Web.Validators;

/// <summary>Validates <see cref="ItemTemplate.StatScaling"/> on the Item Categories admin form.</summary>
public class StatScalingValidator : AbstractValidator<ItemTemplate.StatScaling>
{
    public StatScalingValidator()
    {
        RuleFor(x => x.HpPerLevel).GreaterThanOrEqualTo(0).WithMessage("HP/level must be >= 0.");
        RuleFor(x => x.AttackPerLevel).GreaterThanOrEqualTo(0).WithMessage("Attack/level must be >= 0.");
        RuleFor(x => x.DefensePerLevel).GreaterThanOrEqualTo(0).WithMessage("Defense/level must be >= 0.");
        RuleFor(x => x.SpeedPerLevel).GreaterThanOrEqualTo(0).WithMessage("Speed/level must be >= 0.");
        RuleFor(x => x.CritChancePerLevel)
            .InclusiveBetween(0, 1).WithMessage("Crit Chance/level must be between 0 and 1.");
        RuleFor(x => x.CritMultiplierPerLevel).GreaterThanOrEqualTo(0).WithMessage("Crit Multiplier/level must be >= 0.");
        RuleFor(x => x.ArmorPenetrationPerLevel).GreaterThanOrEqualTo(0).WithMessage("Armor Penetration/level must be >= 0.");
        RuleFor(x => x.AccuracyPerLevel).GreaterThanOrEqualTo(0).WithMessage("Accuracy/level must be >= 0.");
        RuleFor(x => x.EvasionPerLevel).GreaterThanOrEqualTo(0).WithMessage("Evasion/level must be >= 0.");
        RuleFor(x => x.BlockChancePerLevel)
            .InclusiveBetween(0, 1).WithMessage("Block Chance/level must be between 0 and 1.");
        RuleFor(x => x.DamageBonusPerLevel).GreaterThanOrEqualTo(0).WithMessage("Damage Bonus/level must be >= 0.");
        RuleFor(x => x.DamageReductionPerLevel).GreaterThanOrEqualTo(0).WithMessage("Damage Reduction/level must be >= 0.");
        RuleFor(x => x.TrueDamageBonusPerLevel).GreaterThanOrEqualTo(0).WithMessage("True Damage Bonus/level must be >= 0.");
        RuleFor(x => x.LifeStealPerLevel).GreaterThanOrEqualTo(0).WithMessage("Life Steal/level must be >= 0.");
        RuleFor(x => x.HpRegenPerLevel).GreaterThanOrEqualTo(0).WithMessage("HP Regen/level must be >= 0.");
        RuleFor(x => x.EnergyRegenPerLevel).GreaterThanOrEqualTo(0).WithMessage("Energy Regen/level must be >= 0.");
        RuleFor(x => x.TurnMeterGainPerLevel).GreaterThanOrEqualTo(0).WithMessage("Turn Meter Gain/level must be >= 0.");
        RuleFor(x => x.StatusChancePerLevel)
            .InclusiveBetween(0, 1).WithMessage("Status Chance/level must be between 0 and 1.");
        RuleFor(x => x.StatusDurationPerLevel).GreaterThanOrEqualTo(0).WithMessage("Status Duration/level must be >= 0.");
        RuleFor(x => x.StatusResistancePerLevel)
            .InclusiveBetween(0, 1).WithMessage("Status Resistance/level must be between 0 and 1.");
        RuleFor(x => x.DotDamagePerLevel).GreaterThanOrEqualTo(0).WithMessage("DoT Damage/level must be >= 0.");
    }
}
