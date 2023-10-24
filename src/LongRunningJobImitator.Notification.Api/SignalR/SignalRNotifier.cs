using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Notification.Api.SignalR;
public class SignalRNotifier : INotifier
{
    private readonly IHubContext<TextConversionHub> _hub;

    public SignalRNotifier(IHubContext<TextConversionHub> hub)
    {
        _hub = hub;
    }

    public async Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation)
    {
        await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionResult", result, cancellation);
    }


    public async Task SendDoneAsync(Guid jobId, CancellationToken cancellation)
    {
        await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionDone", cancellation);
    }
}
