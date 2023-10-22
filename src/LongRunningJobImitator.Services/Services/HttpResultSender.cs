using LongRunningJobImitator.ClientContracts.Requests;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace LongRunningJobImitator.Services.Services;
public class HttpResultSender : ITextConversionResultSender
{
    private readonly IRetriableHttpClient _httpClient;
    private readonly ILogger<HttpResultSender> _logger;
    private const string _notificationEndpoint = "Notification";

    public HttpResultSender(IRetriableHttpClient httpClient, ILogger<HttpResultSender> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task SendDoneAsync(Guid jobId, CancellationToken cancellation)
    {
        var data = new DoneNotificationRequest(jobId);
        var httpResponseMessage = await _httpClient.PostAsync(Constants.SignalRClientName, $"{_notificationEndpoint}/done", data, cancellation);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Cannot notify user. JobId: {jobId}");
        }
    }

    public async Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation)
    {
        var data = new ResultNotificationRequest(jobId, result);
        var httpResponseMessage = await _httpClient.PostAsync(Constants.SignalRClientName, $"{_notificationEndpoint}/result", data, cancellation);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Cannot notify user. JobId: {jobId}");
        }
    }
}
