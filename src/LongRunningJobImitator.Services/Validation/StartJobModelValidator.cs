using FluentValidation;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Validation;
public class StartJobModelValidator : AbstractValidator<StartJobModel>
{
    public StartJobModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(100);
    }
}
