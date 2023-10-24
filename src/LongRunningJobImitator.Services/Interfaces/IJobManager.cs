using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces;
public interface IJobManager
{
    Task CancelJobAsync(CancelJobModel model, CancellationToken cancellation);

    Task<Guid> StartJobAsync(StartJobModel model, CancellationToken cancellation);
}