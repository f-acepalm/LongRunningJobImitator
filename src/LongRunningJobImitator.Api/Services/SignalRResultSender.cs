using LongRunningJobImitator.Api.SignalR;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Api.Services;
public class SignalRResultSender : ITextConversionResultSender
{
    private readonly IHubContext<TextConversionHub> _hub;

    public SignalRResultSender(IHubContext<TextConversionHub> hub)
    {
        _hub = hub;
    }

    public async Task SendResultAsync(Guid jobId, string result)
    {
        await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionResult", result);
    }


    public async Task SendDoneAsync(Guid jobId)
    {
        await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionDone");
    }
}
