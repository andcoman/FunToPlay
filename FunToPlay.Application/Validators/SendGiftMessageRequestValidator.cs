using FluentValidation;
using FunToPlay.Application.Messages;

namespace FunToPlay.Application.Validators;

public class SendGiftMessageRequestValidator : ValidatorBase<SendGiftRequest>
{
    public SendGiftMessageRequestValidator()
    {
        RuleFor(x => x.FriendPlayerId)
            .NotNull()
            .NotEmpty().WithMessage("FriendPlayerId is required.");

        RuleFor(x => x.ResourceType)
            .NotNull()
            .NotEmpty().WithMessage("ResourceType is required.")
            .Must(type => type == "coins" || type == "rolls")
            .WithMessage("ResourceType must be either 'coins' or 'rolls'.");

        RuleFor(x => x.ResourceValue)
            .InclusiveBetween(1, long.MaxValue)
            .WithMessage("ResourceValue must be between 1 and the maximum value a long can store.");
    }
}