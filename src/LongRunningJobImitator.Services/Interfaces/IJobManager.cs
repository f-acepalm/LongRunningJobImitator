using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces;
public interface IJobManager
{
    Task CancelJobAsync(Guid jobId, CancellationToken cancellation);

    Task<Guid> StartJobAsync(string text, CancellationToken cancellation);
}