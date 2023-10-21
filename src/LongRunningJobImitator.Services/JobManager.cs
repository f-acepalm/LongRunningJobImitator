using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services;
public class JobManager : IJobManager
{
    private ITextConversionBackgroundService _backgroundService;
    private IJobAccessor _jobAccessor;

    public JobManager(
        ITextConversionBackgroundService backgroundService,
        IJobAccessor jobAccessor)
    {
        _backgroundService = backgroundService;
        _jobAccessor = jobAccessor;
    }

    public async Task<Guid> StartJobAsync(StartJobModel model, CancellationToken cancellationToken)
    {
        var jobId = Guid.NewGuid();

        await CreateInitialRecord(jobId, model.Text, cancellationToken);
        await _backgroundService.StartProcessingAsync(jobId, model.Text);

        return jobId;
    }

    public async Task CancelJobAsync(CancelJobModel request, CancellationToken cancellationToken)
    {
        await _jobAccessor.UpdateToCanceledAsync(request.JobId, cancellationToken);
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
