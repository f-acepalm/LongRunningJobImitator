using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Mediator.Requests;
using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Handlers
{
    public class ConversionRequestedHandler : IRequestHandler<ConversionRequestedEvent>
    {
        private readonly ITextConversionBackgroundService _backgroundService;

        public ConversionRequestedHandler(ITextConversionBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public async Task Handle(ConversionRequestedEvent request, CancellationToken cancellationToken)
        {
            await _backgroundService.StartProcessingAsync(request.JobId, request.Text);
        }
    }
}
