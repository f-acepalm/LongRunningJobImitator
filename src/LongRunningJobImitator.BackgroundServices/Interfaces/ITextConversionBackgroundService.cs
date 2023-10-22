namespace LongRunningJobImitator.BackgroundServices.Interfaces;

public interface ITextConversionBackgroundService
{
    Task StartProcessingAsync(Guid jobId);

    Task CancelProcessingAsync(Guid jobId);
}
