namespace LongRunningJobImitator.BackgroundServices.Services;

public interface ITextConversionBackgroundService
{
    Task StartProcessingAsync(Guid jobId);

    Task CancelProcessingAsync(Guid jobId);
}
