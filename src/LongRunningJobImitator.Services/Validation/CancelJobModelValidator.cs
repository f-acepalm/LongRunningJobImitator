using FluentValidation;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Validation;
public class CancelJobModelValidator : AbstractValidator<CancelJobModel>
{
    public CancelJobModelValidator()
    {
        RuleFor(x => x.JobId).NotEmpty();
    }
}
