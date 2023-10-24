using FluentValidation;
using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.ClientContracts.Requests;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Services;
public class JobManager : IJobManager
{
    private readonly IRetriableHttpClient _httpClient;
    private IJobAccessor _jobAccessor;
    private readonly IValidator<StartJobModel> _startJobValidator;
    private readonly IValidator<CancelJobModel> _cancelJobValidator;

    public JobManager(
        IRetriableHttpClient httpClient,
        IJobAccessor jobAccessor,
        IValidator<StartJobModel> startJobValidator,
        IValidator<CancelJobModel> cancelJobValidator)
    {
        _httpClient = httpClient;
        _jobAccessor = jobAccessor;
        _startJobValidator = startJobValidator;
        _cancelJobValidator = cancelJobValidator;
    }

    public async Task<Guid> StartJobAsync(StartJobModel model, CancellationToken cancellation)
    {
        _startJobValidator.ValidateAndThrow(model);

        var jobId = Guid.NewGuid();
        await CreateInitialRecord(jobId, model.Text, cancellation);
        var data = new StartJobRequest(jobId);

        var httpResponseMessage = await _httpClient.PostAsync(Constants.JobClientName, "job/start", data, cancellation);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Can not start processing. Server unavailable.");
        }

        return jobId;
    }

    public async Task CancelJobAsync(CancelJobModel model, CancellationToken cancellation)
    {
        _cancelJobValidator.ValidateAndThrow(model);

        await _jobAccessor.UpdateToCanceledAsync(model.JobId, cancellation);
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
