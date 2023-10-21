using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Api.Services;
public class SignalRSender : ISignalRSender
{
    private readonly IHubContext<TextConversionHub> _hub;

    public SignalRSender(IHubContext<TextConversionHub> hub)
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
