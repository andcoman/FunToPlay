using FluentValidation;
using FunToPlay.Application.Messages;

namespace FunToPlay.Application.Validators;

public class ValidatorBase<T> : AbstractValidator<T> where T : MessageBase
{
    public ValidatorBase()
    {
        RuleFor(x => x.Metadata)
            .NotNull().WithMessage("Metadata is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Metadata!.Id)
                    .NotEmpty().WithMessage("Metadata.Id is required");

                RuleFor(x => x.Metadata!.Version)
                    .GreaterThan(0).WithMessage("Metadata.Version is required");
            });
    }
}