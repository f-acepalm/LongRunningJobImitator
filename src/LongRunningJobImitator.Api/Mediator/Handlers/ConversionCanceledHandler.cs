using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Mediator.Requests;
using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Handlers
{
    public class ConversionCanceledHandler : IRequestHandler<ConversionCanceledEvent>
    {
        private readonly ITextConversionBackgroundService _backgroundService;
        private readonly IJobAccessor _jobAccessor;

        public ConversionCanceledHandler(
            ITextConversionBackgroundService backgroundService,
            IJobAccessor jobAccessor)
        {
            _backgroundService = backgroundService;
            _jobAccessor = jobAccessor;
        }

        public async Task Handle(ConversionCanceledEvent request, CancellationToken cancellationToken)
        {
            await _jobAccessor.UpdateToCanceledAsync(request.JobId, cancellationToken);
            //await _backgroundService.CancelProcessingAsync(request.JobId);
        }
    }
}
