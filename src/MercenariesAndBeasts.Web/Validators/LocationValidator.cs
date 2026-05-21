using FluentValidation;
using MercenariesAndBeasts.Domain;

namespace MercenariesAndBeasts.Web.Validators;

/// <summary>Validates <see cref="Location"/> on the Locations admin form.</summary>
public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(64).WithMessage("Code must be at most 64 characters.");

        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(128).WithMessage("English name must be at most 128 characters.");

        RuleFor(x => x.MinLevel)
            .GreaterThan(0).WithMessage("Min level must be greater than 0.");

        RuleFor(x => x.MaxLevel)
            .GreaterThanOrEqualTo(x => x.MinLevel)
            .WithMessage("Max level must be greater than or equal to Min level.");
    }
}
