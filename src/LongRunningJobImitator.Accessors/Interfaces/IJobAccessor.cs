using LongRunningJobImitator.Accessors.Models;

namespace LongRunningJobImitator.Accessors.Interfaces;
public interface IJobAccessor
{
    Task CreateAsync(JobDoc doc);

    Task<IEnumerable<JobDoc>> GetAllAsync();

    Task<JobDoc?> GetAsync(Guid id);

    Task RemoveAsync(Guid id);

    Task UpdateAsync(Guid id, JobDoc doc);
}
