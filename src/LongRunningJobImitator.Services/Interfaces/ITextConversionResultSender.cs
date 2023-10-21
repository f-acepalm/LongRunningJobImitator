namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConversionResultSender
    {
        Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation);

        Task SendDoneAsync(Guid jobId, CancellationToken cancellation);
    }
}
