using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces;
public interface IJobManager
{
    Task CancelJobAsync(CancelJobModel request, CancellationToken cancellationToken);

    Task<Guid> StartJobAsync(StartJobModel model, CancellationToken cancellationToken);
}