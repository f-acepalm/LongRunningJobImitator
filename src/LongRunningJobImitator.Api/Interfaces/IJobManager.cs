namespace LongRunningJobImitator.Api.Interfaces
{
    public interface IJobManager
    {
        Task CancelTextConversionAsync(Guid jobId);

        Task<Guid> RunTextConversionAsync(string text);
    }
}