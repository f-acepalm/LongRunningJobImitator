using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Mediator.Requests;
using MediatR;

namespace LongRunningJobImitator.Api.Mediator.Handlers;

public class ConversionRequestedHandler : IRequestHandler<ConversionRequestedEvent>
{
    private readonly ITextConversionBackgroundService _backgroundService;
    private readonly IJobAccessor _jobAccessor;

    public ConversionRequestedHandler(
        ITextConversionBackgroundService backgroundService,
        IJobAccessor jobAccessor)
    {
        _backgroundService = backgroundService;
        _jobAccessor = jobAccessor;
    }

    public async Task Handle(ConversionRequestedEvent request, CancellationToken cancellationToken)
    {
        await CreateInitialRecord(request.JobId, request.Text, cancellationToken);
        await _backgroundService.StartProcessingAsync(request.JobId, request.Text);
    }

    private async Task CreateInitialRecord(Guid jobId, string text, CancellationToken cancellationToken)
    {
        await _jobAccessor.CreateAsync(new(
                        jobId,
                        JobStatus.NotStarted,
                        text,
                        string.Empty,
                        0),
                        cancellationToken);
    }
}
