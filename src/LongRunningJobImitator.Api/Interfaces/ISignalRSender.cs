namespace LongRunningJobImitator.Api.Interfaces;

public interface ISignalRSender
{
    Task SendDoneAsync(Guid jobId, CancellationToken cancellation);
    Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation);
}