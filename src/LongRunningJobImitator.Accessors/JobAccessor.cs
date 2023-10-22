using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LongRunningJobImitator.Accessors;
public class JobAccessor : BaseAccessor<JobDoc>, IJobAccessor
{
    public JobAccessor(IOptions<DbSettings> settings) : base(settings)
    {
    }

    public async Task<JobDoc> GetAsync(Guid id, CancellationToken cancellation) =>
        await Collection.Find(x => x.Id == id).FirstAsync(cancellation);

    public async Task CreateAsync(JobDoc doc, CancellationToken cancellation) =>
        await Collection.InsertOneAsync(doc, cancellationToken: cancellation);

    public Task<UpdateResult> UpdateToInProgressAsync(Guid JobId, string conversionResult, CancellationToken cancellation)
    {
        var idFilter = GetIdFilter(JobId);
        var statusFilter = Builders<JobDoc>.Filter.Eq(x => x.Status, JobStatus.NotStarted);
        var filter = Builders<JobDoc>.Filter.And(idFilter, statusFilter);

        var update = Builders<JobDoc>.Update
            .Set(x => x.Status, JobStatus.InProgress)
            .Set(x => x.Result, conversionResult);

        return Collection.UpdateOneAsync(filter, update, cancellationToken: cancellation);
    }

    public Task<UpdateResult> UpdateProgressAsync(Guid JobId, int currentPosition, CancellationToken cancellation)
    {
        var idFilter = GetIdFilter(JobId);
        var statusFilter = Builders<JobDoc>.Filter.Eq(x => x.Status, JobStatus.InProgress);
        var filter = Builders<JobDoc>.Filter.And(idFilter, statusFilter);

        var update = Builders<JobDoc>.Update
            .Set(x => x.ProcessingPosition, currentPosition);

        return Collection.UpdateOneAsync(filter, update, cancellationToken: cancellation);
    }

    public Task<UpdateResult> UpdateToDoneAsync(Guid JobId, CancellationToken cancellation)
    {
        var update = Builders<JobDoc>.Update
            .Set(x => x.Status, JobStatus.Done);

        return Collection.UpdateOneAsync(GetIdFilter(JobId), update, cancellationToken: cancellation);
    }

    public Task<UpdateResult> UpdateToCanceledAsync(Guid JobId, CancellationToken cancellation)
    {
        var update = Builders<JobDoc>.Update
            .Set(x => x.Status, JobStatus.Canceled);

        return Collection.UpdateOneAsync(GetIdFilter(JobId), update, cancellationToken: cancellation);
    }

    private FilterDefinition<JobDoc> GetIdFilter(Guid JobId)
    {
        return Builders<JobDoc>.Filter.Eq(x => x.Id, JobId);
    }
}
