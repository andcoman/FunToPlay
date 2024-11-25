using FluentValidation;
using FunToPlay.Application.Messages;

namespace FunToPlay.Application.Validators;

public class LoginMessageRequestValidator : ValidatorBase<LoginRequest>
{
    public LoginMessageRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotNull().WithMessage("DeviceId is required")
            .NotEmpty().WithMessage("DeviceId is required");
    }
}