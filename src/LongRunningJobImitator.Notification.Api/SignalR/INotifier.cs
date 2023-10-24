namespace LongRunningJobImitator.Notification.Api.SignalR;

public interface INotifier
{
    Task SendDoneAsync(Guid jobId, CancellationToken cancellation);

    Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation);
}