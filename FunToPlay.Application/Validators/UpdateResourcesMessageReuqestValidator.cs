using FluentValidation;
using FunToPlay.Application.Messages;

namespace FunToPlay.Application.Validators;

public class UpdateResourcesMessageReuqestValidator : ValidatorBase<UpdateResourcesRequest>
{
    public UpdateResourcesMessageReuqestValidator()
    {
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("ResourceType is required.")
            .Must(type => type == "coins" || type == "rolls")
            .WithMessage("ResourceType must be either 'coins' or 'rolls'.");

        RuleFor(x => x.ResourceValue)
            .InclusiveBetween(0, long.MaxValue)
            .WithMessage("ResourceValue must be between 0 and the maximum value a long can store.");
    }
}