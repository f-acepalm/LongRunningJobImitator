namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConversionWorker
    {
        Task StartJobAsync(Guid jobId, CancellationToken cancellation);
    }
}
