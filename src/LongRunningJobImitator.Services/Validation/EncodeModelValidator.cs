using FluentValidation;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Validation;
public class EncodeModelValidator : AbstractValidator<EncodeModel>
{
    public EncodeModelValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
    }
}
