using FluentValidation;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Web.Validators;

/// <summary>Validates <see cref="ItemTemplate"/> on the Items admin form.</summary>
public class ItemTemplateValidator : AbstractValidator<ItemTemplate>
{
    public ItemTemplateValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(64).WithMessage("Code must be at most 64 characters.");

        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(128).WithMessage("English name must be at most 128 characters.");

        RuleFor(x => x)
            .Must(x => !(x.MercenarySlot.HasValue && x.MonsterSlot.HasValue))
            .WithMessage("Only one slot (MercenarySlot or MonsterSlot) may be set at a time.")
            .WithName("Slot");
    }
}
