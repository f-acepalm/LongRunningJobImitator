using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.ClientContracts.Requests;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace LongRunningJobImitator.Services.Services;
public class JobManager : IJobManager
{
    private readonly IRetriableHttpClient _httpClient;
    private readonly ILogger<JobManager> _logger;
    private IJobAccessor _jobAccessor;

    public JobManager(
        IRetriableHttpClient httpClient,
        ILogger<JobManager> logger,
        IJobAccessor jobAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jobAccessor = jobAccessor;
    }

    public async Task<Guid> StartJobAsync(string text, CancellationToken cancellation)
    {
        var jobId = Guid.NewGuid();
        await CreateInitialRecord(jobId, text, cancellation);
        var data = new StartJobRequest(jobId);

        var httpResponseMessage = await _httpClient.PostAsync(Constants.JobClientName, "job/start", data, cancellation);

        // TODO: better handling
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Cannot start job. JobId: {jobId}");
        }

        return jobId;
    }

    public async Task CancelJobAsync(Guid jobId, CancellationToken cancellation)
    {
        await _jobAccessor.UpdateToCanceledAsync(jobId, cancellation);
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
