using FluentValidation;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Validation;
public class StartWorkerModelValidator : AbstractValidator<StartWorkerModel>
{
    public StartWorkerModelValidator()
    {
        RuleFor(x => x.JobId).NotEmpty();
    }
}
