using LongRunningJobImitator.Services.Interfaces;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace LongRunningJobImitator.Services.Services;
public class RetriableHttpClient : IRetriableHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RetriableHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string clientName, string path, T data, CancellationToken cancellation)
    {
        var httpClient = _httpClientFactory.CreateClient(clientName);
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            Application.Json);

        var httpResponseMessage = await httpClient.PostAsync(
            path,
            content,
            cancellation);

        return httpResponseMessage;
    }
}
