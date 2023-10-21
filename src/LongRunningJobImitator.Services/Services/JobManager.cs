using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;

namespace LongRunningJobImitator.Services.Services;
public class JobManager : IJobManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<JobManager> _logger;
    private IJobAccessor _jobAccessor;

    public JobManager(
        IHttpClientFactory httpClientFactory,
        ILogger<JobManager> logger,
        IJobAccessor jobAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _jobAccessor = jobAccessor;
    }

    public async Task<Guid> StartJobAsync(string text, CancellationToken cancellation)
    {
        var jobId = Guid.NewGuid();
        await CreateInitialRecord(jobId, text, cancellation);
        var data = new StartJobModel(jobId, text);

        var httpResponseMessage = await SendRequestAsync("job/start", data, cancellation);

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

    private async Task<HttpResponseMessage> SendRequestAsync<T>(string url, T data, CancellationToken cancellation)
    {
        var httpClient = _httpClientFactory.CreateClient(Constants.JobClientName);
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            Application.Json);

        var httpResponseMessage = await httpClient.PostAsync(
            url,
            content,
            cancellation);
        return httpResponseMessage;
    }
}
