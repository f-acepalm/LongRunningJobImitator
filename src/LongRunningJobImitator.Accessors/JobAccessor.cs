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

    public async Task<IEnumerable<JobDoc>> GetAllAsync() =>
        await Collection.Find(_ => true).ToListAsync();

    public async Task<JobDoc?> GetAsync(Guid id) =>
        await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(JobDoc doc) =>
        await Collection.InsertOneAsync(doc);

    public async Task UpdateAsync(Guid id, JobDoc doc) =>
        await Collection.ReplaceOneAsync(x => x.Id == id, doc);

    public async Task RemoveAsync(Guid id) =>
        await Collection.DeleteOneAsync(x => x.Id == id);
}
