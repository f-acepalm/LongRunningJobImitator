using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Api.Services
{
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


        public async Task SendCanceledAsync(Guid jobId)
        {
            await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionCanceled");
        }

        public async Task SendDoneAsync(Guid jobId)
        {
            await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionDone");
        }
    }
}
