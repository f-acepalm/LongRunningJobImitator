using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace LongRunningJobImitator.Services.Services;
public class HttpResultSender : ITextConversionResultSender
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpResultSender> _logger;
    private const string _notificationEndpoint = "Notification";

    public HttpResultSender(IHttpClientFactory httpClientFactory, ILogger<HttpResultSender> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task SendDoneAsync(Guid jobId, CancellationToken cancellation)
    {
        var data = new JobDoneModel(jobId);
        var httpResponseMessage = await SendRequestAsync($"{_notificationEndpoint}/done", data, cancellation);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Cannot notify user. JobId: {jobId}");
        }
    }

    public async Task SendResultAsync(Guid jobId, string result, CancellationToken cancellation)
    {
        var data = new ResultNotificationModel(jobId, result);
        var httpResponseMessage = await SendRequestAsync($"{_notificationEndpoint}/result", data, cancellation);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Cannot notify user. JobId: {jobId}");
        }
    }


    private async Task<HttpResponseMessage> SendRequestAsync<T>(string url, T data, CancellationToken cancellation)
    {
        var httpClient = _httpClientFactory.CreateClient(Constants.SignalRClientName);
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
