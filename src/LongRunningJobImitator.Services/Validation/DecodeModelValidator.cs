using FluentValidation;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Validation;
public class DecodeModelValidator : AbstractValidator<DecodeModel>
{
    public DecodeModelValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
    }
}
