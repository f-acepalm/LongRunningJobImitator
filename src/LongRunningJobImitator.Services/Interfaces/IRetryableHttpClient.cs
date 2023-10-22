namespace LongRunningJobImitator.Services.Interfaces;
public interface IRetriableHttpClient
{
    Task<HttpResponseMessage> PostAsync<T>(string clientName, string path, T data, CancellationToken cancellation);
}
