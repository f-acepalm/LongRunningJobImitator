using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Mediator.Requests;
using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Handlers
{
    public class ConversionCanceledHandler : IRequestHandler<ConversionCanceledEvent>
    {
        private readonly ITextConversionBackgroundService _backgroundService;

        public ConversionCanceledHandler(ITextConversionBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public async Task Handle(ConversionCanceledEvent request, CancellationToken cancellationToken)
        {
            await _backgroundService.CancelProcessingAsync(request.JobId);
        }
    }
}
