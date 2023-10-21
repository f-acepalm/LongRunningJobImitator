using LongRunningJobImitator.Accessors.Models;
using MongoDB.Driver;

namespace LongRunningJobImitator.Accessors.Interfaces;
public interface IJobAccessor
{
    Task CreateAsync(JobDoc doc, CancellationToken cancellation);

    Task<JobDoc> GetAsync(Guid id, CancellationToken cancellation);

    Task<UpdateResult> UpdateToInProgressAsync(Guid JobId, string conversionResult, CancellationToken cancellation);

    Task<UpdateResult> UpdateToDoneAsync(Guid JobId, CancellationToken cancellation);

    Task<UpdateResult> UpdateToCanceledAsync(Guid JobId, CancellationToken cancellation);

    Task<UpdateResult> UpdateProgressAsync(Guid JobId, int currentPosition, CancellationToken cancellation);
}
