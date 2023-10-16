using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Api.Services
{
    public class SignalRResultSender : IConversionResultSender
    {
        private readonly IHubContext<TextConversionHub> _hub;

        public SignalRResultSender(IHubContext<TextConversionHub> hub)
        {
            _hub = hub;
        }

        public async Task SendAsync(Guid jobId, string result)
        {
            await _hub.Clients.Group(jobId.ToString()).SendAsync("ConversionResult", result);
        }
    }
}
