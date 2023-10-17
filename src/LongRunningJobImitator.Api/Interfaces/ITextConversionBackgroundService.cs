namespace LongRunningJobImitator.Api.Interfaces
{
    public interface ITextConversionBackgroundService
    {
        Task StartProcessingAsync(Guid jobId, string text);

        Task CancelProcessingAsync(Guid jobId);
    }
}
